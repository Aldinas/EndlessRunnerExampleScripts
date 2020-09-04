using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public partial class  PlayerController : MonoBehaviour
{
    public AudioClip[] grasslandFootsteps;
    public AudioClip jumpSound;
    public AudioClip pingSound;
    public AudioClip coinCollectSound;
    
    AudioSource audiosource;
    string terrain = null;

    public void PlayFootstep()
    {
        // Get the type of surface we are walking on.
        terrain = GetTerrain();

        // Based on the terrain type, pick a random sound to play from the available ones.
        switch(terrain)
        {
            case "Grassland": 
                int randomInt = Random.Range(0,grasslandFootsteps.Length);
                audiosource.pitch = Random.Range(0.8f, 1f);
                audiosource.PlayOneShot(grasslandFootsteps[randomInt]);
                break;
            default:
                Debug.Log("Playing Nothing!");
                break;
        }
    }

    string GetTerrain()
    {
        // Get the layermask of the ground.
        LayerMask groundMask = LayerMask.GetMask("Terrain Engine");
        // Add a raycast to detect the ground, rather than relying on velocity. 
        RaycastHit2D groundCheckRay = Physics2D.Raycast(transform.position, Vector2.down, 0.75f, groundMask);
        Debug.DrawRay(transform.position, Vector3.down, Color.green, 1f);
        string terrainType = groundCheckRay.collider?.GetComponent<TerrainTypeTag>().terrainType.ToString() ?? null;
        return terrainType;
    }

    // TODO: Refactor these so I can call one function with arguments rather than having multiple play-one-shot methods.
    public void PlayJumpSound()
    {
        // THe sound effect to play when performing a jump.
        audiosource.pitch = Random.Range(0.8f, 1f);
        audiosource.PlayOneShot(jumpSound);
    }

    public void PlaySuccessfulJumpSound(int pointValue)
    {
        // Plays the ping sound associated with a successful jump.
        // Usually when the floating text appears.
        // Adjust the pitch based on the value of points. Higher value = higher pitch.
        audiosource.pitch = pointValue;
        audiosource.PlayOneShot(pingSound);
    }

    public void PlayCoinCollectSound()
    {
        // Play sound effect when collecting.
        audiosource.pitch = 1;
        audiosource.PlayOneShot(coinCollectSound);
    }
}
