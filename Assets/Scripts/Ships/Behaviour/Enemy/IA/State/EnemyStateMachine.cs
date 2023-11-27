
namespace EnemyShip.State
{
    public class EnemyStateMachine : StateMachine 
    {
        #region UNITY_METHODS
        private void Start()
        {
            Initialize(MovementState);
        }
        #endregion
    }
}
