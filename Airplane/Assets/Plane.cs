using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Plane : MonoBehaviour
{

    public InputMaster controls;
    public Wing[] wings;
    public Wheel[] wheels;
    Rigidbody rb;
    Vector3 input;
    Vector3 assistInput;
    float throttle = 0;
    float wingStrength = 0.2f;
    float jetPower = 4f;
    bool assists = true;
    bool braking = false;

    public ParticleSystem ps;
    
    void Awake()
    {
        controls = new InputMaster();
        controls.Player.RollPitchYaw.performed += ctx => SetInput(ctx.ReadValue<Vector3>());
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        foreach (var wing in wings)
        {
            wing.def = wing.Object.transform.localEulerAngles;
        }
    }

    Vector2 ClampVector(Vector2 vector)
    {
        // Clamp each component of the vector between -1 and 1
        vector.x = Mathf.Clamp(vector.x, -1, 1);
        vector.y = Mathf.Clamp(vector.y, -1, 1);
        return vector;
    }
    Vector3 ClampVector(Vector3 vector)
    {
        // Clamp each component of the vector between -1 and 1
        vector.x = Mathf.Clamp(vector.x, -1, 1);
        vector.y = Mathf.Clamp(vector.y, -1, 1);
        vector.z = Mathf.Clamp(vector.z, -1, 1);
        return vector;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space)) throttle = 1;
        else throttle = 0;

        if (throttle > 0.5f) ps.Play();
        else ps.Stop();
        
        float speedReduce = (200 - Mathf.Clamp(rb.velocity.magnitude, 0, 192)) / 200;
        Debug.Log(speedReduce);

        // Get the angular velocity in world coordinates
        Vector3 angularVelocityWorld = rb.angularVelocity;
        // Convert the world angular velocity to local angular velocity
        Vector3 angularVelocityLocal = rb.transform.InverseTransformDirection(angularVelocityWorld);
        // Extract local roll and pitch speeds
        float rollSpeed = angularVelocityLocal.z;   // Rotation around local z-axis (roll)
        float pitchSpeed = -angularVelocityLocal.x;  // Rotation around local x-axis (pitch)
        float yawSpeed = angularVelocityLocal.y;    // Rotation around local y-axis (yaw)

        Vector3 rollPitchSpeed = ClampVector(new Vector3(rollSpeed * 0.4f * Mathf.Pow(speedReduce, 0.3f), pitchSpeed * 0.4f, yawSpeed * 0.4f));
        assistInput.x *= Mathf.Pow(speedReduce, 4); // reduce roll at high speeds
        if (assists) assistInput = ClampVector(input + rollPitchSpeed);
        else assistInput = input;
    }

    void FixedUpdate()
    {
        foreach (var wing in wings)
        {
            Vector3 velocityInWingLocal = wing.Object.transform.InverseTransformDirection(rb.velocity);
            Vector3 upwardForceLocal = new Vector3(0, wingStrength, 0);
            Vector3 upwardForceWorld = wing.Object.transform.TransformDirection(upwardForceLocal);
            rb.AddForceAtPosition((-1) * upwardForceWorld * velocityInWingLocal.y, wing.Object.transform.position);
            // control surface rotation
            wing.Object.transform.localEulerAngles = new Vector3(wing.def.x + wing.degreesOfMotionRollPitchYaw.x * assistInput.x + wing.degreesOfMotionRollPitchYaw.y * assistInput.y, wing.def.y + wing.degreesOfMotionRollPitchYaw.z * assistInput.z, wing.def.z);

            // thrust
            rb.AddForce(transform.forward * throttle * jetPower);

            Debug.DrawRay(wing.Object.transform.position, upwardForceWorld * 10, Color.red); // Scale up force for visibility
        }
        foreach (var wheel in wheels)
        {
            RaycastHit hit;
            if (Physics.Raycast(wheel.LocationObject.transform.position, -transform.up, out hit, wheel.height))
            {
                float damping = Mathf.Clamp((wheel.lastDistance - hit.distance) * 10, 0, 10);
                rb.AddForceAtPosition(transform.up * 15 * (wheel.height - hit.distance + damping), wheel.LocationObject.transform.position);
                wheel.lastDistance = hit.distance;
            }
            //wheel.Object.transform.localPosition = hit.point + transform.up * 0.1f;
        }
    }
    void SetInput(Vector3 newInput)
    {
        input = ClampVector(newInput);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
