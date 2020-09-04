using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SummaryController : MonoBehaviour 
{
	
	GameObject scoreTitle, collectedCoinsTitle, scoreText, collectedCoinsText, finishButton, controller;
	int coinsInt;
	float scoreFloat;
	int gamesPlayed = 0;

	void Start()
	{
		if(PlayerPrefs.GetInt("GamesPlayed") > 0)
			gamesPlayed = PlayerPrefs.GetInt("GamesPlayed");

		// Assign the text fields to the code so they can be manipulated.
		scoreTitle = GameObject.Find ("scoreTitle");
		collectedCoinsTitle = GameObject.Find ("collectedCoinsTitle");
		// totalCoinsTitle = GameObject.Find ("totalCoinsTitle");
		scoreText = GameObject.Find ("scoreText");
		collectedCoinsText = GameObject.Find ("collectedCoinsText");
		// totalCoinsText = GameObject.Find ("totalCoinsText");
		finishButton = GameObject.Find ("Finish Button");
		controller = GameObject.FindGameObjectWithTag ("GameController");
		scoreFloat = controller.GetComponent<Controller> ().DistanceScore;
		coinsInt = controller.GetComponent<Controller> ().CurrentPlayerCoins;

		// Disable all items.
		scoreTitle.SetActive (false);
		collectedCoinsTitle.SetActive (false);
		// totalCoinsTitle.SetActive (false);
		scoreText.SetActive (false);
		collectedCoinsText.SetActive (false);
		// totalCoinsText.SetActive (false);
		finishButton.SetActive (false);
		// finishButton.GetComponent<Button> ().onClick.AddListener(ReturnToMainMenu);

		// Set relevant values to the score items.
		scoreText.GetComponent<Text>().text = scoreFloat.ToString();
		collectedCoinsText.GetComponent<Text>().text = coinsInt.ToString();

		// Re-enable time (if stopped)
		Time.timeScale = 1;

		StartCoroutine (ShowResults ());
	}

	// Work through the results and display them every second.
	IEnumerator ShowResults()
	{
		yield return new WaitForSeconds (0.5f);
		scoreTitle.SetActive (true);
		scoreText.SetActive (true);

		// Increase played games count
		gamesPlayed++;
		PlayerPrefs.SetInt ("GamesPlayed", gamesPlayed);
		PlayerPrefs.Save ();			
			
		yield return new WaitForSeconds (0.5f);
		collectedCoinsTitle.SetActive (true);
		collectedCoinsText.SetActive (true);
		yield return new WaitForSeconds (0.5f);
		// totalCoinsTitle.SetActive (true);
		// totalCoinsText.SetActive (true);
		yield return new WaitForSeconds (0.5f);
		finishButton.SetActive (true);
		StopCoroutine (ShowResults ());
	}
}
