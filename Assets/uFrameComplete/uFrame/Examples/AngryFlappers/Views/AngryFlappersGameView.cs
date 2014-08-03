using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public partial class AngryFlappersGameView { 
    public override void ScoreChanged(int value) {
    }
    public override void PipesAdded(PipeViewBase item) {
    }
    public override void PipesRemoved(PipeViewBase item) {

    }
    public override ViewBase CreatePipesView(PipeViewModel value)
    {
        return base.CreatePipesView(value);
    }
 
    public override void StateChanged(AngryFlappersGameState value) {
        if (value == AngryFlappersGameState.GameOver)
        {
            
        }
    }

}
