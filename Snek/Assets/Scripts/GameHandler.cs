using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour
{

    private static GameHandler instance;

    private static int score;

    [SerializeField] private Snake snake;

    private LevelGrid levelGrid;

    private void Awake()
    {
        instance = this;
        InitializeStaticFields();
    }

    void Start()
    {
        Debug.Log("GameHandler.Start()");

        levelGrid = new LevelGrid(20, 20);

        snake.Setup(levelGrid);
        levelGrid.Setup(snake);

        CMDebug.ButtonUI(new Vector2(250, 335), "Replay Game", () =>
          {
              GameLoader.Load(GameLoader.Scene.GameScene);
          });
    }

    private static void InitializeStaticFields()
    {
        score = 0;
    }

    public static int GetScore()
    {
        return score;
    }

    public static void AddScore(int amount = 100)
    {
        score += amount;
    }
}
