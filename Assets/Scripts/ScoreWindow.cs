using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWindow : MonoBehaviour
{

    private Text highScoreText;
    private Text scoreText;

    // Метод Awake вызывается во время загрузки экземпляра сценария
    private void Awake()
    {
        scoreText = transform.Find("ScoreText").GetComponent<Text>();
        highScoreText = transform.Find("HighScoreText").GetComponent<Text>();
    }

    private void Start()
    {
        highScoreText.text = $"HighScore: {Score.GetHighScore()}";
    }


    // Метод Update вызывается на каждом кадре, если класс MonoBehaviour включен
    private void Update()
    {
        scoreText.text = $"Score: {Level.GetInstance().GetPipesPassedCount()}";
    }



}
