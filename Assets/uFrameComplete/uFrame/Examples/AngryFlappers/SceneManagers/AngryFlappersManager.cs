using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AngryFlappersManager : AngryFlappersManagerBase {

    [Inject]
    public AngryFlappersGameViewModel Game { get; set; }

    public override System.Collections.IEnumerator Load(UpdateProgressDelegate progress) {
        // Use the controllers to create the game.
        yield break;
    }

    public override void Setup() {

        Container.RegisterInstance<ICommand>(new Command(() => StartCoroutine(SpawnPipes())), "PlayGame");
        base.Setup();
      
    }

    public IEnumerator SpawnPipes()
    {
        Game.Pipes.Clear();
        Game.Bird.State = BirdState.Idle;
        Game.Bird.State = BirdState.Alive;
        Game.State = AngryFlappersGameState.Playing;

        while (Game.State == AngryFlappersGameState.Playing)
        {
            Game.Pipes.Add(new PipeViewModel(PipeController) { ScrollSpeed = Game.ScrollSpeed });
            yield return new WaitForSeconds(Game.PipeSpawnSpeed);
        }
       
       
    }
}