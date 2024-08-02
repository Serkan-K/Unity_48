using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAssignedSoundOnTrigger : MonoBehaviour
{
    public AudioClip assignedClip;  // o box collidera atadýðýmýz ses
    private AudioSource playerAudioSource;
    private AudioClip originalClip;

    void Start()
    {
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerAudioSource = player.GetComponent<AudioSource>();

        //eðer daha önce ses varsa onu bi tutuyoruz bi yerde
        if (playerAudioSource != null)
        {
            originalClip = playerAudioSource.clip;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && playerAudioSource != null)
        {
            playerAudioSource.clip = assignedClip; // atanmýs sesi  calan ses yapýyoruz
            playerAudioSource.Play(); 
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && playerAudioSource != null)
        {
            playerAudioSource.clip = originalClip; // ilk sesimize geri dönüyoruz
            playerAudioSource.Play(); // Cýkýnca ilk sesimizi calýyoruz
        }
    }
}

