using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite pipeHeadSprite;

    [Header("Prefabs")]
    public Transform pfPipeHead;
    public Transform pfPipeBody;

    [Header("Sounds")]
    public SoundAudioClip[] soundAudioClips;

    private static GameAssets instance;

    public static GameAssets GetInstance() => instance;

    [Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sounds sound;
        public AudioClip audioClip;
    }
    private void Awake()
    {
        instance = this;
    }


}
