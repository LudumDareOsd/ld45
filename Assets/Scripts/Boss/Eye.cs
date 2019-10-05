using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour
{
	public int health = 30;
	private bool dead = false;
	private SpriteRenderer sprite;
	private Collider2D coll;
	private float spreadtime = 0.0f;
	private HardPoint hp;

	void Start()
    {
		sprite = GetComponent<SpriteRenderer>();
		coll = GetComponent<CircleCollider2D>();
		hp = GetComponentInChildren<HardPoint>();
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

		if (!sprite.enabled && hp)
		{
			hp.Fire();
		}

		//bigEye.GetComponentInChildren<HardPoint>().Fire();
		spreadtime += Time.deltaTime;
	}

	public void ToggleEye()
	{
		if (dead) return;
		sprite.enabled = !sprite.enabled;
		coll.enabled = !coll.enabled;
		spreadtime = 0.0f;
	}

	public void Kill()
	{
		sprite.enabled = true;
		coll.enabled = false;
		dead = true;
	}

	public bool IsDead()
	{
		return dead;
	}

}
