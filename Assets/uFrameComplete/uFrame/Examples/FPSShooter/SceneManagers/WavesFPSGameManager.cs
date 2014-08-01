
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
       
        yield break;
    }

    public override void OnLoaded()
    {
        base.OnLoaded();
        StartCoroutine(WavesFPSGameController.StartGame());
    }

    public override void Setup()
    {
    
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