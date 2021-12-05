using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smoothSpeed = 10f;
    public Vector3 offset;
    public bool _2DMode = false;

    public Transform target;

    private void FixedUpdate()
    {
        Vector3 desiredPos = target.position + offset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPos;

        if(!_2DMode) transform.LookAt(target);
    }
}
