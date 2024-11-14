using Sandbox;

public class BulletController : Component
{
	private Vector3 originPosition;
	private Vector3 direction;
	private float speed;
	private float MaxDistance;
	private float traveledDistance;

	public void Initialize(Vector3 origin, Vector3 direction, float speed, float maxDistance)
	{
		this.originPosition = origin;
		this.direction = direction.z;
		this.speed = speed;
		this.MaxDistance = maxDistance;
		this.traveledDistance = 0.0f;
	}

	protected override void OnUpdate()
	{
		// Calculate the distance to move this frame
		float distanceToMove = speed * Time.Delta;

		// Update the bullet's position
		GameObject.WorldPosition += direction * distanceToMove *2;

		// Update the total traveled distance
		traveledDistance += distanceToMove;

		// Destroy the bullet if it has traveled beyond the maximum distance
		if (traveledDistance >= MaxDistance)
		{
			GameObject.Destroy();
		}
	}

}
