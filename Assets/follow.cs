using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour
{
    public GameObject target;
    public float back = 45f;
    public float up = 10f;
    private Vector3 velocity = Vector3.zero;
    private Vector3 velocity2 = Vector3.zero;
    Vector3 smoothedTargetPos;

    // Update is called once per frame
    void LateUpdate()
    {
        
        smoothedTargetPos = Vector3.SmoothDamp(smoothedTargetPos, target.transform.position, ref velocity2, 0.1f);
        transform.LookAt(smoothedTargetPos);

        Vector3 point = target.transform.position - target.transform.forward * back + target.transform.up * up;
        transform.position = Vector3.SmoothDamp(transform.position, point, ref velocity, 0.1f);
    }
}
