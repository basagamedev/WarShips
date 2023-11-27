using UnityEngine;
using IA.Interface;
using Constants;
using Ship.Damageable;
using Manager;

namespace EnemyShip.State
{
    public class EnemyChaseAttack : IState
    {
        #region PRIVATE_FIELDS
        private readonly EnemyStateMachine stateMachine = null;
        private ShipComponents components = null;
        private Transform playerTransform = null;
        private ContactFilter2D contactFilter;
        private Quaternion targetShipRotation = Quaternion.identity;
        private float delayToAttack = 0;
        private float waitToAttack = 0;
        private float distanceToMove = 0;
        private float distanceFromPlayer = 0;
        private float oscillationAttack = 0;
        private float shipRotation = 0;
        private float delayToAttackAgain = 0;
        private float impulseForce = 10;
        private float minSpeedToDamage = 0.5f;
        private bool rotateReady = false;
        private bool rushAttacked = false;
        private bool shipCollided = false;
        private readonly int collisionDetection = 5;
        private readonly float maxDistance = 999;
        #endregion

        #region CAPSULE_FIELD
        public EnemyChaseAttack(EnemyStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }
        #endregion

        #region STATE_METHODS
        public void EnterState()
        {
            components.navMeshAgent.isStopped = true;
            distanceFromPlayer = maxDistance;
            SetInitialComponents();
            ResetVariables();
            StopShip();
        }

        public void UpdateState()
        {
            if (shipCollided) return;

            DistanceFromPlayer();
            RotateForAttack();

            if(rotateReady)
                CheckCanRotateAndAttackAgain();

            if (rushAttacked)
                CheckCollision();
        }

        public void FixedUpdateState()
        {
            if (shipCollided) return;

            if (rotateReady && CanAttack() && !rushAttacked)
                RushAttack();
        }
        #endregion

        #region PUBLIC_METHODS
        public void SetComponents(ShipComponents components)
        {
            this.components = components;
        }

        public void SetMovementStats(ShipMovementStats shipMovementStats, ShipAttackStats shipAttackStats)
        {
            waitToAttack = shipAttackStats.ShipDelayToAttack;
            distanceToMove = shipAttackStats.ShipDistanceStopAttack;
            oscillationAttack = shipAttackStats.ShipOscillationToAttack;
            shipRotation = shipMovementStats.RotationSpeed;
        }
        #endregion

        #region PRIVATE_METHODS
        private void RotateForAttack()
        {
            if (rotateReady) return;

            if (!IsAttackAngleReady())
            {
                RotateShip();
            }
            else 
            {
                rotateReady = true;
            }
        }

        private void RotateShip()
        {
            Vector3 movementDirection = (playerTransform.position - components.enemyTransform.position).normalized;

            if (movementDirection != Vector3.zero)
            {
                targetShipRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
                float step = shipRotation * Time.deltaTime;
                components.enemyTransform.rotation = Quaternion.RotateTowards(components.enemyTransform.rotation, targetShipRotation, step);
            }
        }

        private bool IsAttackAngleReady()
        {
            float angleDifference = Quaternion.Angle(components.enemyTransform.rotation, targetShipRotation);
            return angleDifference < oscillationAttack;
        }

        private bool CanAttack()
        {
            delayToAttack += Time.deltaTime;

            return delayToAttack > waitToAttack;
        }

        private void RushAttack()
        {
            rushAttacked = true;
            components.rigidBody2d.AddForce(components.enemyTransform.up * impulseForce, ForceMode2D.Impulse);
        }

        private void CheckCanRotateAndAttackAgain()
        {
            delayToAttackAgain += Time.deltaTime;
            if (delayToAttackAgain > (waitToAttack * 3))
            {
                ResetVariables();
            }
        }

        private void ResetVariables()
        {
            targetShipRotation = Quaternion.identity;
            delayToAttackAgain = 0;
            delayToAttack = 0;
            shipCollided = false;
            rotateReady = false;
            rushAttacked = false;
        }

        private void StopShip()
        {
            components.rigidBody2d.MovePosition(components.rigidBody2d.position + Vector2.zero * Time.fixedDeltaTime);
        }

        private void DistanceFromPlayer()
        {
            distanceFromPlayer = Vector2.Distance(playerTransform.position, components.enemyTransform.position);

            if (distanceFromPlayer >= distanceToMove)
            {
                stateMachine.TransitionTo(stateMachine.MovementState);
            }
        }

        private void CheckCollision()
        {
            if (!IsMovingAttack()) return;
          

            Collider2D[] results = new Collider2D[collisionDetection];

            int collisionCount = components.shipCollider.OverlapCollider(contactFilter, results);

            for (int i = 0; i < collisionCount; i++)
            {
                Collider2D hitCollider = results[i];

                if (hitCollider.CompareTag(ConstantStrings.PlayerTag))
                {
                    Vector2 collisionPosition = (hitCollider.bounds.center + components.shipCollider.bounds.center) / 2;
                    hitCollider.GetComponent<ShipDamageable>().TakeDamage(collisionPosition);
                    components.enemyTransform.GetComponent<ShipDamageable>().Die();
                    shipCollided = true;
                    break;
                }
            }
        }

        private bool IsMovingAttack()
        {
            return components.rigidBody2d.velocity.magnitude > minSpeedToDamage;
        }

        private void SetInitialComponents()
        {
            if (playerTransform == null)
                playerTransform = GameManager.Instance.PlayerTransform;

            if (contactFilter.layerMask != LayerMask.GetMask(ConstantStrings.PlayerLayer))
            {
                contactFilter.useLayerMask = true;
                contactFilter.layerMask = LayerMask.GetMask(ConstantStrings.PlayerLayer);
            }
        }
        #endregion
    }
}
