using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAssignedSoundOnTrigger : MonoBehaviour
{
    public AudioClip assignedClip;  // o box collidera atad���m�z ses
    private AudioSource playerAudioSource;
    private AudioClip originalClip;

    void Start()
    {
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerAudioSource = player.GetComponent<AudioSource>();

        //e�er daha �nce ses varsa onu bi tutuyoruz bi yerde
        if (playerAudioSource != null)
        {
            originalClip = playerAudioSource.clip;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && playerAudioSource != null)
        {
            playerAudioSource.clip = assignedClip; // atanm�s sesi  calan ses yap�yoruz
            playerAudioSource.Play(); 
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && playerAudioSource != null)
        {
            playerAudioSource.clip = originalClip; // ilk sesimize geri d�n�yoruz
            playerAudioSource.Play(); // C�k�nca ilk sesimizi cal�yoruz
        }
    }
}

