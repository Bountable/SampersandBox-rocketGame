using Sandbox;
using Sandbox.Physics;

public sealed class FloorTrigger : Component, Component.ITriggerListener
{

	private float storedGravity;
	
    private CubeController trackedPlayer; // Keep track of the player inside the trigger

    public void OnTriggerEnter(Collider other)
    {
        Log.Info(other);

        var player = other.Components.Get<CubeController>();
        if (player != null)
        {
            trackedPlayer = player; // Start tracking the player

            // Disable gravity
            storedGravity = player.GravityForce;
            player.GravityForce = 0;

            // Reset velocity to prevent unintended motion
            var velocity = player.GetVelocity();
            velocity.x = 0;
            velocity.y = 0;
            velocity.z = 0;
            player.SetVelocity(velocity);

            Log.Info("Player entered trigger. Gravity disabled.");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        Log.Info(other);

        var player = other.Components.Get<CubeController>();
        if (player != null && trackedPlayer == player)
        {
            // Restore gravity for the player
            player.GravityForce = storedGravity;
            trackedPlayer = null; // Stop tracking the player

            Log.Info("Player exited trigger. Gravity restored.");
        }
    }

    protected override void OnUpdate()
    {
        // Continuously check the player's position while inside the trigger
        if (trackedPlayer != null)
        {
            var position = trackedPlayer.GameObject.WorldPosition +50;

            // Push the player up if they fall below z = 0
            if (position.z < 0)
            {
                var velocity = trackedPlayer.GetVelocity();
                velocity.z = 1000; // Push upward with a velocity
                trackedPlayer.SetVelocity(velocity);

                // Optionally log or adjust the position directly
                position.z = 0; // Clamp position to z = 0
                trackedPlayer.GameObject.WorldPosition = position;
                Log.Info("Player is below z=0. Adjusting position and velocity.");
            }
        }
    }
}

//TODO FIX THIS BROKEN SHIT
