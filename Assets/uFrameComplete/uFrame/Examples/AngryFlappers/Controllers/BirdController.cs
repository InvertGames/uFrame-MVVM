using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class BirdController : BirdControllerBase {
    
    public override void InitializeBird(BirdViewModel bird) {
    }

    public override void Flapped()
    {
        base.Flapped();
  
    }

    public override void Hit()
    {
        base.Hit();
        Bird.State = BirdState.Dead;
        AngryFlappersGameController.GameOver();
       
    }
}
