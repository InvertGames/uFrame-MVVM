
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;


public partial class FPSMainMenuView {
    
    public override void Bind() {
        base.Bind();

    }

    public void OnGUI()
    {
        if (GUI.Button(new Rect((Screen.width/2f) - 75, (Screen.height/2f) - 15, 150, 30), "Play Game"))
        {
            ExecutePlay();
        }
    }
}