using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interact")]
    public KeyCode interactKey = KeyCode.E;
    public float interactRange = 1.2f;
    public LayerMask pushableMask = ~0;

    [Header("Pushing")]
    public float pushSpeed = 2.0f;
    public float pushTurnSpeed = 8f;
    public float directionSmoothTime = 0.08f;
    public string pushParam = "IsPushing";
    public string startPushTrigger = "StartPush";
    public string stopPushTrigger = "StopPush";
    public float standBackOffset = 0.6f;
    public LayerMask groundMask = ~0;
    public float groundSnapDistance = 3f;
    public bool autoStopOnRelease = false;
    public bool ignoreCartCollision = false;

    private PlayerMovement movement;
    private Animator animator;
    private PushableCart currentCart;
    private bool isPushing;
    private bool hasPushParam;
    private bool hasStartPushTrigger;
    private bool hasStopPushTrigger;
    private CharacterController characterController;
    private readonly List<Collider> ignoredColliders = new List<Collider>();
    private bool pushAnimStarted;
    private Vector3 smoothedPushDir;
    private Vector3 smoothedPushDirVelocity;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
        if (animator != null)
        {
            animator.applyRootMotion = false;
        }
        characterController = GetComponent<CharacterController>();
        if (animator != null)
        {
            foreach (var param in animator.parameters)
            {
                if (param.name == pushParam)
                {
                    hasPushParam = true;
                }
                else if (param.name == startPushTrigger)
                {
                    hasStartPushTrigger = true;
                }
                else if (param.name == stopPushTrigger)
                {
                    hasStopPushTrigger = true;
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            if (isPushing)
            {
                StopPush();
            }
            else
            {
                TryStartPush();
            }
        }
    }

    private void LateUpdate()
    {
        if (!isPushing || currentCart == null)
        {
            return;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 pushDir = GetPushDirection(input);
        smoothedPushDir = Vector3.SmoothDamp(smoothedPushDir, pushDir, ref smoothedPushDirVelocity, directionSmoothTime);
        if (pushDir.sqrMagnitude > 0.000001f)
        {
            if (!pushAnimStarted && hasStartPushTrigger && animator != null)
            {
                animator.ResetTrigger(stopPushTrigger);
                animator.SetTrigger(startPushTrigger);
                pushAnimStarted = true;
                animator.speed = 1f;
            }

            float inputMag = Mathf.Clamp01(input.magnitude);
            Vector3 delta = pushDir * inputMag * pushSpeed * Time.deltaTime;
            currentCart.MoveBy(delta);
            if (smoothedPushDir.sqrMagnitude > 0.000001f)
            {
                currentCart.RotateTowards(smoothedPushDir, pushTurnSpeed);
                RotatePlayerTowards(smoothedPushDir);
            }
        }
        else if (pushAnimStarted)
        {
            if (autoStopOnRelease)
            {
                StopPush();
                return;
            }
            if (hasStopPushTrigger && animator != null)
            {
                animator.ResetTrigger(startPushTrigger);
                animator.SetTrigger(stopPushTrigger);
            }
            pushAnimStarted = false;
        }

        Vector3 facing = smoothedPushDir.sqrMagnitude > 0.000001f ? smoothedPushDir : GetFlatForward(currentCart);
        Vector3 targetPos = GetSafePushPosition(currentCart, facing);
        Vector3 correction = targetPos - transform.position;
        correction.y = 0f;
        if (correction.sqrMagnitude > 0.000001f)
        {
            if (characterController != null)
            {
                characterController.Move(correction);
            }
            else
            {
                transform.position += correction;
            }
        }
    }

    private void TryStartPush()
    {
        PushableCart cart = FindNearestCart();
        if (cart == null)
        {
            return;
        }

        StartPush(cart);
    }

    private PushableCart FindNearestCart()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRange, pushableMask, QueryTriggerInteraction.Ignore);
        PushableCart nearest = null;
        float bestDist = float.MaxValue;

        foreach (var hit in hits)
        {
            PushableCart cart = hit.GetComponentInParent<PushableCart>();
            if (cart == null)
            {
                continue;
            }

            Vector3 handlePos = cart.GetHandlePosition();
            float d = Vector3.Distance(transform.position, handlePos);
            if (d < bestDist)
            {
                bestDist = d;
                nearest = cart;
            }
        }

        return nearest;
    }

    private void StartPush(PushableCart cart)
    {
        currentCart = cart;
        isPushing = true;
        pushAnimStarted = false;

        cart.BeginPush();

        if (ignoreCartCollision)
        {
            SetCartCollisionIgnored(cart, true);
        }

        Vector3 forward = GetFlatForward(cart);
        Vector3 targetPos = GetSafePushPosition(cart, forward);
        if (movement != null)
        {
            movement.Teleport(targetPos);
            movement.SetForwardLock(cart.transform, pushSpeed);
            movement.SetMovementBlocked(true);
        }

        if (hasPushParam && animator != null)
        {
            animator.SetBool(pushParam, true);
        }
        if (animator != null)
        {
            animator.speed = 1f;
        }

    }

    private void StopPush()
    {
        isPushing = false;
        pushAnimStarted = false;

        if (movement != null)
        {
            movement.ClearForwardLock();
            movement.SetMovementBlocked(false);
        }

        if (currentCart != null)
        {
            Vector3 forward = GetFlatForward(currentCart);
            Vector3 targetPos = GetSafePushPosition(currentCart, forward);
            if (movement != null)
            {
                movement.Teleport(targetPos);
            }
            else
            {
                transform.position = targetPos;
            }
        }

        if (ignoreCartCollision)
        {
            SetCartCollisionIgnored(currentCart, false);
        }

        if (hasPushParam && animator != null)
        {
            animator.SetBool(pushParam, false);
        }
        if (hasStopPushTrigger && animator != null)
        {
            animator.ResetTrigger(startPushTrigger);
            animator.SetTrigger(stopPushTrigger);
        }
        if (animator != null)
        {
            animator.speed = 1f;
        }

        if (currentCart != null)
        {
            currentCart.EndPush();
        }

        currentCart = null;
    }

    private void SetCartCollisionIgnored(PushableCart cart, bool ignore)
    {
        if (characterController == null || cart == null)
        {
            return;
        }

        if (ignore)
        {
            ignoredColliders.Clear();
            Transform cartRoot = cart.cartRoot != null ? cart.cartRoot : cart.transform;
            if (cartRoot == null)
            {
                return;
            }

            Collider[] colliders = cartRoot.GetComponentsInChildren<Collider>();
            foreach (var collider in colliders)
            {
                if (collider == null || collider.isTrigger)
                {
                    continue;
                }
                Physics.IgnoreCollision(characterController, collider, true);
                ignoredColliders.Add(collider);
            }
        }
        else
        {
            foreach (var collider in ignoredColliders)
            {
                if (collider == null)
                {
                    continue;
                }
                Physics.IgnoreCollision(characterController, collider, false);
            }
            ignoredColliders.Clear();
        }
    }

    private Vector3 GetFlatForward(PushableCart cart)
    {
        Vector3 forward = Vector3.zero;
        Transform cartRoot = cart.cartRoot != null ? cart.cartRoot : cart.transform;
        if (cartRoot != null)
        {
            Vector3 handlePos = cart.GetHandlePosition();
            Vector3 toCenter = cartRoot.position - handlePos;
            toCenter.y = 0f;
            if (toCenter.sqrMagnitude > 0.001f)
            {
                forward = toCenter.normalized;
            }
        }

        if (forward.sqrMagnitude < 0.001f)
        {
            forward = cart.GetForward();
        }
        forward.y = 0f;
        if (forward.sqrMagnitude < 0.001f)
        {
            forward = cart.transform.forward;
            forward.y = 0f;
        }
        if (forward.sqrMagnitude < 0.001f)
        {
            forward = Vector3.forward;
        }
        return forward.normalized;
    }

    private Vector3 GetSafePushPosition(PushableCart cart, Vector3 forward)
    {
        Vector3 targetPos = cart.GetHandlePosition() - forward * standBackOffset;
        targetPos.y = transform.position.y;

        Transform cartRoot = cart.cartRoot != null ? cart.cartRoot : cart.transform;
        if (cartRoot == null)
        {
            return SnapToGround(targetPos);
        }

        Collider[] colliders = cartRoot.GetComponentsInChildren<Collider>();
        bool hasBounds = false;
        Bounds bounds = new Bounds();
        foreach (var collider in colliders)
        {
            if (collider == null || collider.isTrigger)
            {
                continue;
            }
            if (!hasBounds)
            {
                bounds = collider.bounds;
                hasBounds = true;
            }
            else
            {
                bounds.Encapsulate(collider.bounds);
            }
        }

        if (!hasBounds)
        {
            return SnapToGround(targetPos);
        }

        float forwardExtent =
            Mathf.Abs(forward.x) * bounds.extents.x +
            Mathf.Abs(forward.y) * bounds.extents.y +
            Mathf.Abs(forward.z) * bounds.extents.z;
        float buffer = (characterController != null ? characterController.radius : 0.3f) + 0.05f;
        targetPos = bounds.center - forward * (forwardExtent + buffer + standBackOffset);
        targetPos.y = transform.position.y;

        return SnapToGround(targetPos);
    }

    private Vector3 SnapToGround(Vector3 position)
    {
        float castHeight = groundSnapDistance;
        Vector3 origin = position + Vector3.up * castHeight;
        RaycastHit[] hits = Physics.RaycastAll(origin, Vector3.down, castHeight * 2f, groundMask, QueryTriggerInteraction.Ignore);
        if (hits.Length > 0)
        {
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
            RaycastHit hit = default;
            bool found = false;
            foreach (var h in hits)
            {
                if (h.collider == null)
                {
                    continue;
                }
                if (ignoredColliders.Contains(h.collider))
                {
                    continue;
                }
                hit = h;
                found = true;
                break;
            }

            if (found)
            {
                if (characterController != null)
                {
                    position.y = hit.point.y - characterController.center.y + characterController.height * 0.5f;
                }
                else
                {
                    position.y = hit.point.y;
                }
            }
        }
        return position;
    }

    private Vector3 GetPushDirection(Vector2 input)
    {
        if (input.sqrMagnitude < 0.0001f)
        {
            return Vector3.zero;
        }

        if (movement != null && movement.cameraTransform != null)
        {
            Vector3 camForward = movement.cameraTransform.forward;
            Vector3 camRight = movement.cameraTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();
            Vector3 dir = camForward * input.y + camRight * input.x;
            if (dir.sqrMagnitude > 0.0001f)
            {
                return dir.normalized;
            }
        }

        Vector3 fallback = new Vector3(input.x, 0f, input.y);
        return fallback.sqrMagnitude > 0.0001f ? fallback.normalized : Vector3.zero;
    }

    private void RotatePlayerTowards(Vector3 direction)
    {
        direction.y = 0f;
        if (direction.sqrMagnitude < 0.000001f)
        {
            return;
        }

        Quaternion target = Quaternion.LookRotation(direction.normalized, Vector3.up);
        float t = Mathf.Clamp01(pushTurnSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, t);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }

}
