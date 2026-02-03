using UnityEngine;

public class PushableCart : MonoBehaviour
{
    [Header("Setup")]
    public Transform handle;
    public Transform cartRoot;
    public bool useKinematicWhilePushed = true;

    private Rigidbody rb;
    private Transform originalParent;
    private bool wasKinematic;

    private void Awake()
    {
        if (cartRoot == null)
        {
            cartRoot = transform;
        }

        rb = cartRoot.GetComponent<Rigidbody>();
        originalParent = cartRoot.parent;
        if (rb != null)
        {
            wasKinematic = rb.isKinematic;
        }
    }

    public Vector3 GetHandlePosition()
    {
        return handle != null ? handle.position : cartRoot.position;
    }

    public Vector3 GetForward()
    {
        return cartRoot.forward;
    }

    public void BeginPush()
    {
        if (useKinematicWhilePushed && rb != null)
        {
            wasKinematic = rb.isKinematic;
            if (!rb.isKinematic)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            rb.isKinematic = true;
        }
    }

    public void EndPush()
    {
        if (useKinematicWhilePushed && rb != null)
        {
            rb.isKinematic = wasKinematic;
        }
        cartRoot.SetParent(originalParent, true);
    }

    public void MoveBy(Vector3 delta)
    {
        if (rb != null && !rb.isKinematic)
        {
            rb.MovePosition(rb.position + delta);
        }
        else
        {
            cartRoot.position += delta;
        }
    }

    public void RotateTowards(Vector3 direction, float turnSpeed)
    {
        if (cartRoot == null)
        {
            return;
        }

        direction.y = 0f;
        if (direction.sqrMagnitude < 0.000001f)
        {
            return;
        }

        Quaternion target = Quaternion.LookRotation(direction.normalized, Vector3.up);
        float maxDegrees = Mathf.Max(0f, turnSpeed) * Time.deltaTime * 60f;
        if (rb != null && !rb.isKinematic)
        {
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, target, maxDegrees));
        }
        else
        {
            cartRoot.rotation = Quaternion.RotateTowards(cartRoot.rotation, target, maxDegrees);
        }
    }
}
