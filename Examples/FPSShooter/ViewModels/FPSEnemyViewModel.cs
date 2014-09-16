using Invert.StateMachine;
using UnityEngine;

public partial class FPSEnemyViewModel : FPSDamageableViewModel
{
    public P<float> _DistanceToPlayerProperty;

    
}

public class FPSEnemyAIStateMachine : StateMachine
{
    public EnemyWaitForPlayerInRangeState WaitState { get; set; }
    public EnemyWaitForPlayerInRangeState AttackState { get; set; }

    public FPSEnemyAIStateMachineState CurrentFpsEnemyAiState
    {
        get { return CurrentState as FPSEnemyAIStateMachineState; }
    }
}

public abstract class FPSEnemyAIStateMachineState : State
{
    public virtual void DistanceToPlayerChanged(float f)
    {
        
    }
}
public class EnemyWaitForPlayerInRangeState : FPSEnemyAIStateMachineState
{
    public StateTransition PlayerInRange { get; set; }
    

    public override string Name
    {
        get { return "FPSEnemeyPlayerInSight"; }
    }

    public override void DistanceToPlayerChanged(float f)
    {
        base.DistanceToPlayerChanged(f);
        if (f < 10)
        {
            Transition(PlayerInRange);
        }
    }
}

public class EnemyAttackState : FPSEnemyAIStateMachineState
{
    private StateTransition _playerOutOfRange;

    public StateTransition PlayerOutOfRange
    {
        get { return _playerOutOfRange ; }
        set { _playerOutOfRange = value; }
    }

    public override string Name
    {
        get { return "EnemyAttackState"; }
    }

    public override void DistanceToPlayerChanged(float f)
    {
        base.DistanceToPlayerChanged(f);
        if (f > 10)
        {
            Transition(PlayerOutOfRange);
        }
    }
}