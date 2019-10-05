using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	private int score;

	void Start()
	{

	}

	void Update()
	{

	}

	void Restart()
	{
		// TODO: Code to reset player restart map etc
		score = 0;
	}

	public void AddScore()
	{
		score++;
	}

	public void GameOver()
	{
		Restart();
	}

}
