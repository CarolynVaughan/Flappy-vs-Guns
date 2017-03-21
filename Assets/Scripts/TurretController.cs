using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {
	public bool firing;
	public GameObject target;
	public float turretVelocity;
	public GameObject bulletPrefab;
	public float bulletVelocity;
	public int rateOfFire;

	protected int countdown;
	protected List<GameObject> bullets;

	// Use this for initialization
	void Start()
	{
		bullets = new List<GameObject>();
		countdown = 0;
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}

	void OnDestroy()
	{
		foreach (GameObject bullet in bullets) DestroyObject(bullet);
	}

	protected virtual void SpawnBullet()
	{
	}

	void FixedUpdate()
	{
		if (firing)
		{
			if (countdown == 0)
			{
				SpawnBullet();
				countdown = rateOfFire;
			}
			else countdown--;
		}

		if (bullets.Count > 0)
		{
			for (int index = 0; index < bullets.Count; index++)
			{
				GameObject bullet = bullets[index];
				if (!bullet.GetComponent<BulletController>().exploded)
				{
					float movedX = bullet.transform.localPosition.x + (bulletVelocity * Mathf.Cos(Mathf.Deg2Rad * bullet.transform.localEulerAngles.z));
					if (target.GetComponent<FlappyController>().moving) movedX += turretVelocity;
					float movedY = bullet.transform.localPosition.y + (bulletVelocity * Mathf.Sin(Mathf.Deg2Rad * bullet.transform.localEulerAngles.z));
					bullet.transform.localPosition = new Vector3(movedX, movedY, bullet.transform.localPosition.z);

					if (bullet.transform.localPosition.x < -10 || bullet.transform.localPosition.x > 10 || bullet.transform.localPosition.y < -5 || bullet.transform.localPosition.y > 5)
					{
						// Destroy bullets when they go off the screen
						bullets.RemoveAt(index);
						DestroyObject(bullet);
						index--;
					}
				}
				else
				{
					bullets.RemoveAt(index);
					DestroyObject(bullet);
					index--;
				}
			}
		}
	}
}
