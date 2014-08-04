using UnityEngine;

public partial class AngryFlappersGameView
{
    public Transform _GameOverDisplay;

    public Transform _HudDisplay;

    public Transform _MenuDisplay;

    public GUIText _ScoreLabel;
    public GUIText _GameOverScoreLabel;

    public override ViewBase CreatePipesView(PipeViewModel value)
    {
        return base.CreatePipesView(value);
    }

    public override void ScoreChanged(int value)
    {
        _ScoreLabel.text = string.Format("Score: {0}", value.ToString());
        _GameOverScoreLabel.text = string.Format("Score: {0}", value.ToString());
    }

    public override void StateChanged(AngryFlappersGameState value)
    {
        if (_MenuDisplay != null)
            _MenuDisplay.gameObject.SetActive(value == AngryFlappersGameState.Menu || value == AngryFlappersGameState.GameOver);

        if (_GameOverDisplay != null)
            _GameOverDisplay.gameObject.SetActive(value == AngryFlappersGameState.GameOver);

        if (_HudDisplay != null)
            _HudDisplay.gameObject.SetActive(value == AngryFlappersGameState.Playing);
    }

    public void Update()
    {
        if (AngryFlappersGame.State == AngryFlappersGameState.Menu ||
            AngryFlappersGame.State == AngryFlappersGameState.GameOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ExecutePlay();
            }
        }
    }
}