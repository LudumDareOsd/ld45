using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public GameObject hardPoint;
	public GameObject bullet;
	public AudioClip fireSound;

	private PlayerRenderer playerRenderer;
	private float moveSpeed = 600f;
	private float maxMovespeed = 4f;
	private Rigidbody2D body;
	private List<HardPoint> hardPoints = new List<HardPoint>();
	private int powerLevel = 1;

	private void Start()
	{
		playerRenderer = GetComponentInChildren<PlayerRenderer>();
		body = GetComponent<Rigidbody2D>();
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
		if (collision.CompareTag("PowerUp"))
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
		CleanHardpoints();
		playerRenderer.PowerLevel(powerLevel);

		hardPoints = new List<HardPoint>();

		switch (level) {
			case 1:
				CreateHardPoint(transform.position + new Vector3(0, 0.15f, 0), transform.rotation);
				break;

			case 2:
				CreateHardPoint(transform.position + new Vector3(0.09f, 0.15f, 0), transform.rotation);
				CreateHardPoint(transform.position + new Vector3(-0.09f, 0.15f, 0), transform.rotation);
				break;

			case 3:
				CreateHardPoint(transform.position + new Vector3(0.3f, -0.15f, 0), transform.rotation);
				CreateHardPoint(transform.position + new Vector3(-0.3f, -0.15f, 0), transform.rotation);
				CreateHardPoint(transform.position + new Vector3(0, 0.15f, 0), transform.rotation);
				break;

			case 4:
				var rightRotation = new Quaternion();
				var leftRotation = new Quaternion();

				rightRotation.eulerAngles = new Vector3(0, 0, 25);
				leftRotation.eulerAngles = new Vector3(0, 0, -25);

				CreateHardPoint(transform.position + new Vector3(0.3f, -0.15f, 0), leftRotation);
				CreateHardPoint(transform.position + new Vector3(-0.3f, -0.15f, 0), rightRotation);
				CreateHardPoint(transform.position + new Vector3(0.09f,  0.15f, 0), transform.rotation);
				CreateHardPoint(transform.position + new Vector3(-0.09f,  0.15f, 0), transform.rotation);
				break;

			default:
				var rightRotation2 = new Quaternion();
				var leftRotation2 = new Quaternion();

				rightRotation2.eulerAngles = new Vector3(0, 0, 25);
				leftRotation2.eulerAngles = new Vector3(0, 0, -25);

				CreateHardPoint(transform.position + new Vector3(0.3f, -0.15f, 0), leftRotation2);
				CreateHardPoint(transform.position + new Vector3(-0.3f, -0.15f, 0), rightRotation2);
				CreateHardPoint(transform.position + new Vector3(0.3f, -0.15f, 0), transform.rotation);
				CreateHardPoint(transform.position + new Vector3(-0.3f, -0.15f, 0), transform.rotation);
				CreateHardPoint(transform.position + new Vector3(0, 0.15f, 0), transform.rotation);
				break;
		}
	}

	private void CreateHardPoint(Vector3 position, Quaternion rotation) {
		var hp = Instantiate(hardPoint, position, rotation, transform);
		var hpScript = hp.GetComponent<HardPoint>();
		hpScript.bullet = bullet;
		hpScript.rateOfFire = 0.3f;
		hpScript.audioClip = fireSound;

		hardPoints.Add(hpScript);
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
