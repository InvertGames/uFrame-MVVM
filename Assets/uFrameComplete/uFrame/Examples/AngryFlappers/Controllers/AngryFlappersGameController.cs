using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class AngryFlappersGameController : AngryFlappersGameControllerBase {

    [Inject("PlayGame")]
    public ICommand PlayGameCommand { get; set; }

    public override void InitializeAngryFlappersGame(AngryFlappersGameViewModel angryFlappersGame)
    {
        angryFlappersGame.Play = PlayGameCommand;
    }


}
