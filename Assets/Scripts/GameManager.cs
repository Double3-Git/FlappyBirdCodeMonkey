using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameManager.Start");

        int count = 0;
        FunctionPeriodic.Create(() =>
        {
            CMDebug.TextPopupMouse($"Ding! {count++}");
        }, .300f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
