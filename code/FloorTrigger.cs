using Sandbox;
using Sandbox.Physics;

public sealed class FloorTrigger : Component, Component.ITriggerListener
{
	private float currentGravity = 0;

	public void OnTriggerEnter( Collider other )
	{
		Log.Info( other );
		var player = other.Components.Get<CubeController>();
		currentGravity = player.GravityForce;
		
		//this will make player interact with the ground and 'Crash'
		if ( player != null )
		{
			player.GravityForce = 0;
			player.Velocity = 0;
		}
	}
	
	public void OnTriggerExit( Collider other )
	{
		Log.Info( other );
		var player = other.Components.Get<CubeController>();

		if ( player != null )
		{
			player.GravityForce = currentGravity;
			player. Velocity = 1; //allow velocity changing
		}
	}
}
