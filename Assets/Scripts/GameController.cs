using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	private int wave;
	private Vector3 spawnPos;
	private GameObject enemyPrefab;
	private GameObject enemies;

	void Start()
	{

		enemyPrefab = (GameObject)Resources.Load("Prefabs/Enemy", typeof(GameObject));
			//GameObject.FindWithTag("Enemy");
		enemies = GameObject.FindWithTag("Enemies");
		spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 1.1f, 0.0f));
		StartCoroutine(SpawnWaves());
		Restart();
	}

	void Restart()
	{
		//Debug.Log("restart");

		// TODO: Code to reset player restart map etc
		wave = 8;
	}

	public void GameOver()
	{
		Restart();
	}

	public int GetWave() { return wave; }

	IEnumerator SpawnWaves()
	{
		yield return new WaitForSeconds(2);
		while (true)
		{
			Debug.Log("Spawning wave " + wave.ToString());

			for (var i = 0; i < wave; i++)
			{
				//var spawnPosition = new Vector3(Random.Range(-spawnPos.x, spawnPos.x), spawnPos.y, 0.0f);
				var spawnPosition = new Vector3(spawnPos.x, spawnPos.y, 0.0f);
				Quaternion spawnRotation = Quaternion.identity;
				var enemy = Instantiate(enemyPrefab, spawnPosition, spawnRotation);
				enemy.transform.SetParent(enemies.transform);
				yield return new WaitForSeconds(1);
			}
			yield return new WaitForSeconds(10);
			wave++;
		}
	}

}
