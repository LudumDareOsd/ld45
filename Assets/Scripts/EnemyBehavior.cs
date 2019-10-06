using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
	public GameObject bloodsplosion;
	public AudioClip deathSound;

	private LTBezierPath[] enemyPaths;
	private Rigidbody2D rb;
	private int life = 3;
	private float lifetime = 0.0f;
	private float lifetime_max = 7.0f;
	private float shootDelay = 1.0f;
	private int wave;
	private GameController gameController;
	private AudioController audioController;
	private HardPoint hardpoint;

	void Start()
	{
		audioController = GameObject.Find("AudioController").GetComponent<AudioController>();
		gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		hardpoint = GetComponentInChildren<HardPoint>();
		wave = gameController.GetWave(true);
		rb = GetComponent<Rigidbody2D>();
		shootDelay = Random.Range(0.0f, 1.0f);

		//It goes in the order: startPoint, endControl, startControl, endPoint -
		//Note: the control for the end and start are reversed! This is just a quirk of the API.
		enemyPaths = new LTBezierPath[] {
			new LTBezierPath(new Vector3[] {
				Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 1.1f, 0.0f)), Camera.main.ViewportToWorldPoint(new Vector3(0.7f, 0.9f, 0.0f)),
				Camera.main.ViewportToWorldPoint(new Vector3(0.3f, 0.3f, 0.0f)), Camera.main.ViewportToWorldPoint(new Vector3(1.1f, -0.1f, 0.0f))
			}),
			new LTBezierPath(new Vector3[] {
				Camera.main.ViewportToWorldPoint(new Vector3(1.1f, 1.1f, 0.0f)), Camera.main.ViewportToWorldPoint(new Vector3(0.1f, 0.7f, 0.0f)),
				Camera.main.ViewportToWorldPoint(new Vector3(0.7f, 0.3f, 0.0f)), Camera.main.ViewportToWorldPoint(new Vector3(-0.1f, -0.1f, 0.0f))
			}),
			new LTBezierPath(new Vector3[] {
				Camera.main.ViewportToWorldPoint(new Vector3(0.6f, 1.1f, 0.0f)), Camera.main.ViewportToWorldPoint(new Vector3(0.2f, 0.5f, 0.0f)),
				Camera.main.ViewportToWorldPoint(new Vector3(0.7f, 0.5f, 0.0f)), Camera.main.ViewportToWorldPoint(new Vector3(0.1f, 1.1f, 0.0f))
			}),
			new LTBezierPath(new Vector3[] {
				Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 1.1f, 0.0f)), Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.7f, 0.0f)),
				Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f)), Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 1.1f, 0.0f))
			})
		};

		//StartCoroutine(Evade());
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("PlayerBullet"))
		{
			var bullet = collision.gameObject.GetComponent<Bullet>();
			if (bullet)
			{
				life -= bullet.damage;
			}
			else
			{
				life--;
			}
			if (life < 1)
			{
				gameController.SpawnPowerup(gameObject, wave);
				Instantiate(bloodsplosion, transform.position, transform.rotation);
				audioController.PlaySingle(deathSound, 0.3f);
				Destroy(gameObject);
			}
		}
	}

	void FixedUpdate()
	{
		lifetime += Time.deltaTime;
		if (lifetime > shootDelay)
		{
			hardpoint.Fire();
		}

		var p = (lifetime / lifetime_max);
		Vector3 pt = enemyPaths[wave % enemyPaths.Length].point(p);
		rb.position = pt;

		if (lifetime > lifetime_max)
		{
			Destroy(gameObject);
		}
	}
}
