using Sandbox;

public class BulletController : Component
{
	private Vector3 originPosition;
	private Vector3 direction;
	private float speed;
	private float MaxDistance;
	private float traveledDistance;

	[Property] float targetObjectVelocity;

	public void Initialize(Vector3 origin, Vector3 direction, float speed, float maxDistance, float velocity)
	{
		this.originPosition = origin;
		this.direction = direction;
		this.speed = speed;
		this.MaxDistance = maxDistance;
		this.traveledDistance = 0.0f;
		this.targetObjectVelocity = velocity;
	}



	protected override void OnUpdate()
	{
		// Calculate the distance to move this frame
		float distanceToMove = speed * Time.Delta;

		// Move the bullet in its forward direction
		GameObject.WorldPosition += direction * distanceToMove;

		// Update the total traveled distance
		traveledDistance += distanceToMove;

		// Destroy the bullet if it has traveled beyond the maximum allowed distance
		if (traveledDistance >= MaxDistance)
		{
			GameObject.Destroy();
		}

		
	}
}
