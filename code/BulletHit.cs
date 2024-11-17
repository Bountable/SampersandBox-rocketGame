using Sandbox;

public sealed class BulletHit : Component, Component.ITriggerListener
{


	public void OnTriggerEnter(Collider other)
	{
		
		Log.Info( other);
		other.GetComponent<ModelRenderer>().Destroy();
		
	}
}
