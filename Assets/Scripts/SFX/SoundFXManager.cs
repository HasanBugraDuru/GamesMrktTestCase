using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;
    [SerializeField] private AudioSource SoundObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audio,Transform spawnTransform,float valume)
    {
        //spawn in gameobject
        AudioSource audioSource = Instantiate(SoundObject,spawnTransform.position,Quaternion.identity);

        //assing the audioClip
        audioSource.clip = audio;

        //assing the valume
        audioSource.volume = valume;

        //play yhe sound
        audioSource.Play();

        // get the lenght of the clip
        float cliplenght = audioSource.clip.length;

        //destroy
        Destroy(audioSource.gameObject,cliplenght);
    }
}
