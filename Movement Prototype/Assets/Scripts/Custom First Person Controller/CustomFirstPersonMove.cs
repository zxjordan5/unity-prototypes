using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CustomFirstPersonMove : MonoBehaviour
{
    [SerializeField] Rigidbody rBody;
    // Velocity & acceleration field
    [SerializeField] float maxSpeed;
    [SerializeField, Tooltip("Speed below which the player will come to a full stop.")] float minSpeed;
    [SerializeField] Vector3 velocity;
    [SerializeField] Vector3 acceleration;
    [SerializeField] Vector3 movementDirection;

    // Fields for Acceleration/Deceleration
    [SerializeField, Range(0, 1), Tooltip("The % of the max speed the player can achieve in 1 second.")] float accelPercent;
    [SerializeField, Range(0, 1), Tooltip("The % by which the player is reduced during 1 second of 0 acceleration.")] float decelPercent;

    // Sprint fields
    [Header("Sprinting")]
    [SerializeField] bool canSprint;

    /// <summary>
    /// Gets and sets whether or not the player can sprint
    /// </summary>
    public bool CanSprint { get => canSprint; set => canSprint = value; }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Setup
        Vector3 nextPosition = rBody.transform.position;
    }
}
