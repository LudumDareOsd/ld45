﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
	private GameController gameController;

	// Start is called before the first frame update
	void Start()
    {
		gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		var rb = GetComponent<Rigidbody2D>();
		rb.velocity.Set(0.0f, 0.3f);
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("PICKUP POWERUP");
		gameController.PickupPowerup();
		Destroy(gameObject);
	}
}
