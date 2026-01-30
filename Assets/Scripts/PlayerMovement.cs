using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3.5f;
    public float rotationSpeed = 12f;
    public float gravity = -20f;

    [Header("References")]
    public Transform cameraTransform;

    [Header("Animator Params")]
    public string speedParam = "Speed";
    public string groundedParam = "IsGrounded";
    public string isMovingParam = "IsMoving";

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;
    private bool hasSpeedParam;
    private bool hasGroundedParam;
    private bool hasIsMovingParam;
    private bool lockToForward;
    private Transform forwardLock;
    private bool hasSpeedOverride;
    private float savedSpeed;
    private bool movementBlocked;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            foreach (var param in animator.parameters)
            {
                if (param.name == speedParam)
                {
                    hasSpeedParam = true;
                }
                else if (param.name == groundedParam)
                {
                    hasGroundedParam = true;
                }
                else if (param.name == isMovingParam)
                {
                    hasIsMovingParam = true;
                }
            }
        }
    }

    private void Update()
    {
        Vector2 input = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        if (movementBlocked)
        {
            if (controller != null)
            {
                controller.Move(Vector3.zero);
            }
            velocity = Vector3.zero;
            bool groundedBlocked = controller != null && controller.isGrounded;
            if (animator != null)
            {
                if (hasSpeedParam)
                {
                    animator.SetFloat(speedParam, 0f);
                }
                if (hasGroundedParam)
                {
                    animator.SetBool(groundedParam, groundedBlocked);
                }
                if (hasIsMovingParam)
                {
                    animator.SetBool(isMovingParam, false);
                }
            }
            return;
        }

        Vector3 move = Vector3.zero;
        if (lockToForward && forwardLock != null)
        {
            float forwardInput = input.y;
            Vector3 forward = forwardLock.forward;
            forward.y = 0f;
            if (forward.sqrMagnitude > 0.001f)
            {
                forward.Normalize();
                move = forward * forwardInput;
                Quaternion targetRot = Quaternion.LookRotation(forward, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            }
        }
        else if (input.sqrMagnitude > 0.0001f)
        {
            input = input.normalized;
            if (cameraTransform != null)
            {
                Vector3 camForward = cameraTransform.forward;
                Vector3 camRight = cameraTransform.right;
                camForward.y = 0f;
                camRight.y = 0f;
                camForward.Normalize();
                camRight.Normalize();
                move = camForward * input.y + camRight * input.x;
            }
            else
            {
                move = new Vector3(input.x, 0f, input.y);
            }

            Quaternion targetRot = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        controller.Move(move * moveSpeed * Time.deltaTime);

        bool grounded = controller.isGrounded;
        if (grounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        float moveAmount = move.magnitude;
        bool isMoving = moveAmount > 0.01f;

        if (animator != null)
        {
            if (hasSpeedParam)
            {
                animator.SetFloat(speedParam, moveAmount);
            }
            if (hasGroundedParam)
            {
                animator.SetBool(groundedParam, grounded);
            }
            if (hasIsMovingParam)
            {
                animator.SetBool(isMovingParam, isMoving);
            }
        }
    }

    public void SetForwardLock(Transform lockTransform, float speedOverride = -1f)
    {
        lockToForward = true;
        forwardLock = lockTransform;
        if (speedOverride > 0f)
        {
            if (!hasSpeedOverride)
            {
                savedSpeed = moveSpeed;
                hasSpeedOverride = true;
            }
            moveSpeed = speedOverride;
        }
    }

    public void ClearForwardLock()
    {
        lockToForward = false;
        forwardLock = null;
        if (hasSpeedOverride)
        {
            moveSpeed = savedSpeed;
            hasSpeedOverride = false;
        }
    }

    public void SetMovementBlocked(bool blocked)
    {
        movementBlocked = blocked;
    }

    public void Teleport(Vector3 position)
    {
        if (controller != null)
        {
            controller.enabled = false;
        }
        transform.position = position;
        velocity = Vector3.zero;
        if (controller != null)
        {
            controller.enabled = true;
        }
    }
}
