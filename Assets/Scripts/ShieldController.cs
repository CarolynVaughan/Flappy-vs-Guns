using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
	void OnTriggerEnter(Collider objectHit)
	{
		if (objectHit.name.StartsWith("Bullet")) gameObject.SetActive(false);
	}
}
