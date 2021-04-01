using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverWindow : MonoBehaviour
{
    private static GameOverWindow overWindow;
    public Button_UI gameOverButton;
    private void Awake()
    {
        overWindow = this;

        gameOverButton.ClickFunc = () =>
        {
            GameLoader.Load(GameLoader.Scene.GameScene);
        };

        Hide();
    }
    private void Reveal()
    {
        gameObject.SetActive(true);
    }
    
    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public static void Show()
    {
        overWindow.Reveal();
    }
}
