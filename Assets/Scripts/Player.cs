using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public GameObject hardPoint;
	public GameObject bullet;
	public GameObject laserBullet;
	public GameObject laserScytheSmall;
	public GameObject laserScytheLarge;

	public GameObject explosion;
	public AudioClip fireSound;
	public AudioClip powerUpSound;
	public AudioClip impact;
	public AudioClip playerBreak;

	private PlayerRenderer playerRenderer;
	private float moveSpeed = 600f;
	private float maxMovespeed = 4f;
	private Rigidbody2D body;
	private List<HardPoint> hardPoints = new List<HardPoint>();
	private int powerLevel = 0;
	private AudioController audioController;
	private GameController gameController;
	private int lifes = 3;
	private bool immortal = false;
	private WeaponType currentWeaponType;

	private void Start()
	{
		playerRenderer = GetComponentInChildren<PlayerRenderer>();
		body = GetComponent<Rigidbody2D>();
		audioController = GameObject.Find("AudioController").GetComponent<AudioController>();
		gameController = GameObject.Find("GameController").GetComponent<GameController>();

		PowerLevel(powerLevel, WeaponType.Plasma);
	}

	private void FixedUpdate()
	{
		if (!gameController.IsIntro())
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
		else
		{
			body.velocity = new Vector3(0, 1f, 0);
		}
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("PowerUp"))
		{
			var pu = collision.gameObject.GetComponent<Powerup>();

			if (pu.weaponType.Equals(currentWeaponType))
			{
				powerLevel++;
			}

			PowerLevel(powerLevel, pu.weaponType);

			PowerLevel(4, WeaponType.Beam);

			audioController.PlaySingle(powerUpSound, 0.7f);
		}
		else
		{
			if (!immortal)
			{
				if (lifes <= 1)
				{
					Destroy(gameObject);
					Instantiate(explosion, transform.position + new Vector3(-1, -1, 0), transform.rotation);
					audioController.PlaySingle(playerBreak, 1f);
					lifes--;
					gameController.RenderLife(lifes);
					gameController.Dead();
				}
				else
				{
					StartCoroutine(Flash());
					lifes--;
					gameController.RenderLife(lifes);
					audioController.PlaySingleHigh(playerBreak, 0.3f);
					Instantiate(explosion, transform.position + new Vector3(-1, -1, 0), transform.rotation);
				}
			}
		}
	}

	private void PowerLevel(int level, WeaponType type)
	{
		currentWeaponType = type;
		CleanHardpoints();
		playerRenderer.PowerLevel(level, type);

		hardPoints = new List<HardPoint>();

		switch (level)
		{
			case 0:
				break;

			case 1:
				if (type.Equals(WeaponType.Plasma))
				{
					CreateHardPoint(transform.position + new Vector3(0, 0.15f, 0), transform.rotation, bullet);
				}
				else
				{
					CreateHardPoint(transform.position + new Vector3(0, 0.15f, 0), transform.rotation, laserBullet);
				}
				break;

			case 2:
				if (type.Equals(WeaponType.Plasma))
				{
					CreateHardPoint(transform.position + new Vector3(0.09f, 0.15f, 0), transform.rotation, bullet);
					CreateHardPoint(transform.position + new Vector3(-0.09f, 0.15f, 0), transform.rotation, bullet);
				}
				else
				{
					CreateHardPoint(transform.position + new Vector3(0.09f, 0.15f, 0), transform.rotation, laserBullet);
					CreateHardPoint(transform.position + new Vector3(-0.09f, 0.15f, 0), transform.rotation, laserBullet);
				}
				break;

			case 3:
				if (type.Equals(WeaponType.Plasma))
				{
					CreateHardPoint(transform.position + new Vector3(0.3f, -0.15f, 0), transform.rotation, bullet);
					CreateHardPoint(transform.position + new Vector3(-0.3f, -0.15f, 0), transform.rotation, bullet);
					CreateHardPoint(transform.position + new Vector3(0, 0.15f, 0), transform.rotation, bullet);
				}
				else
				{
					CreateHardPoint(transform.position + new Vector3(0, 0.7f, 0), transform.rotation, laserScytheSmall);
				}
				break;

			case 4:
				if (type.Equals(WeaponType.Plasma))
				{
					var rightRotation = new Quaternion();
					var leftRotation = new Quaternion();

					rightRotation.eulerAngles = new Vector3(0, 0, 25);
					leftRotation.eulerAngles = new Vector3(0, 0, -25);

					CreateHardPoint(transform.position + new Vector3(0.3f, -0.15f, 0), leftRotation, bullet);
					CreateHardPoint(transform.position + new Vector3(-0.3f, -0.15f, 0), rightRotation, bullet);
					CreateHardPoint(transform.position + new Vector3(0.09f, 0.15f, 0), transform.rotation, bullet);
					CreateHardPoint(transform.position + new Vector3(-0.09f, 0.15f, 0), transform.rotation, bullet);
				}
				else
				{
					CreateHardPoint(transform.position + new Vector3(0, 0.7f, 0), transform.rotation, laserScytheLarge);
				}

				break;

			default:
				var rightRotation2 = new Quaternion();
				var leftRotation2 = new Quaternion();

				rightRotation2.eulerAngles = new Vector3(0, 0, 25);
				leftRotation2.eulerAngles = new Vector3(0, 0, -25);

				CreateHardPoint(transform.position + new Vector3(0.3f, -0.15f, 0), leftRotation2, bullet);
				CreateHardPoint(transform.position + new Vector3(-0.3f, -0.15f, 0), rightRotation2, bullet);
				CreateHardPoint(transform.position + new Vector3(0.3f, -0.15f, 0), transform.rotation, bullet);
				CreateHardPoint(transform.position + new Vector3(-0.3f, -0.15f, 0), transform.rotation, bullet);
				CreateHardPoint(transform.position + new Vector3(0, 0.15f, 0), transform.rotation, bullet);
				break;
		}
	}

	private void CreateHardPoint(Vector3 position, Quaternion rotation, GameObject bullet)
	{
		var hp = Instantiate(hardPoint, position, rotation, transform);
		var hpScript = hp.GetComponent<HardPoint>();

		hpScript.bullet = bullet;

		hpScript.rateOfFire = 0.3f;
		hpScript.audioClip = fireSound;
		hpScript.volume = 0.7f;

		hardPoints.Add(hpScript);
	}

	private void CleanHardpoints()
	{
		foreach (var hardPoint in hardPoints)
		{
			Destroy(hardPoint.gameObject);
		}
	}

	private IEnumerator Flash()
	{
		immortal = true;
		var flashcolor = new Color32(200, 200, 200, 130);
		for (var n = 0; n < 6; n++)
		{
			playerRenderer.shipRenderer.material.color = Color.white;
			yield return new WaitForSeconds(.2f);
			playerRenderer.shipRenderer.material.color = flashcolor;
			yield return new WaitForSeconds(.2f);
		}
		playerRenderer.shipRenderer.material.color = Color.white;
		immortal = false;
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
