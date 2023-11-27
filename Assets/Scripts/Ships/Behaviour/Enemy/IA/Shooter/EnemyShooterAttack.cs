using UnityEngine;
using IA.Interface;
using Ship.Attack;
using PoolSystem;
using Constants;
using Manager;

namespace EnemyShip.State
{
    public class EnemyShooterAttack : IState
    {
        #region READONLY_FIELDS
        private readonly StateMachine stateMachine = null;
        #endregion

        #region PRIVATE_FIELDS
        private ShipComponents components = null;
        private Animator cannonAnimator = null;
        private Transform playerTransform = null;
        private Quaternion targetCannonRotation = Quaternion.identity;
        Vector2 currentVelocity = Vector2.zero;
        private float delayToShoot = 0;
        private float waitToShoot = 0;
        private float deceleration = 0;
        private float rotationCannonSpeed = 0;
        private float distanceToMove = 0;
        private float distanceFromPlayer = 0;
        private float oscillationShoot = 0;
        private float cannonBallSpeed = 0;
        private readonly float maxDistance = 999;
        #endregion

        #region CAPSULE
        public EnemyShooterAttack(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }
        #endregion

        #region STATE_METHODS
        public void EnterState()
        {
            components.navMeshAgent.isStopped = true;

            if (cannonAnimator == null)
                cannonAnimator = components.cannonTransform.GetComponentInChildren<Animator>();

            if (playerTransform == null)
                playerTransform = GameManager.Instance.PlayerTransform;

            currentVelocity = components.enemyTransform.up;
            distanceFromPlayer = maxDistance;
            delayToShoot = 0;
        }

        public void UpdateState()
        {
            DistanceFromPlayer();
            RotateCannon();
            Attack();
        }

        public void FixedUpdateState()
        {
            StopShip();
        }
        #endregion

        #region PUBLIC_METHODS
        public void SetComponents(ShipComponents components)
        {
            this.components = components;
        }

        public void SetMovementStats(ShipCannonStats shipCannonStats, ShipMovementStats shipMovementStats, ShipCannonBallStats shipCannonBallStats, ShipAttackStats shipAttack)
        {
            deceleration = shipMovementStats.Deceleration;
            rotationCannonSpeed = shipCannonStats.ShipCannonRotationSpeed;
            cannonBallSpeed = shipCannonBallStats.ShipCannonBallSpeed;
            waitToShoot = shipAttack.ShipDelayToAttack;
            distanceToMove = shipAttack.ShipDistanceStopAttack;
            oscillationShoot = shipAttack.ShipOscillationToAttack;
        }
        #endregion

        #region PRIVATE_METHODS
        private void RotateCannon()
        {
            Vector3 direction = playerTransform.position - components.cannonTransform.position;
            direction.z = 0;
            targetCannonRotation = Quaternion.LookRotation(Vector3.forward, direction);
            components.cannonTransform.rotation = Quaternion.Lerp(components.cannonTransform.rotation, targetCannonRotation, Time.deltaTime * rotationCannonSpeed);
        }

        private void Attack()
        {
            if(CanAttack())
            {
                delayToShoot = 0;
                CannonBall pooledCannonBall = GenericObjectPool.Instance.GetPooledObject<CannonBall>();
                pooledCannonBall.SetCannonBall(components.cannonTransform, cannonBallSpeed, ConstantStrings.PlayerTag);
                cannonAnimator.CrossFade(ConstantStrings.ShootCannonAnimation, 0);
            }
        }

        private bool CanAttack()
        {
            float angleDifference = Quaternion.Angle(components.enemyTransform.rotation, targetCannonRotation);
            delayToShoot += Time.deltaTime;

            return angleDifference > oscillationShoot && delayToShoot > waitToShoot;
        }

        private void StopShip()
        {
            currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            components.rigidBody2d.MovePosition(components.rigidBody2d.position + currentVelocity * Time.fixedDeltaTime);
        }

        private void DistanceFromPlayer()
        {
            distanceFromPlayer = Vector2.Distance(playerTransform.position, components.enemyTransform.position);

            if (distanceFromPlayer >= distanceToMove)
            {
                stateMachine.TransitionTo(stateMachine.MovementState);
            }
        }
        #endregion
    }
}
