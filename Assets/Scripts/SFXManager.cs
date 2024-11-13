using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioClip playerShoot;
    public AudioClip asteroidExplosion;
    public AudioClip playerDamage;
    public AudioClip deathSound;
    public AudioClip bgMusic;   

    private AudioSource SFXaudioSource;

    public AudioSource BgMusicAudioSource;  

    public void Awake()
    {
        SFXaudioSource = GetComponent<AudioSource>();
        BgMusicAudioSource = gameObject.transform.Find("BgMusic").gameObject.GetComponent<AudioSource>();                          
    }



    //called in the PlayerController Script
    public void PlayerShoot()
    {
        SFXaudioSource.PlayOneShot(playerShoot);
    }

    //called in the PlayerController Script
    public void PlayerDamage()
    {
        SFXaudioSource.PlayOneShot(playerDamage);
    }

    public void PlayerDeathSound()
    {
        SFXaudioSource.PlayOneShot(deathSound);
    }
    
    //called in the PlayerController Script
    

    //called in the AsteroidDestroy script
    public void AsteroidExplosion()
    {
        SFXaudioSource.PlayOneShot(asteroidExplosion);
    }

    
    public void BGMusic()
    {
        BgMusicAudioSource.clip = bgMusic;
        BgMusicAudioSource.Play();
    }    
}
