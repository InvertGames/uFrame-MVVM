using System;
using System.Collections;
using System.Linq;
using UnityEngine;


public partial class GameRootView {
    
    public override void Bind() {
        base.Bind();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteMainMenu();
        }
    }
    public void OnGUI()
    {
        GUI.Label(new Rect(15f,15f,400f,400f), "Press 'Enter' to return to menu." );
    }
}
