using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounds : MonoBehaviour
{
	private Vector2 screenBounds;

	private void LateUpdate()
	{
		screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

		var viewpos = transform.position;
		viewpos.x = Mathf.Clamp(viewpos.x, (screenBounds.x * -1 + 1), screenBounds.x - 1);
		viewpos.y = Mathf.Clamp(viewpos.y, (screenBounds.y * -1 + 1), screenBounds.y - 1);

		transform.position = viewpos;

	}
}
