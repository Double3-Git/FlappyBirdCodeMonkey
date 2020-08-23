using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class SoundManager 
{
    public enum Sounds
    {
        BirdJump,
        Score,
        Loose,
        ButtonOver,
        ButtonClick,
    }
    public static void PlaySound(Sounds sound)
    {
        GameObject gameObject = new GameObject("Sound", typeof(AudioSource));
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();

        audioSource.PlayOneShot(GetAudioClip(sound));
        Object.Destroy(gameObject, 2);

    }

    private static AudioClip GetAudioClip(Sounds sound)
    {
        foreach (GameAssets.SoundAudioClip item in GameAssets.GetInstance().soundAudioClips)
        {
            if (item.sound == sound) return item.audioClip;
        }
        return null;
    }

    public static void AddButtonSound(this Button button)
    {
        button.onClick.AddListener(() => { PlaySound(Sounds.ButtonClick);  });
    }
}
