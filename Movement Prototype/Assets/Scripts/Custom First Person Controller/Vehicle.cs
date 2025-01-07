using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Ex7 - https://docs.google.com/document/d/1Igukg_XBW6FtwrM2sL0n3BkryQwu6pYfwAX64tpI40M/edit?tab=t.0#heading=h.w14grdblwqpa
// Ex8 - https://docs.google.com/document/d/1n0c7FayT74mB6qEMTrCZQlP-Z4Nez4H2jtH6zxWCZ8U/edit?tab=t.0#heading=h.w14grdblwqpa
public class Vehicle : MonoBehaviour
{
    // Reference to RigidBody on this GameObject
    [SerializeField] Rigidbody rBody;

    // Fields for Speed
    [SerializeField] float maxSpeed;
    [SerializeField, Tooltip("Speed below which the vehicle will come to a full stop.")] float minSpeed;

    // Fields for Acceleration/Deceleration
    [SerializeField, Range(0, 1), Tooltip("The % of the max speed the vehicle can achieve in 1 second.")] float accelPercent;
    [SerializeField, Range(0, 1), Tooltip("The % by which the velocity is reduced during 1 second of 0 acceleration.")] float decelPercent;

    // Fields for Turning
    [SerializeField, Tooltip("The # of degrees a moving vehicle can turn per second.")] float turnRate;

    // Fields for Input
    Vector3 movementDirection;

    // Fields for Movement Vectors
    Vector3 velocity, acceleration;

    // Fields for Quaternions
    Quaternion rotDelta;

    // Stuff for snapping to terrain
    [SerializeField] bool snapToTerrain = false;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] float maxHeight = 101f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        // Setup
        Vector3 nextPosition = transform.position;
        Quaternion nextRotation = transform.rotation;

        #region Raycasting
        RaycastHit terrainHit;
        Vector3 origin = transform.position;
        origin.y = maxHeight;

        if (snapToTerrain && Physics.Raycast(origin, Vector3.down, out terrainHit, maxHeight+1, groundLayerMask))
        {
            // Raycast to adjust position & rotation to the terrain if needed
            nextPosition = terrainHit.point;
        }
        #endregion

        #region Velocity
        // Always reset acceleration to zero
        acceleration = Vector3.zero;

        // Accelerate if pressing up
        if (movementDirection.z != 0)
        {
            // Calc accel based on movement direction
            acceleration = transform.forward * movementDirection.z * accelPercent * maxSpeed;

            // Calc velocity based on accel scaled by time
            velocity += acceleration * Time.fixedDeltaTime;

            // Clamp velocity to min/max speed
            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        }

        // Otherwise, decelerate
        else if (velocity != Vector3.zero)
        {
            // decelerate velocity based on deceleration rate scaled by time
            velocity *= 1 - (decelPercent * Time.fixedDeltaTime);

            // If it gets really small, just zero it out
            if (velocity.sqrMagnitude < minSpeed * minSpeed)
            {
                velocity = Vector3.zero;
            }
        }
        #endregion

        #region Rotation
        float turnAmount = movementDirection.x * turnRate * Time.fixedDeltaTime;

        // Rotation stuff
        rotDelta = Quaternion.Euler(
            0f,
            turnAmount,
            0f);

        nextRotation *= rotDelta;
        nextRotation.Normalize();

        // ALSO need to rotate the velocity vector. This won't impact the magnitude
        velocity = rotDelta * velocity; // quarterion HAS to be on the left
        #endregion

        //  Use velocity to calc next position
        nextPosition += (velocity * Time.fixedDeltaTime);

        //  Move and Rotate the Vehicle
        rBody.Move(nextPosition, nextRotation);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputDir = context.ReadValue<Vector2>();
        movementDirection.z = inputDir.y;
        movementDirection.x = inputDir.x;
    }
}
