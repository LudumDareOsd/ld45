using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorEye : MonoBehaviour
{
	public GameObject bloodsplosion;
	public AudioClip deathSound;

	private LTBezierPath[] enemyPaths;
	private Rigidbody2D rb;
	private int life = 3;
	private float lifetime = 0.0f;
	private float lifetime_max = 7.0f;
	private int wave;
	private GameController gameController;
	private AudioController audioController;

	void Start()
	{
		audioController = GameObject.Find("AudioController").GetComponent<AudioController>();
		gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		wave = gameController.GetWave();
		rb = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		rb.velocity = new Vector2(0, -2.3f);
		lifetime += Time.deltaTime;
		if (lifetime > lifetime_max)
		{
			Destroy(gameObject);
		}
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		life--;
		if (life < 1)
		{
			Instantiate(bloodsplosion, transform.position, transform.rotation);
			audioController.PlaySingle(deathSound, 0.5f);
			gameController.SpawnPowerup(gameObject);
			Destroy(gameObject);
		}
	}

}
