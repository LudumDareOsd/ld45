using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
	private float currentSpeed;
	private float targetManeuver;
	private LTBezierPath[] enemyPaths;
	private Rigidbody2D rb;
	private int lifetime;
	private GameController gameController;
	private int wave;

	void Start()
	{
		var gameControllerObject = GameObject.FindWithTag("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent<GameController>();
		}

		wave = gameController.GetWave();

		lifetime = 0;
		rb = GetComponent<Rigidbody2D>();
		currentSpeed = rb.velocity.y;
		// Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 1.1f, 0.0f));

		//It goes in the order: startPoint, endControl, startControl, endPoint - 
		//Note: the control for the end and start are reversed! This is just a quirk of the API.
		enemyPaths = new LTBezierPath[] {
			new LTBezierPath(new Vector3[] {
				Camera.main.ViewportToWorldPoint(new Vector3(0f, 1.1f, 0f)), Camera.main.ViewportToWorldPoint(new Vector3(0.7f, 0.9f, 0f)),
				Camera.main.ViewportToWorldPoint(new Vector3(0.3f, 0.3f, 0f)), Camera.main.ViewportToWorldPoint(new Vector3(1.1f, -0.1f, 0f))
			}),
			new LTBezierPath(new Vector3[] {
				Camera.main.ViewportToWorldPoint(new Vector3(1.1f, 1.1f, 0f)), Camera.main.ViewportToWorldPoint(new Vector3(0.1f, 0.7f, 0f)),
				Camera.main.ViewportToWorldPoint(new Vector3(0.7f, 0.3f, 0f)), Camera.main.ViewportToWorldPoint(new Vector3(-0.1f, -0.1f, 0f))
			}),
			new LTBezierPath(new Vector3[] {
				Camera.main.ViewportToWorldPoint(new Vector3(1.1f, 1.1f, 0f)), Camera.main.ViewportToWorldPoint(new Vector3(0.1f, 0.7f, 0f)),
				Camera.main.ViewportToWorldPoint(new Vector3(0.7f, 0.3f, 0f)), Camera.main.ViewportToWorldPoint(new Vector3(-0.1f, -0.1f, 0f))
			})

		};
		
		//enemyPath = new LTBezierPath(new Vector3[] {
		//	new Vector3(0f, 0f, 0f), new Vector3(1f, 0f, 0f),
		//	new Vector3(1f, 0f, 0f), new Vector3(1f, 1f, 0f)
		//});
		//StartCoroutine(Evade());
		//LeanTween.move(gameObject, enemyPath, 4.0f).setOrientToPath(true).setDelay(1f).setEase(LeanTweenType.easeInOutQuad); // animate

	}

	void FixedUpdate()
	{
		//targetManeuver = Random.Range(1, 2) * -Mathf.Sign(transform.position.x);
		//var newManeuver = Mathf.MoveTowards(rb.velocity.x, targetManeuver, Time.deltaTime * 0.1f);
		//rb.velocity = new Vector3(newManeuver, -0.1f, currentSpeed);
		//rb.position = new Vector3
		//(
		//	rb.position.x,
		//	rb.position.y,
		//	0
		//	//Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
		//	//0.0f,
		//	//Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
		//);
		//rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);



		var p = (lifetime / 400.0f);
		Vector3 pt = enemyPaths[wave % enemyPaths.Length].point(p); // retrieve a point along the path

		rb.position = pt;


		if (p > 1.0f)
		{
			Destroy(gameObject);
		}

		//LeanTween.move(lt, ltPath.vec3, 4.0f).setOrientToPath(true).setDelay(1f).setEase(LeanTweenType.easeInOutQuad); // animate
		//LeanTween.move(gameObject, enemyPath, 4.0f).setOrientToPath(true).setDelay(1f).setEase(LeanTweenType.easeInOutQuad); // animate



		//LeanTween.move(gameObject, [pt1, control2, control3, pt4], 1.0);
		//You can also chain different paths together very easily, like
		//LeanTween.move(gameObject, [pt1, control2, control3, pt4, pt4, control5, control6, pt7..... etc], 1.0);

		//And if you do not wish it to move at a constant speed you can pass an easing function like:
		//LeanTween.move(gameObject, [pt1, control2, control3, pt4], 1.0, { "ease":LeanTweenType.easeInOutQuad} );


		lifetime++;
	}
}
