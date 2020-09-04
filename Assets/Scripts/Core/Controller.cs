using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public partial class Controller : MonoBehaviour
{
	// Pause speed/state management
	private float pauseSpeed = 0;
	public bool Paused { get; set; }

	// Player specific items.
	private GameObject player;
	private static PlayerController pc;
	public bool PlayerFirstRun { get; set; }

	// Scenes list for changing level.
	private string[] nonLevelScenes = {"Title", "Shop", "ScoreScene"};

	// Game difficulty components.
	public static int playerSpeed = 4;
	public int levelSeed;

	// Use this for initialization
	void Start ()
	{
		PlayerFirstRun = true;
		levelSeed = CreateLevelSeed();

		if(PlayerPrefs.GetFloat("DistanceHiScore-" + levelSeed) > 0)
			distanceHiScore = PlayerPrefs.GetFloat("DistanceHiScore-" + levelSeed);

		if(PlayerPrefs.GetInt("PointsHiScore-" + levelSeed) > 0)
			bestPoints = PlayerPrefs.GetInt("PointsHiScore-" + levelSeed);

		if(PlayerPrefs.GetString("PlayerName") != "")
			playerName = PlayerPrefs.GetString("PlayerName");

		canvasMenuManager = GameObject.Find("Canvas").GetComponent<MenuManager>();

		// Flatten my high score and name for testing.
		// bestPoints = 0;
		// playerName = "";

		if (!Array.Exists (nonLevelScenes, element => element == SceneManager.GetActiveScene ().name))
			InitialSetup ();
	}

	private void Update ()
	{
		// These only run if we are not at the title screen.
		if(!Array.Exists(nonLevelScenes, element => element == SceneManager.GetActiveScene().name))
		{
			// Catch all - if the game objects are not detected for some reason:
			if (displayTextGO == null) 
			{
				// Force a reload of the initial setup.
				InitialSetup();
			}

            // Update the coin count with the coins.
            totalCoinsText.text = totalPlayerCoins.ToString();
            currentPointsText.text = points.ToString();

			if (Input.GetButtonDown ("Pause") && !pc.isDead()) 
			{
				PauseToggle();
			}
		}

	}

	public void PauseToggle()
	{
						if (Paused) 
				{
					// Game is paused, unpause.
					Time.timeScale = 1.0f;
					Paused = false;
					canvasMenuManager.ActivateMenu(4);
				} 
				else 
				{
					// Game is unpaused, pause it.
					Time.timeScale = pauseSpeed;
					Paused = true;
					displayText.text = "Paused";
					canvasMenuManager.ActivateMenu(5);
				}
	}

	private void SetHighScore ()
	{
		// Update the points hi-score.
		if(points > bestPoints)
		{
			bestPoints = points;
			// Save it to player prefs.
			PlayerPrefs.SetInt("PointsHiScore-" + levelSeed, bestPoints);
			PlayerPrefs.Save ();

			Debug.Log("New Hi-Score");
			// If they have a high score, let them enter initials.
			canvasMenuManager.ActivateMenu(3);
		}
		else
			canvasMenuManager.ActivateMenu(2);

		if (distanceScore > distanceHiScore) {
			// New high score, update the scoreboard.
			distanceHiScore = distanceScore;
			// Save it to player prefs.
			PlayerPrefs.SetFloat("DistanceHiScore-" + levelSeed, distanceHiScore);
			PlayerPrefs.Save ();
		}
	}

	private void InitialSetup ()
	{
		// Pause the game.
		Paused = true;
		Time.timeScale = 0;

		// Assign the player game object.
		player = GameObject.FindWithTag ("Player");
		pc = player.GetComponent<PlayerController> ();

		// Setup all hud components for use.
		AssignHudComponents();

		// Reset the score.
		distanceScore = 0;
        points = 0;
		bestPointsText.text = bestPoints.ToString();
		SetHighScore ();

		// Reset current coins:
		SetUpCoins ();

		// Reactivate the new game hud.
		canvasMenuManager.ActivateMenu(0);
			
	}

	// Controls for various touch buttons.
	public void StartGame()
	{
		// Unpause the game.
		Paused = false;
		Time.timeScale = 1;
	}

	public void ReloadLevel ()
	{
		// Save coin score to player prefs.
		Time.timeScale = 1;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		EventSystem.current.SetSelectedGameObject(null);
	}

	public void QuitGame()
	{
		Application.Quit();
		EventSystem.current.SetSelectedGameObject(null);
	}

    public void AddPoints(int pointsToAdd)
    {
        points += pointsToAdd;
    }

	public int CreateLevelSeed()
	{
				// Set our random seed from the current date:
		string todaysDate = System.DateTime.Now.ToString("ddMMyyyy");
		todaysDate = todaysDate.Replace("/", "");
		int todaysDateInt = 255;

		// Try to convert the string to and int.
		try
		{
			todaysDateInt = int.Parse(todaysDate);
		}
		catch (System.Exception)
		{
			Debug.LogWarning("Date was invalid, could not parse");
		}

		return todaysDateInt;
	}

	public void PlayerDied()
	{
		// Capture the players x Position.
		distanceScore = player.transform.position.x;
		PlayerFirstRun = false;
		Paused = true;
		// Save coins to player prefs
		SaveCoinsToPrefs();
		// Check if this also needs an update to the best score.
		SetHighScore ();
	}
}
