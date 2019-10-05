using UnityEngine;

public class Player : MonoBehaviour
{
	private PlayerRenderer renderer;
	private float moveSpeed = 500f;
	private float maxMovespeed = 5f;
	private Rigidbody2D body;

	void Start()
    {
		renderer = GetComponentInChildren<PlayerRenderer>();
		body = GetComponent<Rigidbody2D>();

	}

    void Update()
    {
		Turn(Input.GetAxis("Horizontal"));
		Accelerate(Input.GetAxis("Vertical"));
    }

	private void Turn(float turn) {
		var velocity = body.velocity;
		var turnForce = (moveSpeed * turn);

		body.AddForce(new Vector2(turnForce, velocity.y));

		body.velocity = new Vector2(normalize(velocity.x, maxMovespeed), velocity.y);
	}

	private void Accelerate(float acceleration) {
		var velocity = body.velocity;
		var accelerationForce = (moveSpeed * acceleration);

		body.AddForce(new Vector2(velocity.x, accelerationForce * 0.7f));

		body.velocity = new Vector2(velocity.x, normalize(velocity.y, maxMovespeed));
	}

	private float normalize(float value, float maxValue) {
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
