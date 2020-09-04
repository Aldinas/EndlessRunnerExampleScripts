using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script should be used for managing anything related to the coins and other collectibles the player
// Is able to collect.
public partial class Controller : MonoBehaviour {

	// Getters for reading current coins value.
	public int CurrentPlayerCoins {
		get { return totalPlayerCoins; }
	}

	// private int currentPlayerCoins = 0;
	private int totalPlayerCoins = 0;
	private int coinMultiplier = 1;
	private int multiplierUpThreshold;
	// private int coinsThisRun = 0;
	private float multiplierScalingConstant = 0.4f;

	public void SetUpCoins()
	{
		// Check to see if this is a new seed since the player last played.
		if(PlayerPrefs.GetInt("PreviousSeed") != levelSeed)
		{
			// Seeds have changed, store this seed as the new previous seed.
			PlayerPrefs.SetInt("PreviousSeed", levelSeed);
			// Reset coins for the new run.
			PlayerPrefs.SetInt ("Coins", 0);
			totalPlayerCoins = 0;
			PlayerPrefs.Save();
		}
		else
		{
			if(totalPlayerCoins == 0)
				totalPlayerCoins = PlayerPrefs.GetInt("Coins");
		}

		coinMultiplier = 1;
		// Set out next multiplayer point dynamically with a bit of math.
		multiplierUpThreshold = (int)Mathf.Round(Mathf.Pow(coinMultiplier / multiplierScalingConstant, 2));
	}

	public void AddCoins(int amount = 1)
	{
		totalPlayerCoins += (amount);

		// if(totalPlayerCoins >= multiplierUpThreshold)
		// {
		// 	IncreaseMultiplier();
		// }
	}

	public void SpendCoins(int amount)
	{
		totalPlayerCoins -= amount;
	}

	public void IncreaseMultiplier()
	{
		// Increase the multiplier by one, and set the next multiplier threshold based on our scaling variable.
		coinMultiplier++;
		playerSpeed++;
		multiplierUpThreshold = ((int)Mathf.Round(Mathf.Pow(coinMultiplier / multiplierScalingConstant, 2))) * coinMultiplier;
	}

	public void SaveCoinsToPrefs()
	{
		Debug.Log("Saving total player coins of: " + totalPlayerCoins + " to player prefs Coins");
		PlayerPrefs.SetInt ("Coins", totalPlayerCoins);
		PlayerPrefs.Save();
	}
}
