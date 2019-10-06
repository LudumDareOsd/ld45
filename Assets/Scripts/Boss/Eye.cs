using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EyeState {
	OPEN, CLOSED, DEAD
}

public class Eye : MonoBehaviour
{
	public int health = 30;
	public int reviveHealth = 40;
	public AudioClip deathSound;

	private Collider2D coll;
	private float spreadtime = 0.0f;
	private HardPoint hp;
	private Boss boss;
	private SpriteRenderer closedSprite, deadSprite;
	private AudioController audioController;
	private EyeState state;

	void Start()
    {
		boss = transform.parent.GetComponent<Boss>();
		closedSprite = transform.Find("Closed").GetComponent<SpriteRenderer>();
		deadSprite = transform.Find("Dead").GetComponent<SpriteRenderer>();
		coll = GetComponent<Collider2D>();
		hp = GetComponentInChildren<HardPoint>();
		audioController = GameObject.Find("AudioController").GetComponent<AudioController>();
		SwitchState(EyeState.CLOSED);
	}

	private void LateUpdate()
	{
		// want a angle between like 100 and 260?
		var angle = 100 + (spreadtime * 32);
		if (angle > 260)
		{
			spreadtime = 0.0f;
			angle = 100;
		}
		hp.transform.rotation = Quaternion.Euler(0, 0, angle);

		if (state == EyeState.OPEN)
		{
			hp.Fire();
		}

		spreadtime += Time.deltaTime;
	}

	public void SwitchState(EyeState newState)
	{
		switch (newState)
		{
			case EyeState.OPEN:
				coll.enabled = true;
				closedSprite.enabled = false;
				deadSprite.enabled = false;
				spreadtime = 0.0f;
				break;
			case EyeState.CLOSED:
				coll.enabled = false;
				closedSprite.enabled = true;
				deadSprite.enabled = false;
				break;
			case EyeState.DEAD:
				coll.enabled = false;
				closedSprite.enabled = false;
				deadSprite.enabled = true;
				audioController.PlaySingle(deathSound, 0.7f);
				break;
			default:
				break;
		}
		state = newState;
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("PlayerBullet"))
		{
			health--;
			if (health < 1)
			{
				SwitchState(EyeState.DEAD);
			}
			boss.TakeDamage();
		}
	}

	public void Revive()
	{
		health = reviveHealth;
		SwitchState(EyeState.CLOSED);
	}

	public void ToggleOpen()
	{
		if (state == EyeState.DEAD)
			return;
		else if (state == EyeState.OPEN)
			SwitchState(EyeState.CLOSED);
		else if (state == EyeState.CLOSED)
			SwitchState(EyeState.OPEN);
	}

	public bool IsDead() { return state == EyeState.DEAD; }
	public EyeState GetState() { return state; }
}
