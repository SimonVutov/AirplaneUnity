using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;
    private int followDist = 8;
    private float followSpeed = 5.0f;  // Adjustable speed for smoother following

    void LateUpdate()
    {
        // Smoothly rotate towards the target each frame
        transform.LookAt(target.position + transform.up * 2.0f);

        // Calculate the distance from the camera to the target
        float distance = Vector3.Distance(transform.position, target.position + transform.up * 2.0f);

        if (distance > followDist)
        {
            // Calculate the amount to move closer to the follow distance
            float step = followSpeed * Time.deltaTime * (distance - followDist);
            
            // Move the camera forward by 'step' units towards the target
            transform.position += transform.forward * step;
        }
    }
}
