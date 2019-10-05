using UnityEngine;

public class HardPoint : MonoBehaviour
{
	public GameObject bullet;
	public float rateOfFire;
	private float cd = 0f;

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
			cd = rateOfFire;
		}
	}

	private void CreateBullet() {
		Instantiate(bullet, transform.position, transform.rotation);
	}

	
}
