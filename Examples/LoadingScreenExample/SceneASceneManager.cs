using System.Collections;
using UnityEngine;

public class SceneASceneManager : SceneManager
{
    public override IEnumerator Load(UpdateProgressDelegate progress)
    {
        progress("Scene A Controller - Step 1", 0.2f);
        yield return new WaitForSeconds(1f);
        progress("Scene A Controller - Step 2", 0.4f);
        yield return new WaitForSeconds(1f);
        progress("Scene A Controller - Step 3", 0.6f);
        yield return new WaitForSeconds(1f);
        progress("Scene A Controller - Step 4", 0.8f);
        yield return new WaitForSeconds(1f);
        progress("Scene A Controller - Step 5", 1f);
    }

    public void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 100), "Press the 'S' Key to switch levels.");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetMouseButtonDown(0))
        {
            GameManager.TransitionLevel<SceneBSceneManager>(
                (sceneBGame)=>{ /* Initialize the game if needed. */ }, 
                "SceneB" // The Unity Level Name
                );
        }
    }
}