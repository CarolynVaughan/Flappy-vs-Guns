using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
	public bool exploded;

	void OnTriggerEnter()
	{
		exploded = true;
	}
}
