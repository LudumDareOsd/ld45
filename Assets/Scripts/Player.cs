using UnityEngine;

public class Player : MonoBehaviour
{
	private PlayerRenderer playerRenderer;
	private float moveSpeed = 400f;
	private float maxMovespeed = 5f;
	private Rigidbody2D body;
	private HardPoint[] hardPoints;
	private int powerLevel = 1;

	private void Start()
	{
		playerRenderer = GetComponentInChildren<PlayerRenderer>();
		body = GetComponent<Rigidbody2D>();
		hardPoints = GetComponentsInChildren<HardPoint>();
		PowerLevel(powerLevel);
	}

	private void Update()
	{
		Turn(Input.GetAxis("Horizontal"));
		Accelerate(Input.GetAxis("Vertical"));

		if (Input.GetButton("Fire1"))
		{
			foreach (var hardPoint in hardPoints)
			{
				hardPoint.Fire();
			}
		}
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("powerup"))
		{
			powerLevel++;
			PowerLevel(powerLevel);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void PowerLevel(int level) {
		playerRenderer.PowerLevel(powerLevel);

		switch (level) {
			case 1:

				break;

			case 2:
				break;

			case 3:
				break;

			case 4:
				break;

			case 5:
				break;
			
		}

		hardPoints = GetComponentsInChildren<HardPoint>();
	}

	private void CleanHardpoints() {
		foreach (var hardPoint in hardPoints) {
			Destroy(hardPoint.gameObject);
		}
	}

	private void Turn(float turn)
	{
		var velocity = body.velocity;
		var turnForce = (moveSpeed * turn);

		body.AddForce(new Vector2(turnForce, velocity.y));

		body.velocity = new Vector2(normalize(velocity.x, maxMovespeed), velocity.y);

		if (turn > 0)
		{
			playerRenderer.TurnRight();
		}
		else if (turn < 0)
		{
			playerRenderer.TurnLeft();
		}
		else
		{
			playerRenderer.Straight();
		}
	}

	private void Accelerate(float acceleration)
	{
		var velocity = body.velocity;
		var accelerationForce = (moveSpeed * acceleration);

		body.AddForce(new Vector2(velocity.x, accelerationForce * 0.5f));

		body.velocity = new Vector2(velocity.x, normalize(velocity.y, maxMovespeed));
	}

	private float normalize(float value, float maxValue)
	{
		var result = value;

		if (value > maxValue)
		{
			result = maxValue;
		}
		else if (value < -maxValue)
		{
			result = -maxValue;
		}

		return result;
	}
}
