using Sandbox;

public sealed class CameraController : Component
{

	[Property] private GameObject Target { get; set;}
	[Property] public Vector3 Offset = new Vector3(2000, 2000, 2000); // Offset from the target

	private CameraComponent camera = null;

	protected override void OnStart()
	{
		camera = GetComponent<CameraComponent>();
		camera.WorldPosition = Offset;
	}

	protected override void OnUpdate()
	{
		
		camera.WorldPosition = Target.WorldPosition + Offset;

	}
}
