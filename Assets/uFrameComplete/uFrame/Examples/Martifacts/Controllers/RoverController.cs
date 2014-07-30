using System;
using System.Collections;
using System.Linq;
using UnityEngine;


public class RoverController : RoverControllerBase
{
    
    public override void InitializeRover(RoverViewModel rover)
    {
        
    }

    public override IEnumerator Drill()
    {
        if (Rover.State != RoverState.Idle) yield break;
        Rover.State = RoverState.Drilling;
        for (var i = 0; i < Rover.DrillFactor; i++)
        {
            yield return new WaitForSeconds(4f);
        }

        var game = MartifactsGameController.MartifactsGame;
        var artifactViewModel = Context["A" + game.CurrentTile.Identifier] as ArtifactViewModel;
        if (artifactViewModel != null)
        {
            Rover.CollectedArtifacts.Add(artifactViewModel);
            game.Artifacts.Add(artifactViewModel);
            MartifactsGameController.ArtifactPickedUp(artifactViewModel);
            Rover.Battery -= 1;
        }
       
        Rover.State = RoverState.Idle;
    }
    public override IEnumerator ShootFlare()
    {
        if (Rover.State != RoverState.Idle) yield break;
        Rover.State = RoverState.Firing;
        yield return new WaitForSeconds(Rover.SonarTime);
        Rover.State = RoverState.Idle;
        Rover.Battery -= 3;
    }

    public override IEnumerator Sonar()
    {
        return base.Sonar();
    }

    public override void MoveBackward()
    {
        base.MoveBackward();

        if (Rover.State != RoverState.Idle) return;

        if (MartifactsGameController.MartifactsGame.TileBack is RockyTileViewModel)
        {
            MartifactsGameController.MoveFailed();
            return;
        }

        Rover.TileY -= 1;
        Rover.State = RoverState.Moving;
        MartifactsGameController.RoverMoved(Rover);
        Rover.Battery -= 1;
    }

    public override void MoveForward()
    {
        base.MoveForward();
        if (Rover.State != RoverState.Idle) return;
        if (MartifactsGameController.MartifactsGame.TileFront is RockyTileViewModel)
        {
            MartifactsGameController.MoveFailed();
            return;
        }
        Rover.TileY += 1;
        Rover.State = RoverState.Moving;
        MartifactsGameController.RoverMoved(Rover);
        Rover.Battery -= 1;
    }
    public override void MoveLeft()
    {
        base.MoveLeft();
        if (Rover.State != RoverState.Idle) return;
        if (MartifactsGameController.MartifactsGame.TileLeft is RockyTileViewModel)
        {
            MartifactsGameController.MoveFailed();
            return;
        }
        Rover.TileX -= 1;
        Rover.State = RoverState.Moving;
        MartifactsGameController.RoverMoved(Rover);
        Rover.Battery -= 1;
    }

    public override void MoveRight()
    {
        base.MoveRight();
        if (Rover.State != RoverState.Idle) return;
        if (MartifactsGameController.MartifactsGame.TileRight is RockyTileViewModel)
        {
            MartifactsGameController.MoveFailed();
            return;
        }
        Rover.TileX += 1;
        Rover.State = RoverState.Moving;
        MartifactsGameController.RoverMoved(Rover);
        Rover.Battery -= 1;
    }

    public override void ReachedDestination()
    {
        base.ReachedDestination();

        this.Rover.State = RoverState.Idle;

    }
}
