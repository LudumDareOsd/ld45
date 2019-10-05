using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour
{
	public int health = 30;
	private bool dead = false;
	private SpriteRenderer sprite;
	private Collider2D coll;

	void Start()
    {
		sprite = GetComponent<SpriteRenderer>();
		coll = GetComponent<CircleCollider2D>();
	}

	public void ToggleEye()
	{
		sprite.enabled = !sprite.enabled;
		coll.enabled = !coll.enabled;
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
