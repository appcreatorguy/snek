using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    private bool firstUpdate = true;

    private void Update()
    {
        if (firstUpdate)
        {
            firstUpdate = false;
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        GameLoader.LoaderCallback();
    }
}
