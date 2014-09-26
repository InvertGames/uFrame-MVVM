using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public partial class FPSGameView
{ 


    public override void EnemiesAdded(ViewBase viewBase)
    {
        base.EnemiesAdded(viewBase);
        viewBase.transform.position = GetRandomSpawnPoint().position;
    }

    public override void EnemiesRemoved(ViewBase viewBase)
    {
        base.EnemiesRemoved(viewBase);
    }

    public Transform _SpawnPointsParent;

    public override void Bind()
    {
        base.Bind();
        
    }


    public override ViewBase CreateEnemiesView(FPSEnemyViewModel fPSEnemy)
    {
        return InstantiateView(fPSEnemy);
    }

    //public override void StateChanged(FPSGameState value)
    //{
    //    base.StateChanged(value); 
    //    gameObject.SetActive(value != FPSGameState.Paused);
    //}

    public Transform GetRandomSpawnPoint()
    {
        return _SpawnPointsParent.GetChild(Random.Range(0, _SpawnPointsParent.transform.childCount - 1));
    }
}