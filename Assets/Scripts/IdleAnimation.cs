using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAnimation : MonoBehaviour
{
	public Sprite firstFrame;
	public Sprite secondFrame;
	[Range(0.0f, 2.0f)]
	public float animationTime;

	private SpriteRenderer sprite;
	private float flipTime = 0.0f;
	private bool flip = false;

	// Start is called before the first frame update
	void Awake()
    {
		sprite = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update()
    {
		flipTime += Time.deltaTime;
		if (flipTime > animationTime)
		{
			flipTime = 0.0f;
			flip = !flip;
			if (flip)
				sprite.sprite = firstFrame;
			else
				sprite.sprite = secondFrame;
		}
    }
}
