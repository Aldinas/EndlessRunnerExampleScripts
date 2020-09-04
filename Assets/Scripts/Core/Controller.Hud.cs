using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class Controller : MonoBehaviour
{
    // Contains items relative to HUD and other overlay/menu features.

    // The various text fields that need to be updated.
	private Text displayText, currentPointsText, bestPointsText, totalCoinsText, newHighScoreText;
	// HUD Buttons and other interactables.
	private GameObject displayTextGO,  nameInputField, highScoreCanvas, submitScoreButton, menuCanvas, shopCanvas;
    private MenuManager canvasMenuManager;
    
    //These two handle the fade to black.
    private Image black;
	private Animator fadeAnim;

    private void AssignHudComponents()
    {
        // Set up GameObjects
        displayTextGO = GameObject.Find ("Display Text");
        nameInputField = GameObject.Find ("InputField");
        submitScoreButton = GameObject.Find("Submit Score Button");

        // Set up Text UI components.
        displayText = displayTextGO.GetComponent<Text> ();
        totalCoinsText = GameObject.Find ("TotalCoins").GetComponent<Text> ();
		currentPointsText = GameObject.Find ("CurrentPointCount").GetComponent<Text> ();
		bestPointsText = GameObject.Find ("BestPointCount").GetComponent<Text> ();
		newHighScoreText = GameObject.Find ("NewHighScoreText").GetComponent<Text> ();

        // Set up Canvas
        highScoreCanvas = GameObject.Find("HighScoreUI");
        menuCanvas = GameObject.Find("MenuUI");
        shopCanvas = GameObject.Find("Shop Canvas");
    }

    // Make sure all the hud components are reset to a clean baseline to avoid code level changes.
    private void HudComponentsDefaults()
    {
        // Default display message is paused.
		displayText.text = "Paused";
    }



    public void HiScoreHud()
    {
        // Set up the submit button to use the correct instance of the controller.
        submitScoreButton.GetComponent<Button>().onClick.AddListener(SubmitHighScore);

        // Cache our input field.
        InputField nameIF = nameInputField.GetComponentInChildren<InputField>();

        // Player name is set, pre-populate the field.
        if(playerName != "")
            nameIF.text = playerName;
    
        // Activate the input field for clarity.
        nameIF.Select();
        //nameIF.ActivateInputField();
    }


}
