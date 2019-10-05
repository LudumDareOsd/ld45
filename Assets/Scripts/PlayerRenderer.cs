using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRenderer : MonoBehaviour
{
	public Sprite middle;
	public Sprite left;
	public Sprite right;

	private PlayerState state = PlayerState.Straight;
	private SpriteRenderer renderer;

	public void Start()
	{
		renderer = GetComponent<SpriteRenderer>();
	}

	public void TurnLeft() {
		if (state != PlayerState.Left) {
			renderer.sprite = left;
			state = PlayerState.Left;
		}
	}

	public void TurnRight() {
		if (state != PlayerState.Right)
		{
			renderer.sprite = right;
			state = PlayerState.Right;
		}
	}

	public void Straight() {
		if (state != PlayerState.Straight)
		{
			renderer.sprite = middle;
			state = PlayerState.Straight;
		}
	}

	public void Forward() {

	}

	enum PlayerState {
		Left,
		Right,
		Straight
	}
}
