using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitToScreen : MonoBehaviour
{
    void Update()
    {
		var height = Camera.main.orthographicSize * 2.0f;
		var width = height * Screen.width / Screen.height;
		transform.localScale = new Vector3(width, height, 0.1f);
	}
}
