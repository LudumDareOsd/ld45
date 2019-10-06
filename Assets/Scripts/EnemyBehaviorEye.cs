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
	private float lifetime_max = 9.0f;
	//private GameController gameController;
	private AudioController audioController;

	void Start()
	{
		audioController = GameObject.Find("AudioController").GetComponent<AudioController>();
		//gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		rb = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		lifetime += Time.deltaTime;
		rb.velocity = new Vector2(0, -2.3f);
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
			audioController.PlaySingle(deathSound, 0.3f);
			Destroy(gameObject);
		}
	}

}
