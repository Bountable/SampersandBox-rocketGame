using System;
using System.Reflection.Metadata;
using Sandbox;


//TODO Make the Lerp more Loose 
public sealed class BotNavigation : Component
{
	
	[Property] private float ThrustForce { get; set; } = 100.0f; // Adjustable thrust force
	[Property] private float GravityForce { get; set; } = 9.8f;   // Gravity simulation
	[Property] private float RotationalSpeed { get; set; } = 100.0f; // Adjustable rotational speed
	[Property] private float MaxSpeed { get; set; } = 50.0f;      // Maximum speed limit

	[Property] private float MinimumDistance { get; set; } = 2;     

	
	[Property] private GameObject Target { get; set;}
	

	
	public float RollRotation;    // Track current yaw rotation
	
	private Vector3 Velocity;


	protected override void OnStart()
	{
		Velocity = Vector3.Zero;
		
	}

	
	protected override void OnUpdate()
	{
		HandleNavigation();
		if (Input.Pressed( "attack2" ))
		{
			PlayerShoot();
		}
		

	}

	private void HandleNavigation()
	{
		var currentRotation = Rotation.FromRoll(RollRotation); // Get current roll rotation
		var thrustDirection = currentRotation.Up; // Calculate the upward direction based on roll
		
		//always apply gravity
		Velocity.z -= GravityForce * Time.Delta;
		
		//add constant thrust since it's a bot.
		Velocity += thrustDirection * ThrustForce * Time.Delta;
		
		//cap max speed
		if (Velocity.Length > MaxSpeed)
		{
			Velocity = Velocity.Normal * MaxSpeed;
		}
		
		//change positions of the bot
		GameObject.WorldPosition += Velocity * Time.Delta;
		
		RotateTowardsPlayer();
	}

	private void RotateTowardsPlayer()
	{
		Vector3 directionToPlayer = (Target.WorldPosition - this.WorldPosition).Normal;

		// Check if the enemy is within a minimum distance to stop rotating
		if (Vector3.DistanceBetween(Target.WorldPosition, WorldPosition) < MinimumDistance)
		{
			return; // Stop rotating if already close to the player
		}

		// Calculate the target roll angle based on the direction to the player
		float targetRoll = MathF.Atan2(directionToPlayer.y, directionToPlayer.z) * (180.0f / MathF.PI);

		// Smoothly rotate the roll towards the target angle
		RollRotation = LerpAngle(RollRotation, -targetRoll, RotationalSpeed * Time.Delta);

		// Apply only the roll rotation to the enemy
		this.WorldRotation = Rotation.FromRoll(RollRotation);

		Log.Info("ROLLING TO POINT TOWARDS PLAYER");
	}

	// Custom LerpAngle function to handle wrapping correctly
	private float LerpAngle(float current, float target, float t)
	{
		float delta = target - current;

		// Wrap delta to be in the range [-180, 180] for shortest rotation path
		if (delta > 180) delta -= 360;
		if (delta < -180) delta += 360;

		return current + delta * t;
	}

	
	
	//TESTING
	private void PlayerShoot()
	{
		// Define the bullet's speed
		float bulletSpeed = 2000f; // Adjust the speed as needed
	    


	    
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
	
	
	
}
