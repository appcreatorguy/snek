using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWindow : MonoBehaviour
{

    public TMPro.TMP_Text scoreText;

    private void Update()
    {
        scoreText.text = GameHandler.GetScore().ToString();
    }
}
