using System;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using Sandbox;

public sealed class CubeController : Component
{
    [Property] public float ThrustForce { get; set; } = 100.0f; // Adjustable thrust force
    [Property] public float GravityForce { get; set; } = 9.8f;   // Gravity simulation
    [Property] public float RotationalSpeed { get; set; } = 100.0f; // Adjustable rotational speed
    
    [Property] public float MaxSpeed { get; set; } = 50.0f;      // Maximum speed limit


    public Vector3 Velocity; // Custom velocity vector

    public float RollRotation;    // Track current yaw rotation



    protected override void OnStart()
    {
	    Velocity = Vector3.Zero;
	    var initialRotation = GameObject.WorldRotation.Angles();
	    RollRotation = initialRotation.roll;
	    
	    var xRotation = Rotation.FromAxis(Vector3.Up, -90);
	    GameObject.WorldRotation = xRotation * GameObject.WorldRotation;
    }

    protected override void OnUpdate()
    {
	    HandleMovement();
	    
	    if (Input.Pressed( "attack1" ))
	    {
			PlayerShoot();
	    }



    }

    private void PlayerShoot()
    {
	    // Define the bullet's speed
	    float bulletSpeed = 1000f; // Adjust the speed as needed
	    


	    
	    Vector3 spawnPosition = GameObject.WorldPosition + GameObject.WorldRotation.Forward * 100;


	    var bulletPrefab = new GameObject();
	    bulletPrefab.SetPrefabSource("prefabs/bullet.prefab");
	    bulletPrefab.Destroy();
	    
	   
	    
	    // Clone the bullet slightly ahead of the player to avoid overlap
	    GameObject bullet = bulletPrefab.Clone(GameObject.WorldPosition + GameObject.WorldRotation.Up * 150, GameObject.WorldRotation);

	    // Add a Rigidbody component to push it forwards
	    var bulletRigidbody = bullet.GetComponent<Rigidbody>();
	    if (bulletRigidbody == null)
	    {
		    bulletRigidbody = bullet.AddComponent<Rigidbody>();
		    bulletRigidbody.Gravity = false;
	    }
	    

	    // Apply forward velocity to the bullet
	    bulletRigidbody.Velocity = GameObject.WorldRotation.Up * bulletSpeed;

	    var bulletController = bullet.AddComponent<BulletController>();
	    bulletController.Initialize(spawnPosition, GameObject.WorldRotation.Forward, 500, 1000);
	    


    }

    //This Function Will handle the thrust of the player, and rotation of the player
    // Thrust will be directed according to the current local angle of the player 
    private void HandleMovement()
    {
	    //====MOVEMENT AND ROTATION======
	    var currentRotation =  Rotation.FromRoll(RollRotation); //Get current rotation
	    var thrustDirection = currentRotation.Up;  // Calculate direction based on full rotation
	    
	    Velocity.z -= GravityForce * Time.Delta; //always applying gravity	    
	    if (Input.Down("Jump"))
	    {
		    Velocity += thrustDirection * ThrustForce * Time.Delta;
	    }

	    if (Input.Down("Right"))
	    {
		    RollRotation += RotationalSpeed * Time.Delta;
	    }
	    else if (Input.Down("Left"))
	    {
		    RollRotation -= RotationalSpeed * Time.Delta;
	    }

	    if (Velocity.Length > MaxSpeed)
	    {
		    Velocity = Velocity.Normal * MaxSpeed;
	    }
	    
	    GameObject.WorldPosition += Velocity * Time.Delta;
	    GameObject.WorldRotation = Rotation.FromRoll(RollRotation);
	    //====MOVEMENT AND ROTATION======
	    
    }




    
}
