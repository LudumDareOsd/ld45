﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public GameObject lifePrefab;

	private int wave, bosswave = 1;
	private Vector3 spawnPos, spawnPosEye;
	private Player player;
	private GameObject enemyPrefab, enemyEyePrefab, bossPrefab, powerupPrefab, enemiesContainer;
	private List<GameObject> enemies = new List<GameObject>();

	private GameObject lifeParent;

	void Start()
	{
		powerupPrefab = (GameObject)Resources.Load("Prefabs/Powerup", typeof(GameObject));
		enemyPrefab = (GameObject)Resources.Load("Prefabs/Enemy", typeof(GameObject));
		enemyEyePrefab = (GameObject)Resources.Load("Prefabs/EnemyEye", typeof(GameObject));
		bossPrefab = (GameObject)Resources.Load("Prefabs/Boss", typeof(GameObject));
		enemiesContainer = GameObject.FindWithTag("Enemies");
		player = GameObject.FindWithTag("Player").GetComponent<Player>();
		spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(-1.0f, 1.1f, 0.0f));
		spawnPosEye = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 1.1f, 0.0f));
		StartCoroutine(SpawnWaves());
		StartCoroutine(SpawnEyes());
		Restart();

		lifeParent = GameObject.Find("Lifes");

		RenderLife(3);
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

	IEnumerator SpawnEyes()
	{
		yield return new WaitForSeconds(8);
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(5, 10 + bosswave * 2 - wave * 2));
			var spawnPosition = new Vector3(Random.Range(0.0f, spawnPosEye.x - 0.0f), spawnPosEye.y, 0.0f);
			var enemy = Instantiate(enemyEyePrefab, spawnPosition, Quaternion.identity);
			enemy.transform.SetParent(enemiesContainer.transform);
		}
	}

	IEnumerator SpawnWaves()
	{
		yield return new WaitForSeconds(4);
		while (true)
		{
			//Debug.Log("Spawning wave " + wave.ToString());
			if (wave == bosswave)
			{
				Instantiate(bossPrefab);
			}

			for (var i = 0; i < wave; i++)
			{
				var spawnPosition = new Vector3(spawnPos.x, spawnPos.y, 0.0f);
				//var enemy = Instantiate(enemyPrefab, spawnPosition, spawnRotation);
				var enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
				enemy.transform.SetParent(enemiesContainer.transform);
				enemies.Add(enemy);
				yield return new WaitForSeconds(1);
			}
			yield return new WaitForSeconds(10);
			enemies.Clear();
			wave += (wave > bosswave) ? 0 : 1;
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

	public void RenderLife(int lifes) {

		foreach (Transform child in lifeParent.transform)
		{
			Destroy(child.gameObject);
		}

		for (var i = 0; i < lifes; i++) {
			var tempLife = Instantiate(lifePrefab, new Vector3(32 + (40 * i), Screen.height - 32, 0), lifeParent.transform.rotation, lifeParent.transform);
		}

	}
}
