using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerTurretController : TurretController
{
	protected override void SpawnBullet()
	{
		float xDifference = target.transform.position.x - gameObject.transform.position.x;
		float yDifference = target.transform.position.y - (gameObject.transform.position.y + (gameObject.transform.localEulerAngles.z == 0 ? 4.45f : -4.45f));

		float angle = Mathf.Atan2(yDifference, xDifference);

		GameObject newBullet = GameObject.Instantiate(bulletPrefab);
		newBullet.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + (gameObject.transform.localEulerAngles.z == 0 ? 4.45f : -4.45f), 0);
		newBullet.transform.localEulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * angle);

		bullets.Add(newBullet);
	}
}
