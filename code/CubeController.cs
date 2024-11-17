using System;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using Sandbox;

public sealed class CubeController : Component
{
    [Property] public float ThrustForce { get; set; } = 100.0f; // Adjustable thrust force
    [Property] public float GravityForce { get; set; } = 500f;   // Gravity simulation
    
    [Property] public float RotationalSpeed { get; set; } = 100.0f; // Adjustable rotational speed
    
    [Property] public float MaxSpeed { get; set; } = 50.0f;      // Maximum speed limit


    private Vector3 Velocity; // Custom velocity vector

    private float RollRotation;    // Track current yaw rotation



    protected override void OnStart()
    {
	    Velocity = Vector3.Zero;
	    var initialRotation = GameObject.WorldRotation.Angles();
	    RollRotation = initialRotation.roll;
	    
		// var xRotation = Rotation.FromAxis(Vector3.Up, -90);
		//  GameObject.WorldRotation = xRotation * GameObject.WorldRotation;
	    
	    
	    
    }

    protected override void OnUpdate()
    {
	    Velocity.z -= GravityForce * Time.Delta; //always applying gravity	  
	    HandleMovement();
	    
	    
	    if (Input.Pressed( "attack1" ))
	    {
			PlayerShoot();
	    }
	  


    }

    private void PlayerShoot()
    {
	    // Define the bullet's base speed
	    float bulletBaseSpeed = 6000;

	    // Get the player's current speed (magnitude of velocity)
	    float currentVelocity = GetVelocity().Length;

	    // Calculate the bullet's total speed
	    float bulletTotalSpeed = bulletBaseSpeed + currentVelocity;

	    // Log the calculated speed for debugging
	    // Log.Info("CURRENT SPEED: " + currentVelocity);
	    // Log.Info("BULLET TOTAL SPEED: " + bulletTotalSpeed);

	    // Determine the spawn position of the bullet
	    Vector3 spawnPosition = GameObject.WorldPosition + GameObject.WorldRotation.Up * 100;
	    // Create the bullet prefab and clone the rigid body IDK
	    var bulletPrefab = new GameObject();
	    bulletPrefab.SetPrefabSource("prefabs/bullet.prefab");
	    bulletPrefab.WorldScale = new Vector3(1, 1, 5); //scale of the prefab to be longer/ more bullet like 
	    bulletPrefab.Destroy();

	    // Clone the bullet slightly ahead of the player
	    GameObject bullet = bulletPrefab.Clone(spawnPosition, GameObject.WorldRotation);
	    // Add a Rigidbody component if not already present
	    var bulletRigidbody = bullet.GetComponent<Rigidbody>();
	    if (bulletRigidbody == null)
	    {
		    bulletRigidbody = bullet.AddComponent<Rigidbody>();
		    bulletRigidbody.AddComponent<BoxCollider>();

		    bulletRigidbody.Gravity = false;
	    }

	    // Set the bullet's velocity
	    bulletRigidbody.Velocity = GameObject.LocalRotation.Up * bulletTotalSpeed;
	    bulletRigidbody.AddComponent<BulletHit>();
	    
	    // Attach a bullet controller and initialize it
	    var bulletController = bullet.AddComponent<BulletController>();
	    bulletController.Initialize(spawnPosition, GameObject.LocalRotation.Up, bulletTotalSpeed, 10000, currentVelocity);
	    


    }

    //This Function Will handle the thrust of the player, and rotation of the player
    // Thrust will be directed according to the current local angle of the player 
    private void HandleMovement()
    {
	    //====MOVEMENT AND ROTATION======
	    var currentRotation =  Rotation.FromRoll(RollRotation); //Get current rotation
	    
	    var thrustDirection = currentRotation.Up;  // Calculate direction based on full rotation
	    
	    
	    
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
    
    public Vector3 GetVelocity()
    {
	    return Velocity;
    }

    public void SetVelocity( Vector3 setVelocity )
    {
	    this.Velocity = setVelocity;
    }
    





}
