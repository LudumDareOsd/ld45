using UnityEngine;

public class HardPoint : MonoBehaviour
{
	public GameObject bullet;
	public AudioClip audioClip;
	public float rateOfFire;
	public float volume = 1f;
	private float cd = 0f;
	private AudioController audioController;

	private void Start()
	{
		audioController = GameObject.Find("AudioController").GetComponent<AudioController>();
	}

	void Update()
    {
		if (cd > 0)
		{
			cd -= Time.deltaTime;
		}
    }

	public void Fire()
	{
		if (cd <= 0) {
			CreateBullet();
			audioController.PlaySingle(audioClip, volume);
			cd = rateOfFire;
		}
	}

	private void CreateBullet() {
		Instantiate(bullet, transform.position, transform.rotation);
	}

	
}
