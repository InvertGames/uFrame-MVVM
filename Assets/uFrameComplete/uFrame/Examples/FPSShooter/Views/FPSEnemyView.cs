using UnityEngine;

public partial class FPSEnemyView : FPSEnemyViewBase
{
    public NavMeshAgent _NavAgent;
    public FPSPlayerViewBase _TargetPlayer;

    public override void Bind()
    {
        // Add bindings here or add binding components in unity.
        this.BindProperty(() => FPSEnemy._StateProperty, state => gameObject.SetActive(state != FPSPlayerState.Dead));
        this.BindProperty(() => FPSEnemy._HealthProperty,
            v => gameObject.renderer.material.SetColor("_Color", new Color(1.0f, 1.0f - (1.0f - v), 1.0f - (1.0f - v))));

        //this.BindCollision(() => FPSEnemy.BulletHit, CollisionEventType.OnCollisionEnter)
        //    .When(g => g.IsView<FPSBulletView>())
        //    .SetParameterSelector((go) => go.GetViewModel<FPSBulletViewModel>())
        //    .Subscribe(() => Debug.Log("Was hit!!!"))
        //    ;
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