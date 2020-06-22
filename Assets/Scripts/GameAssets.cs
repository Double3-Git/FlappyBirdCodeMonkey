using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite pipeHeadSprite;

    private static GameAssets instance;

    public static GameAssets GetInstance() => instance;

    private void Awake()
    {
        instance = this;
    }


}
