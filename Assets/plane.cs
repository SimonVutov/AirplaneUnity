using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plane : MonoBehaviour
{
    Rigidbody rb;
    float thrustMultiplyer = 40f;
    float liftMultiplyer = 0.1f;
    float roll = 5f;
    float pitch = 35f;
    float verticalStabilzer = 0.2f;
    public GameObject[] wings;
    public ParticleSystem particles;

    private Vector2 velocity = Vector3.zero;
    Vector2 smoothedPitchRoll;
    Vector3 verticalStabilzerDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float thrust = 0;
        if (Input.GetButton("Jump"))
        {
            thrust = 1;
            if (!particles.isPlaying) particles.Play();
        }
        else
        {
            thrust = 0;
            if (particles.isPlaying) particles.Stop();
        }

        rb.AddForce(transform.forward * thrust * thrustMultiplyer);

        Vector2 pitchRoll;
        pitchRoll.x = Input.GetAxisRaw("Horizontal");
        pitchRoll.y = Input.GetAxisRaw("Vertical");

        smoothedPitchRoll = Vector2.SmoothDamp(smoothedPitchRoll, pitchRoll, ref velocity, 0.1f);

        for (int i = 0; i < 6; i++)
        {
            if (i == 2 || i == 3) wings[i].transform.localEulerAngles = new Vector3(pitch * -smoothedPitchRoll.y, 0, 0);
            if (i == 0) wings[i].transform.localEulerAngles = new Vector3(roll * smoothedPitchRoll.x, 0, 0);
            if (i == 1) wings[i].transform.localEulerAngles = new Vector3(roll * -smoothedPitchRoll.x, 0, 0);

            if (i == 4 || i == 5) rb.AddForceAtPosition(-wings[i].transform.up * wings[i].transform.InverseTransformDirection(rb.linearVelocity).y * liftMultiplyer * transform.localScale.magnitude * verticalStabilzer, wings[i].transform.position);
            else rb.AddForceAtPosition(-wings[i].transform.up * wings[i].transform.InverseTransformDirection(rb.linearVelocity).y * liftMultiplyer * transform.localScale.magnitude, wings[i].transform.position);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draws a blue line from this transform to the target
        Gizmos.color = Color.white;
        Gizmos.DrawLine(wings[5].transform.position, wings[5].transform.position + verticalStabilzerDirection);
    }
}