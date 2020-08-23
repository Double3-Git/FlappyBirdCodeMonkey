using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour
{
    private Text scoreText;

    // Метод Awake вызывается во время загрузки экземпляра сценария
    private void Awake()
    {
        scoreText = transform.Find("ScoreText").GetComponent<Text>();
        transform.Find("RetryButton").GetComponent<Button>().onClick.AddListener(() => 
                                            { Loader.Load(Loader.Scene.GameScene); });
        transform.Find("RetryButton").GetComponent<Button>().AddButtonSound();

        transform.Find("MainMenuButton").GetComponent<Button>().onClick.AddListener(() =>
                                           { Loader.Load(Loader.Scene.MainMenu); });
        transform.Find("MainMenuButton").GetComponent<Button>().AddButtonSound();

    }

    // Метод Start вызывается перед первым вызовом какого-либо метода Update
    private void Start()
    {
        Bird.GetInstance().OnDied += Bird_OnDied;
        Hide();
    }

    private void Bird_OnDied(object sender, System.EventArgs e)
    {
        scoreText.text = $"Score: {Level.GetInstance().GetPipesPassedCount()}";
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

}
