using UnityEngine;

public partial class FPSEnemyView : FPSEnemyViewBase
{
    public NavMeshAgent _NavAgent;
    public FPSPlayerViewBase _TargetPlayer;

    public override void Bind()
    {
        base.Bind();

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

    public override void Start()
    {
        base.Start();
        this._NavAgent.SetDestination(_TargetPlayer.transform.position);
        _started = true;
    }

    public bool _started = false;
    public void Update()
    {
        if (_TargetPlayer == null) return;
        if (_started == false) return;
        this._NavAgent.SetDestination(_TargetPlayer.transform.position);
        this.transform.LookAt(_TargetPlayer.transform);
        this.transform.Translate(this.transform.forward * Time.deltaTime * FPSEnemy.Speed);
    }

}