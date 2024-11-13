using System;
using Sandbox;

public sealed class CubeController : Component
{
    [Property] public float ThrustForce { get; set; } = 100.0f; // Adjustable thrust force
    [Property] public float GravityForce { get; set; } = 9.8f;   // Gravity simulation
    [Property] public float RotationalSpeed { get; set; } = 100.0f; // Adjustable rotational speed
    
    [Property] public float MaxSpeed { get; set; } = 50.0f;      // Maximum speed limit

    public Vector3 velocity; // Custom velocity vector
    public float rollRotation;    // Track current yaw rotation
    
    
  
    protected override void OnStart()
    {
	    velocity = Vector3.Zero;
	    var initialRotation = GameObject.WorldRotation.Angles();
	    rollRotation = initialRotation.roll;
    }

    protected override void OnUpdate()
    {
	    
	    var currentRotation =  Rotation.FromRoll(rollRotation); //Get current rotation
	    var thrustDirection = currentRotation.Up;  // Calculate direction based on full rotation
	    
	    velocity.z -= GravityForce * Time.Delta; //always applying gravity	    
	    if (Input.Down("Jump"))
	    {
		    velocity += thrustDirection * ThrustForce * Time.Delta;
	    }

	    if (Input.Down("Right"))
	    {
		    rollRotation += RotationalSpeed * Time.Delta;
	    }
	    else if (Input.Down("Left"))
	    {
		    rollRotation -= RotationalSpeed * Time.Delta;
	    }

	    if (velocity.Length > MaxSpeed)
	    {
		    velocity = velocity.Normal * MaxSpeed;
	    }
	    
	    GameObject.WorldPosition += velocity * Time.Delta;
	    GameObject.WorldRotation = Rotation.FromRoll(rollRotation);
    
    }

    


    
}
