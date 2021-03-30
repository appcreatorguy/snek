using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameLoader
{
    public enum Scene
    {
        GameScene,
        Loading
    }
    private static Action loaderCallbackAction;

    public static void Load(Scene scene)
    {
        // Setup Action to be called when a single frame passes on the loading scene
        loaderCallbackAction = () =>
        {
            SceneManager.LoadScene(scene.ToString());
        };

        SceneManager.LoadScene(Scene.Loading.ToString());
    }

    public static void LoaderCallback()
    {
        if (loaderCallbackAction != null)
        {
            loaderCallbackAction();
            loaderCallbackAction = null;
        }
    }
}
