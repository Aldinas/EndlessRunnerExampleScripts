using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrackPlayer : MonoBehaviour 
{
	public float customCameraXOffset;
	private GameObject playerCharacter;
	private float cameraXOffset;

	// Use this for initialization
	void Start () 
	{
		playerCharacter = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		float playerPosX = playerCharacter.transform.position.x; 
		float offsetX = playerPosX + customCameraXOffset;
		transform.position = new Vector3(offsetX, transform.position.y, transform.position.z);
	}
}
