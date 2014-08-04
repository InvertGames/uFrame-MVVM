using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class AngryFlappersGameController : AngryFlappersGameControllerBase {

    [Inject("PlayGame")]
    public ICommand PlayGameCommand { get; set; }

    [Inject("EndGame")]
    public ICommand EndGameCommand { get; set; }

    public override void InitializeAngryFlappersGame(AngryFlappersGameViewModel angryFlappersGame)
    {
        AngryFlappersGame.Play = PlayGameCommand;
        AngryFlappersGame.GameOver = EndGameCommand;
    }

    public override void GameOver()
    {
        base.GameOver();

        AngryFlappersGame.State = AngryFlappersGameState.GameOver;
        StopAllCoroutines();

    }
}
