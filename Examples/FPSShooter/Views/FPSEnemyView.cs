using System;
using UnityEngine;
using UniRx;
public partial class FPSEnemyView
{
    public NavMeshAgent _NavAgent;
    private IDisposable disposable;
    public override void Bind()
    {
        base.Bind();
        //disposable = FPSEnemy.ParentFPSGame.CurrentPlayer._PositionProperty.Subscribe(_ =>
        //{
        //    _NavAgent.SetDestination(this.transform.position);
        //    transform.LookAt(_);
        //});

        //UpdateAsObservable().Subscribe(_ => transform.Translate(this.transform.forward*Time.deltaTime*FPSEnemy.Speed));
    }

    public override void HealthChanged(float value)
    {
        base.HealthChanged(value);
        gameObject.renderer.material.SetColor("_Color", new Color(1.0f, 1.0f - (1.0f - value), 1.0f - (1.0f - value)));
    }
    public override void StateChanged(FPSPlayerState value)
    {
        
        gameObject.SetActive(value != FPSPlayerState.Dead);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        disposable.Dispose();
    }
}