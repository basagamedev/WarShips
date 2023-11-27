using IA.Interface;
using Manager;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    #region STATE_FIELDS
    protected IState CurrentState { get; set; }
    public IState MovementState { get; protected set; }
    public IState AttackState { get; protected set; }
    #endregion

    #region STATE_MACHINE_METHODS
    public void SetStates(IState Movement, IState Attack)
    {
        MovementState = Movement;
        AttackState = Attack;
    }

    public void Initialize(IState startingState)
    {
        CurrentState = startingState;
        startingState.EnterState();
    }

    public void TransitionTo(IState nextState)
    {
        CurrentState = nextState;
        nextState.EnterState();
    }

    protected void Update()
    {
        if (GameManager.Instance.CheckMinigameFinished()) return;

        if (CurrentState != null)
        {
            CurrentState.UpdateState();
        }
    }

    protected void FixedUpdate()
    {
        if (GameManager.Instance.CheckMinigameFinished()) return;

        if (CurrentState != null)
        {
            CurrentState.FixedUpdateState();
        }
    }
    #endregion
}
