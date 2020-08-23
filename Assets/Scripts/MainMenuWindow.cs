﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuWindow : MonoBehaviour
{

    // Метод Awake вызывается во время загрузки экземпляра сценария
    private void Awake()
    {
        GameObject.Find("ButtonPlay").GetComponent<Button>().onClick.AddListener(() => { Loader.Load(Loader.Scene.GameScene); });
        
    }

}
 