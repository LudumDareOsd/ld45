using UnityEngine;

public class Bullet : MonoBehaviour
{
	public Sprite sprite;
	public float velocity;

	public void Start()
	{
		var renderer = GetComponent<SpriteRenderer>();
		renderer.sprite = sprite;

		var body = GetComponent<Rigidbody2D>();
		body.velocity = transform.up * velocity;

		Destroy(gameObject, 5);
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		Destroy(gameObject);
	}
}
