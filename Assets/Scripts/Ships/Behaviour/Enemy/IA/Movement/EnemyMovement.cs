using IA.Interface;
using Manager;
using Ship.Interface;
using UnityEngine;

namespace EnemyShip.State
{
    public class EnemyMovement : IState, IShipMovement
    {
        #region PRIVATE_FIELDS
        private ShipComponents components = null;
        private Transform playerTransform = null;
        private Vector2 currentVelocity = Vector2.zero;
        private float maxSpeed = 0;
        private float acceleration = 0;
        private float rotationSpeed = 0;
        private float distanceToAttack = 0;
        private float distanceFromPlayer = 0;
        private float preventOscillation = 0.1f;
        #endregion

        #region READONLY_FIELDS
        private readonly StateMachine stateMachine = null;
        private float maxDistance = 999;
        #endregion

        #region CAPSULE_FIELD
        public EnemyMovement(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }
        #endregion

        #region INTERFACE_METHODS
        public void EnterState()
        {
            if (playerTransform == null)
                playerTransform = GameManager.Instance.PlayerTransform;

            distanceFromPlayer = maxDistance;
            components.navMeshAgent.isStopped = false;
        }

        public void UpdateState()
        {
            DistanceFromPlayer();
            ShipRotate();
        }

        public void FixedUpdateState()
        {
            ShipMove();
        }

        public void SetMovementStats(ShipMovementStats shipMovementStats)
        {
            maxSpeed = shipMovementStats.MaxSpeed;
            acceleration = shipMovementStats.Acceleration;
            rotationSpeed = shipMovementStats.RotationSpeed;
        }

        public void ShipMove()
        {
            components.navMeshAgent.SetDestination(playerTransform.position);
            Vector2 newPosition = components.navMeshAgent.nextPosition + components.enemyTransform.up;
            Vector2 desiredVelocity = (newPosition - components.rigidBody2d.position).normalized * maxSpeed;
            currentVelocity = Vector2.Lerp(currentVelocity, desiredVelocity, acceleration * Time.fixedDeltaTime);
            components.rigidBody2d.MovePosition(components.rigidBody2d.position + currentVelocity * Time.fixedDeltaTime);
        }

        public void ShipRotate()
        {
            Vector3 newPosition = components.navMeshAgent.nextPosition;
            Vector3 movementDirection = (newPosition - components.enemyTransform.position).normalized;

            if (movementDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
                float angleDifference = Quaternion.Angle(components.enemyTransform.rotation, targetRotation);

                if (angleDifference > preventOscillation)
                {
                    float step = rotationSpeed * Time.deltaTime;
                    components.enemyTransform.rotation = Quaternion.RotateTowards(components.enemyTransform.rotation, targetRotation, step);
                }
            }
        }
        #endregion

        #region PRIVATE_METHODS
        private void DistanceFromPlayer()
        {
            distanceFromPlayer = Vector2.Distance(playerTransform.position, components.enemyTransform.position); ;

            if (distanceFromPlayer <= distanceToAttack)
            {
                stateMachine.TransitionTo(stateMachine.AttackState);
            }
        }
        #endregion

        #region PUBLIC_METHODS
        public void SetComponents(ShipComponents components)
        {
            this.components = components;
        }

        public void SetAttackStats(ShipAttackStats shipAttackStats)
        {
            distanceToAttack = shipAttackStats.ShipDistanceToAttack;
        }
        #endregion
    }
}