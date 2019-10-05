﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	private int wave, bosswave = 3;
	private Vector3 spawnPos;
	private Player player;
	private GameObject enemyPrefab, bossPrefab, powerupPrefab, enemiesContainer;
	private List<GameObject> enemies = new List<GameObject>();
	
	void Start()
	{
		powerupPrefab = (GameObject)Resources.Load("Prefabs/Powerup", typeof(GameObject));
		enemyPrefab = (GameObject)Resources.Load("Prefabs/Enemy", typeof(GameObject));
		bossPrefab = (GameObject)Resources.Load("Prefabs/Boss", typeof(GameObject));
		enemiesContainer = GameObject.FindWithTag("Enemies");
		player = GameObject.FindWithTag("Player").GetComponent<Player>();
		spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(-1.0f, 1.1f, 0.0f));
		StartCoroutine(SpawnWaves());
		Restart();
	}

	void Restart()
	{
		// TODO: Code to reset player restart map etc
		wave = 1;
	}

	public void GameOver()
	{
		// Show some gameover screen???
		Restart();
	}

	IEnumerator SpawnWaves()
	{
		yield return new WaitForSeconds(4);
		while (true)
		{
			Debug.Log("Spawning wave " + wave.ToString());

			if (wave == 3)
			{
				var boss = Instantiate(bossPrefab);
			}

			for (var i = 0; i < wave; i++)
			{
				var spawnPosition = new Vector3(spawnPos.x, spawnPos.y, 0.0f);
				Quaternion spawnRotation = Quaternion.identity;
				var enemy = Instantiate(enemyPrefab, spawnPosition, spawnRotation);
				enemy.transform.SetParent(enemiesContainer.transform);
				enemies.Add(enemy);
				yield return new WaitForSeconds(1);
			}
			yield return new WaitForSeconds(13);
			enemies.Clear();
			wave += wave >= bosswave ? 1 : 0;
		}
	}

	public void SpawnPowerup(GameObject enemy)
	{
		if (enemies.Count < 2)
		{
			Instantiate(powerupPrefab, enemy.transform.position, enemy.transform.rotation);
		}
		enemies.Remove(enemy);
	}

	public int GetWave() { return wave; }
}
