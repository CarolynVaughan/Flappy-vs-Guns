using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Engine : MonoBehaviour
{
	public FlappyController theBird;
	public List<GameObject> turretPrefabs;
	public GameObject powerUpPrefab;
	public float timeBetweenTurrets;
	public float scrollVelocity;

	public Text scoreMeter;
	public Text highScoreMeter;
	public GameObject finalScreen;
	public Text finalScoreMeter;
	public GameObject highScoreLabel;

	private float timeSinceLastTurret;
	private bool topTurret;
	private List<GameObject> turrets;
	private GameObject powerUp;
	private float distance;
	private float highScore;

	public void RestartGame()
	{
		theBird.theAnimator.SetTrigger("Reset");
		theBird.restarting = true;
		theBird.dead = false;
		theBird.transform.localPosition = Vector3.zero;
		theBird.transform.localEulerAngles = Vector3.zero;
		foreach (var turret in turrets) DestroyObject(turret);
		turrets.Clear();
		timeSinceLastTurret = 0;
		topTurret = true;
		distance = 0;
		scoreMeter.text = "Score\n0m";
		highScoreMeter.text = string.Format("High Score\n{0:0.##}m", highScore);;
		finalScreen.SetActive(false);
		if (powerUp != null)
		{
			DestroyObject(powerUp);
			powerUp = null;
		}
	}

	// Use this for initialization
	void Start()
	{
		timeSinceLastTurret = 0;
		topTurret = true;
		turrets = new List<GameObject>();
		powerUp = null;
		distance = 0;
	}
	
	// Update is called once per frame
	void Update()
	{
		if (theBird.moving)
		{
			timeSinceLastTurret += Time.deltaTime;
			distance += Time.deltaTime * 2;
		}
	}

	void FixedUpdate()
	{
		if (theBird.moving)
		{
			if (timeSinceLastTurret >= timeBetweenTurrets)
			{
				// Determine placement
				int chance = Random.Range(1, 10);
				if (chance > 1)	// 10% chance of no turret
				{
					if (chance > 5) topTurret = !topTurret;	// 50% chance of having the opposite orientation as the last one

					// Choose turret type
					chance = Random.Range(1, 10);

					int type = chance > 3 ? 0 : 1;	// 30% chance of Y-shaped turret

					// Create a new Turret
					GameObject newTurret = GameObject.Instantiate(turretPrefabs[type]);
					if (topTurret)
					{
						newTurret.transform.localPosition = new Vector3(10, 5 + Random.Range(0f, 3f), 0);
						newTurret.transform.localEulerAngles = new Vector3(0, 0, 180);
					}
					else
					{
						newTurret.transform.localPosition = new Vector3(10, -5 - Random.Range(0f, 3f), 0);
					}

					TurretController theController = newTurret.GetComponent<TurretController>();
					theController.turretVelocity = scrollVelocity;

					theController.target = theBird.gameObject;

					turrets.Add(newTurret);
				}
				timeSinceLastTurret = 0;

				if (powerUp == null)
				{
					// Check to see if we should spawn a power up
					chance = Random.Range(1, 20);
					if (chance <= 3)	// 15% chance
					{
						powerUp = GameObject.Instantiate(powerUpPrefab);
						powerUp.transform.localPosition = new Vector3(10, topTurret ? -2.5f : 2.5f, 0);
					}
				}
			}

			for (int index = 0; index < turrets.Count; index++)
			{
				GameObject turret = turrets[index];
				turret.transform.localPosition = new Vector3(turret.transform.localPosition.x + scrollVelocity, turret.transform.localPosition.y, turret.transform.localPosition.z);
				if (turret.transform.localPosition.x < -10)
				{
					// Destroy turrets when they go off the screen
					turrets.RemoveAt(index);
					DestroyObject(turret);
					index--;
				}
			}

			if (powerUp != null)
			{
				if (powerUp.GetComponent<PowerUpController>().claimed)
				{
					DestroyObject(powerUp);
					powerUp = null;
				}
				else
				{
					powerUp.transform.localPosition = new Vector3(powerUp.transform.localPosition.x + scrollVelocity, powerUp.transform.localPosition.y, powerUp.transform.localPosition.z);
					if (powerUp.transform.localPosition.x < -10)
					{
						// Destroy the power up when it goes off the screen
						DestroyObject(powerUp);
						powerUp = null;
					}
				}
			}

			scoreMeter.text = string.Format("Score\n{0:0.##}m", distance);
		}
		else if (theBird.dead && !theBird.dying && !finalScreen.activeSelf)
		{
			if (distance > highScore)
			{
				highScore = distance;
				highScoreLabel.SetActive(true);
			}
			else highScoreLabel.SetActive(false);
			finalScoreMeter.text = string.Format("{0:0.##}m", distance);
			finalScreen.SetActive(true);
		}
	}
}
