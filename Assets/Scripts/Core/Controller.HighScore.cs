using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public partial class Controller : MonoBehaviour
{
    // Score tracking items.
    	// score display and management.
	public float DistanceScore {
		get { return distanceScore; }
	}
    private int points, bestPoints;
	private float distanceScore, distanceHiScore;

    // // Used for pixellation of the camera effect. NOT USED CURRENTLY.
    // int xPixelationValue = 10;
    // int yPixelationValue= 5;

    // The players entered name. Used for uploading high scores.
    string playerName;

    public void SubmitHighScore()
    {
        playerName = nameInputField.GetComponent<InputField>().text;

        if(playerName == "")
            Debug.LogError("Player name must be something");
        else
        {
            PlayerPrefs.SetString("PlayerName", playerName);
            StartCoroutine(UploadHighScore());
        }

        // ReloadLevel();
    }
    IEnumerator UploadHighScore()
    {
        WWWForm form = new WWWForm();
        // Build the form for submission.
        form.AddField("name", playerName);
        form.AddField("seed_key", levelSeed.ToString());
        form.AddField("score", points.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post("https://urlToPost.to", form))
        {
            yield return www.SendWebRequest();

            if(www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("High Score Submitted!");
            }

            ReloadLevel();
        }
    } 
}
