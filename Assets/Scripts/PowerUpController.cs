using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
	public bool claimed;

	void OnTriggerEnter(Collider objectHit)
	{
		Debug.Log(objectHit);
		if (objectHit.name.StartsWith("Flappy")) claimed = true;
	}
}
