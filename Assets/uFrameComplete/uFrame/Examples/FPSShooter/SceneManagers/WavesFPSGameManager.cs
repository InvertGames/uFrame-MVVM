
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class WavesFPSGameManager : WavesFPSGameManagerBase
{
    public TextAsset _SceneState;
    public override IEnumerator Load(UpdateProgressDelegate progress)
    {
 
        // This is for the future scene serialization :) But not implemented entirely yet.
        for (int index = 0; index < RootViews.Count; index++)
        {
            var persistableView = RootViews[index];
            progress("Loading " + persistableView.name, (1f / RootViews.Count) * (index + 1));
        }
        yield break;
    }

    public override void OnLoaded()
    {
        base.OnLoaded();
        StartCoroutine(WavesFPSGameController.StartGame());
    }

    public override void Setup()
    {
        if (_SceneState != null)
        {
            Context.Load(new StringSerializerStorage() { Result = _SceneState.text }, new JsonStream() {UseReferences = true});
        }
        base.Setup();
        
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            var stringStorage = new StringSerializerStorage();
            Context.Save(stringStorage,new JsonStream());
            Debug.Log(stringStorage);
        }
    }
}