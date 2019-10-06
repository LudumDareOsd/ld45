using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public GameObject lifePrefab;
	public Sprite orangeSprite;

	private int wave, bosswave = 10;
	private Vector3 spawnPos, spawnPosEye;
	private GameObject enemyPrefab, enemyEyePrefab, bossPrefab, powerupPrefab, enemiesContainer;
	private GameObject lifeParent;
	private bool intro = true;
	private bool isDead = false;
	private bool isWin = false;
	private GameObject title;
	private GameObject gameOver;
	private GameObject pressStart;
	private GameObject congrats;

	private void Start()
	{
		powerupPrefab = (GameObject)Resources.Load("Prefabs/Powerup", typeof(GameObject));
		enemyPrefab = (GameObject)Resources.Load("Prefabs/Enemy", typeof(GameObject));
		enemyEyePrefab = (GameObject)Resources.Load("Prefabs/EnemyEye", typeof(GameObject));
		bossPrefab = (GameObject)Resources.Load("Prefabs/Boss", typeof(GameObject));
		enemiesContainer = GameObject.FindWithTag("Enemies");
		spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(-1.0f, 1.1f, 0.0f));
		spawnPosEye = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 1.1f, 0.0f));
		title = GameObject.Find("Title");
		gameOver = GameObject.Find("GameOver");
		pressStart = GameObject.Find("PressStart");
		congrats = GameObject.Find("Congrats");

		gameOver.SetActive(false);
		pressStart.SetActive(false);
		congrats.SetActive(false);


		StartCoroutine(IntroSequence());

		Restart();

		lifeParent = GameObject.Find("Lifes");
	}

	public void FixedUpdate()
	{
		if (isDead || isWin)
		{
			if (Input.GetButton("Fire3"))
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}
		else
		{
			var i = 0;
			foreach (Transform child in lifeParent.transform)
			{
				child.gameObject.transform.position = new Vector3(32 + (40 * i), Screen.height - 32, 0);
				i++;
			}
		}


	}

	private void Restart()
	{
		// TODO: Code to reset player restart map etc
		wave = 1;
	}

	public bool IsIntro()
	{
		return intro;
	}

	public void Dead()
	{
		if (!isWin)
		{
			isDead = true;

			gameOver.SetActive(true);
			pressStart.SetActive(true);
		}
	}

	public void Win()
	{
		if (!isDead)
		{
			isWin = true;

			congrats.SetActive(true);
			pressStart.SetActive(true);
		}
	}

	private IEnumerator IntroSequence()
	{
		intro = true;
		var po = Instantiate(powerupPrefab, new Vector3(0, 15, 0), new Quaternion());
		po.GetComponent<Rigidbody2D>().gravityScale = 0.1f;

		yield return new WaitForSeconds(8);
		StartCoroutine(SpawnWaves());
		StartCoroutine(SpawnEyes());
		RenderLife(3);
		intro = false;
		title.SetActive(false);
	}

	private IEnumerator SpawnEyes()
	{
		yield return new WaitForSeconds(8);
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(5, 10 + bosswave * 2 - GetWave() * 2));
			var spawnPosition = new Vector3(Random.Range(-spawnPosEye.x, spawnPosEye.x), spawnPosEye.y, 0.0f);
			var enemy = Instantiate(enemyEyePrefab, spawnPosition, Quaternion.identity);
			enemy.transform.SetParent(enemiesContainer.transform);
		}
	}

	private IEnumerator SpawnWaves()
	{
		yield return new WaitForSeconds(4);
		while (true)
		{
			//Debug.Log("Spawning wave " + wave.ToString());
			if (wave == bosswave)
			{
				Instantiate(bossPrefab);
			}

			for (var i = 0; i < GetWave(); i++)
			{
				var spawnPosition = new Vector3(spawnPos.x, spawnPos.y, 0.0f);
				//var enemy = Instantiate(enemyPrefab, spawnPosition, spawnRotation);
				var enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
				enemy.transform.SetParent(enemiesContainer.transform);
				yield return new WaitForSeconds(1);
			}
			yield return new WaitForSeconds(10);
			wave++;
		}
	}

	public void SpawnPowerup(GameObject enemy, int mobwave = 1)
	{
		var spawnChance = Random.Range(0.0f, 0.8f - ((bosswave - mobwave) / bosswave));
		if (spawnChance < 0.2f)
		{
			var powerup = Instantiate(powerupPrefab, enemy.transform.position, enemy.transform.rotation);

			var type = Random.Range(1, 3) == 1 ? WeaponType.Plasma : WeaponType.Beam;

			if (type.Equals(WeaponType.Beam)) {
				powerup.GetComponent<Powerup>().weaponType = WeaponType.Beam;
				powerup.GetComponent<SpriteRenderer>().sprite = orangeSprite;
			}
		}
	}

	public int GetWave(bool real = false) { return (wave > bosswave && !real ? bosswave : wave); }

	public void RenderLife(int lifes)
	{
		foreach (Transform child in lifeParent.transform)
		{
			Destroy(child.gameObject);
		}

		for (var i = 0; i < lifes; i++)
		{
			Instantiate(lifePrefab, new Vector3(32 + (40 * i), Screen.height - 32, 0), lifeParent.transform.rotation, lifeParent.transform);
		}

	}
}
