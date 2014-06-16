using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public partial class FPSGameView
{
    public Transform _SpawnPointsParent;

    public override void Bind()
    {
        base.Bind();

        //if (_PlayerView == null) // Create the player if he doesn't exist.  Resources/FPSPlayer.prefab
        //    _PlayerView = InstantiateView(FPSGame.CurrentPlayer) as FPSPlayerView;
        //else
        //    _PlayerView.FPSPlayer = FPSGame.CurrentPlayer;

        //// Add bindings here or add binding components in unity.
        //this.BindProperty(() => FPSGame._StateProperty, state => gameObject.SetActive(state != FPSGameState.Paused));
        //// Bind to a ViewModel collection, instantiating views when the model collection is changed
        //this.BindToViewCollection(() => FPSGame._EnemiesProperty, _Enemies)
        //    .SetAddHandler((enemyView) =>
        //    {
        //        var enemy = ((FPSEnemyView)enemyView);
        //        enemy._TargetPlayer = _PlayerView;
        //        enemy.transform.position = GetRandomSpawnPoint().position;
        //        _Enemies.Add(enemyView);
        //    })
        //    .SetParent(_EnemiesTransform);
    }

    public override void CurrentPlayerChanged(FPSPlayerViewModel value)
    {
        base.CurrentPlayerChanged(value);
        //_CurrentPlayer.FPSPlayer = FPSGame.CurrentPlayer;

    }

    public override ViewBase CreateEnemiesView(FPSEnemyViewModel fPSEnemy)
    {
        return InstantiateView(fPSEnemy);
    }

    public override void StateChanged(FPSGameState value)
    {
        base.StateChanged(value);
        gameObject.SetActive(value != FPSGameState.Paused);
    }

    public override void EnemiesAdded(FPSEnemyViewBase enemy)
    {
        base.EnemiesAdded(enemy);
        ((FPSEnemyView) enemy)._TargetPlayer = (FPSPlayerView)_CurrentPlayer;
        enemy.transform.position = GetRandomSpawnPoint().position;

    }

    public Transform GetRandomSpawnPoint()
    {
        return _SpawnPointsParent.GetChild(Random.Range(0, _SpawnPointsParent.transform.childCount - 1));
    }
}