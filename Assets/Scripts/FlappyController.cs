using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyController : MonoBehaviour
{
	public Animator theAnimator;
	public GameObject theShield;

	// State flags
	public bool moving;
	public bool dead;
	public bool dying;
	public bool restarting;

	// Movement parameters
	public float initialVelocity;
	public float gravity;
	private float velocity;

	void Murdered()
	{
		theShield.SetActive(false);
		moving = false;
		dead = true;
		dying = true;
		theAnimator.SetTrigger("Die");
		velocity = 0.05f;
	}

	// Use this for initialization
	void Start()
	{
		restarting = false;
		moving = false;
		dead = false;
	}
	
	// Update is called once per frame
	void Update()
	{
		if ((Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0)) && !dead)
		{
			if (restarting) restarting = false;
			else
			{
				moving = true;
				theAnimator.SetTrigger("Flap");
				velocity = initialVelocity;
			}
		}
			
	}

	void FixedUpdate()
	{
		if (moving)
		{
			gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y + velocity, gameObject.transform.localPosition.z);
			velocity += gravity;
			if (gameObject.transform.localPosition.y <= -5)
			{
				Murdered();
			}
			else if (gameObject.transform.localPosition.y > 5.25f)
			{
				gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, 5.25f, gameObject.transform.localPosition.z);
			}
		}
		else if (dying)
		{
			gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y + velocity, gameObject.transform.localPosition.z);
			velocity += gravity;
			if (gameObject.transform.localPosition.y <= -5.6) dying = false;
		}
	}

	void OnTriggerEnter(Collider objectHit)
	{
		if (!dead)
		{
			if (objectHit.name.StartsWith("Turret"))
			{
				TurretController theTurret = objectHit.GetComponent<TurretController>();
				theTurret.firing = true;
			}
			else if (objectHit.name.StartsWith("Shield Power Up")) theShield.SetActive(true);
			else if (!objectHit.name.StartsWith("Shield")) Murdered();
		}
	}

	void OnTriggerExit(Collider objectHit)
	{
		if (objectHit.name.StartsWith("Turret"))
		{
			TurretController theTurret = objectHit.GetComponent<TurretController>();
			theTurret.firing = false;
		}
	}
}
