using Sandbox;

public sealed class FloorTrigger : Component, Component.ITriggerListener
{

	public void OnTriggerEnter( Collider other )
	{
		Log.Info( other );
		var player = other.Components.Get<CubeController>();

		if ( player != null )
		{
			player.GravityForce = 0;
			player.velocity = 0;
		}
	}
	
	public void OnTriggerExit( Collider other )
	{
		Log.Info( other );
		var player = other.Components.Get<CubeController>();

		if ( player != null )
		{
			player.GravityForce =  500f;
			player. velocity = 1;
		}
	}
}
