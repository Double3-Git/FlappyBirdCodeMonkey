using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Score
{
    public static void Start()
    {
        Bird.GetInstance().OnDied += score_OnDied;
    }

    private static void score_OnDied(object sender, System.EventArgs e)
    {
        TrySetNewHighsore(Level.GetInstance().GetPipesPassedCount());
    }

    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt("highscore");
    }

    public static bool TrySetNewHighsore(int score)
    {
        int highScore = GetHighScore();
        if (highScore >= score) return false;

        PlayerPrefs.SetInt("highscore", score);
        PlayerPrefs.Save();
        return true; 
    }

    public static void ResetHighscore()
    {
        PlayerPrefs.SetInt("highscore", 0);
        PlayerPrefs.Save();
    }
}
