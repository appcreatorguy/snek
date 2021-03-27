using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour
{

    [SerializeField] private Snake snake;

    private LevelGrid levelGrid;
    void Start()
    {
        Debug.Log("GameHandler.Start()");

        levelGrid = new LevelGrid(20, 20);

        snake.Setup(levelGrid);
        levelGrid.Setup(snake);
    }
}
