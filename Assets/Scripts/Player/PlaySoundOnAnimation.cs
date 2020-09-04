using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnAnimation : MonoBehaviour
{
    public PlayerController playerController;
    public void PlayWalkSound()
    {
        // audioPlayer.PlayOneShot(walkSounds[Random.Range(0,walkSounds.Length)], 0.2f);
        playerController.PlayFootstep();
    }
}
