using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using UnityEngine;


public class MartifactsGameController : MartifactsGameControllerBase
{

    public override void InitializeMartifactsGame(MartifactsGameViewModel martifactsGame)
    {
    }

    public void RoverMoved(RoverViewModel rover)
    {
        var m = MartifactsGame;
        m.MoveCount++;
        m.TileBackLeft = Context[TilePositionIdentifier(-1, -1)] as TileViewModel;
        m.TileBackRight = Context[TilePositionIdentifier(1, -1)] as TileViewModel;
        m.TileBack = Context[TilePositionIdentifier(0, -1)] as TileViewModel;
        m.TileLeft = Context[TilePositionIdentifier(-1, 0)] as TileViewModel;
        m.TileRight = Context[TilePositionIdentifier(1, 0)] as TileViewModel;
        m.TileFront = Context[TilePositionIdentifier(0, 1)] as TileViewModel;
        m.TileFrontLeft = Context[TilePositionIdentifier(-1, 1)] as TileViewModel;
        m.TileFrontRight = Context[TilePositionIdentifier(1, 1)] as TileViewModel;
        m.CurrentTile = Context[TilePositionIdentifier(0, 0)] as TileViewModel;

        if (m.Rover != null)
        {
            if (m.Rover.Battery <= 0)
            {
                m.State = MartifactsGameState.GameOver;
            }
            if (m.CurrentTile == m.CompleteTile)
            {
                m.State = MartifactsGameState.Complete;
            }

        }


    }

    public string TilePositionIdentifier(int x, int y)
    {
        if (MartifactsGame.Rover == null)
        {
            return x.ToString() + y.ToString();
        }
        return (MartifactsGame.Rover.TileX + x).ToString() + (MartifactsGame.Rover.TileY + y).ToString();
    }

    public bool _ArtifaceMessageToggle;
    public void ArtifactPickedUp(ArtifactViewModel artifactViewModel)
    {
        MartifactsGame.Artifacts.Remove(artifactViewModel);

        if (!_ArtifaceMessageToggle)
        {
            MartifactsGame.MessageTitle = "Found an Artifact!";
            MartifactsGame.Message = string.Format("Found {1}. Extending the battery life by +{0}", ((int)artifactViewModel.Type + 1) * 2, artifactViewModel.Type.ToString());
            MartifactsGame.Rover.Battery += ((int)artifactViewModel.Type + 1) * 2;
        }
        else
        {
            MartifactsGame.MessageTitle = "You found another artifact.";
            MartifactsGame.Message = string.Format("Found {1}. Extending battery life by +{0}", ((int)artifactViewModel.Type + 1) * 2, artifactViewModel.Type.ToString());
            MartifactsGame.Rover.Battery += ((int)artifactViewModel.Type + 1) * 2;
        }

        _ArtifaceMessageToggle = !_ArtifaceMessageToggle;
        if (!MartifactsGame.Artifacts.Any())
        {
            MartifactsGame.State = MartifactsGameState.GameOver;
        }

    }

    public bool _MessageToggle;
    public void MoveFailed()
    {
        if (!_MessageToggle)
        {
            MartifactsGame.MessageTitle = "Rover Says NO!!!!";
            MartifactsGame.Message = "I can't move in that direction.  I won't make it out alive :(";
        }
        else
        {
            MartifactsGame.MessageTitle = "Don't push me!";
            MartifactsGame.Message = "I can't help what I can't do.";
        }
        _MessageToggle = !_MessageToggle;
    }

    public override void Retry()
    {
        base.Retry();
        Application.LoadLevel(Application.loadedLevelName);
    }
}
