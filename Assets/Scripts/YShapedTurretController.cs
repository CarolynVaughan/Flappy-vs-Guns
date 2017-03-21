using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YShapedTurretController : TurretController
{
	bool fireLeft = true;

	protected override void SpawnBullet()
	{
		float angle;

		if (gameObject.transform.localEulerAngles.z == 0) angle = fireLeft ? 135 : 45;
		else angle = fireLeft ? -45 : -135;

		GameObject newBullet = GameObject.Instantiate(bulletPrefab);
		newBullet.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + (gameObject.transform.localEulerAngles.z == 0 ? 4.45f : -4.45f), 0);
		newBullet.transform.localEulerAngles = new Vector3(0, 0, angle);

		bullets.Add(newBullet);

		fireLeft = !fireLeft;
	}
}
