using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;         // Assign the player transform in the Inspector
    public Vector3 offset = new Vector3(0, 0, -10f);
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}