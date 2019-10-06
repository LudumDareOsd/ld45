using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState
{
	INITIAL, INTRO, PHASE1, PHASE2, REVIVE, PHASE3, DEAD
}

public class Boss : MonoBehaviour
{
	public float dodge = 2.0f;
	public Vector2 maneuverTime = new Vector2(2, 5);
	public AudioClip appearSound, phase3Sound;
	public Sprite angryEyes;

	private Eye leftEye, rightEye, bigEye;
	private bool flashing = false;
	private float lifetime = 0.0f, moveSmoothing = 0.9f;
	private float eyeToggle = 0.0f;
	private float bigEyeToggle = 2.5f;
	private float targetXManeuver = 0.0f;
	private float targetYManeuver = 0.0f;
	private BossState state = BossState.INITIAL;
	private GameController gameController;
	private AudioController audioController;
	private Rigidbody2D rb;
	private SpriteRenderer sprite, eyeSprite;

	//private GameObject spreadHP;

	void Start()
    {
		leftEye = transform.Find("LeftEye").GetComponent<Eye>();
		rightEye = transform.Find("RightEye").GetComponent<Eye>();
		bigEye = transform.Find("BigEye").GetComponent<Eye>();
		audioController = GameObject.Find("AudioController").GetComponent<AudioController>();
		gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		rb = GetComponent<Rigidbody2D>();
		sprite = GetComponent<SpriteRenderer>();
		eyeSprite = transform.Find("Eyes").GetComponent<SpriteRenderer>();
		NextPhase();
	}

	// Update is called once per frame
	void Update()
    {
		lifetime += Time.deltaTime;
		eyeToggle += Time.deltaTime;
		bigEyeToggle += Time.deltaTime;

		switch (state)
		{
			case BossState.INTRO:
				Intro();
				break;
			case BossState.PHASE1:
				Phase1();
				break;
			case BossState.PHASE2:
				Phase2();
				break;
			case BossState.REVIVE:
				Revive();
				break;
			case BossState.PHASE3:
				Phase3();
				break;
			case BossState.DEAD:
				//gameController.Win();
				break;
			default:
				break;
		}


	}

	public void NextPhase()
	{
		state = (BossState)((int)state + 1);
		Debug.Log("BOSS IN PHASE: " + state.ToString());

		if (state == BossState.INTRO)
		{
			StartCoroutine(Appear());
			audioController.PlaySingle(appearSound, 0.7f);
		}

		if (state == BossState.PHASE1)
		{
			StopCoroutine(Appear());
			leftEye.ToggleOpen();
			eyeToggle = 0.0f;
		}

		if (state == BossState.PHASE2)
		{
			StartCoroutine(Evade());
			bigEyeToggle = 2.5f;
		}

		if (state == BossState.REVIVE)
		{
			StopCoroutine(Evade());
			StartCoroutine(Regenerate());
		}

		if (state == BossState.PHASE3)
		{
			StartCoroutine(Evade());
			StopCoroutine(Regenerate());
			leftEye.SwitchState(EyeState.OPEN);
			rightEye.SwitchState(EyeState.CLOSED);
			bigEye.SwitchState(EyeState.CLOSED);
			eyeToggle = 0.0f;
			bigEyeToggle = 2.5f;
			moveSmoothing = 1.7f;
			eyeSprite.sprite = angryEyes;
		}

		else if (state == BossState.DEAD)
		{
			Destroy(gameObject);
		}
	}

	void Intro()
	{

	}

	void Phase1()
	{
		if (eyeToggle > 5.0f)
		{
			leftEye.ToggleOpen();
			rightEye.ToggleOpen();
			eyeToggle = 0.0f;
		}
	}

	void Phase2()
	{
		if (bigEyeToggle > 5.0f)
		{
			bigEye.ToggleOpen();
			bigEyeToggle = 0.0f;
		}
	}

	void Revive()
	{

	}

	void Phase3()
	{
		if (eyeToggle > 5.0f)
		{
			leftEye.ToggleOpen();
			rightEye.ToggleOpen();
			eyeToggle = 0.0f;
		}

		if (bigEyeToggle > 5.0f)
		{
			bigEye.ToggleOpen();
			bigEyeToggle = 0.0f;
		}

		if (leftEye.IsDead() && rightEye.IsDead() && bigEye.IsDead())
			NextPhase();
	}

	void FixedUpdate()
	{
		var camMin = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f));
		var camMax = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, 0.0f));

		var newXManeuver = Mathf.MoveTowards(rb.velocity.x, targetXManeuver, Time.deltaTime * moveSmoothing);
		var newYManeuver = Mathf.MoveTowards(rb.velocity.y, targetYManeuver, Time.deltaTime * moveSmoothing);
		rb.velocity = new Vector3(newXManeuver, newYManeuver, 0.0f);
		rb.position = new Vector3
		(
			Mathf.Clamp(rb.position.x, camMin.x, camMax.x),
			Mathf.Clamp(rb.position.y, camMin.y - 100.0f, camMax.y + 100.0f),
			0.0f
		);
	}

	public void TakeDamage()
	{
		switch (state)
		{
			case BossState.INTRO:
				return;
			case BossState.PHASE1:
				if (leftEye.IsDead() && rightEye.IsDead())
					NextPhase();
				break;
			case BossState.PHASE2:
				if (leftEye.IsDead() && rightEye.IsDead() && bigEye.IsDead())
					NextPhase();
				break;
			case BossState.PHASE3:
				if (leftEye.IsDead() && rightEye.IsDead() && bigEye.IsDead())
					NextPhase();
				break;
		}
		if (!flashing) StartCoroutine(Flash(new Color32(255, 166, 166, 130)));
	}

	IEnumerator Flash(Color32 flashcolor)
	{
		flashing = true;
		for (var n = 0; n < 4; n++)
		{
			sprite.material.color = Color.white;
			yield return new WaitForSeconds(.03f);
			sprite.material.color = flashcolor;
			yield return new WaitForSeconds(.03f);
		}
		sprite.material.color = Color.white;
		flashing = false;
	}

	IEnumerator Appear()
	{
		yield return new WaitForSeconds(2);
		targetYManeuver = -0.3f;
		yield return new WaitForSeconds(10);
		targetYManeuver = 0;
		NextPhase();
	}

	IEnumerator Evade()
	{
		yield return new WaitForSeconds(2);

		while (true)
		{
			targetXManeuver = Random.Range(1, dodge) * -Mathf.Sign(transform.position.x);
			yield return new WaitForSeconds(Random.Range(maneuverTime.x, maneuverTime.y));
			targetXManeuver = 0;
			yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));
		}
	}

	IEnumerator Regenerate()
	{
		targetXManeuver = 0;
		yield return new WaitForSeconds(2);
		audioController.PlaySingle(phase3Sound, 0.7f);
		yield return new WaitForSeconds(1);
		StartCoroutine(Flash(new Color32(255, 44, 244, 255)));
		yield return new WaitForSeconds(1);
		leftEye.Revive();
		rightEye.Revive();
		bigEye.Revive();
		yield return new WaitForSeconds(1);
		NextPhase();
	}

}
