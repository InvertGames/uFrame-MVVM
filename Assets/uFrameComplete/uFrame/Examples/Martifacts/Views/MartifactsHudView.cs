using System;
using System.Collections;
using System.Linq;
using UnityEngine;


public partial class MartifactsHudView
{
    public dfPanel _MessagePanel;
    public dfLabel _MessageTileLabel;
    public dfLabel _MessageLabel;

    public float _MessageDisplayTime = 3f;
    public dfLabel _EtThenRockCountLabel;
    public dfLabel _BedRockCountLabel;
    public dfLabel _PinacleRockCountLabel;
    public dfLabel _MysteryRockCountLabel;

    public dfPanel _GameOverPanel;
    public dfPanel _GameWonPanel;


    public dfLabel _MovesLabel;
    public dfLabel _ArtifactsLabel;
    public dfLabel _BatteryLeftLabel;
    public dfLabel _ScoreLabel;
    public dfLabel _MainScoreLabel;
    //public dfLabel _MysteryRockCountLabel;
    public override void Bind()
    {
        base.Bind();

    }

    public override void StateChanged(MartifactsGameState value)
    {
        base.StateChanged(value);

        _GameWonPanel.IsVisible = MartifactsGame.State == MartifactsGameState.Complete;
        _GameOverPanel.IsVisible = MartifactsGame.State == MartifactsGameState.GameOver;

    }

    public override void MessageChanged(string value)
    {
        base.MessageChanged(value);
        _MessageLabel.Text = value;
        StartCoroutine(ShowForSeconds());
    }

    public override void MessageTitleChanged(string value)
    {
        base.MessageTitleChanged(value);
        _MessageTileLabel.Text = value;

    }

    public IEnumerator ShowForSeconds()
    {
        _MessagePanel.Show();
        yield return new WaitForSeconds(_MessageDisplayTime);
        _MessagePanel.Hide();
    }
    public void OnClick(dfControl control, dfMouseEventArgs mouseEvent)
    {
        control.IsVisible = false;
    }

    public void Update()
    {
        if (!IsBound) return;
        _BedRockCountLabel.Text = string.Format("Found: {0}", MartifactsGame.Rover.CollectedArtifacts.Count(p => p.Type == ArtifactType.BedRock));
        _EtThenRockCountLabel.Text = string.Format("Found: {0}", MartifactsGame.Rover.CollectedArtifacts.Count(p => p.Type == ArtifactType.EtThenRock));
        _PinacleRockCountLabel.Text = string.Format("Found: {0}", MartifactsGame.Rover.CollectedArtifacts.Count(p => p.Type == ArtifactType.PinnacleRock));
        _MysteryRockCountLabel.Text = string.Format("Found: {0}", MartifactsGame.Rover.CollectedArtifacts.Count(p => p.Type == ArtifactType.MysteryRock));
        _ArtifactsLabel.Text = string.Format("Artifacts Found: {0}", MartifactsGame.Rover.CollectedArtifacts.Count);
        _MovesLabel.Text = string.Format("Moves: {0}", MartifactsGame.MoveCount);
        _BatteryLeftLabel.Text = string.Format("Battery Life Remaining: {0}", MartifactsGame.Rover.Battery);
        _ScoreLabel.Text = string.Format("Score: Artifacts * Battery Life = {0}", MartifactsGame.Rover.CollectedArtifacts.Count * MartifactsGame.Rover.Battery);
        _MainScoreLabel.Text = string.Format("Score: {0} Moves: {1}", MartifactsGame.Rover.CollectedArtifacts.Count * MartifactsGame.Rover.Battery, MartifactsGame.MoveCount);
    }
}
