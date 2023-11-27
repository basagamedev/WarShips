using EnemyShip.State;
using Manager;
using PoolSystem;
using Ship.Damageable;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyShip
{
    public class EnemyShipController : MonoBehaviour
    {
        #region SERIALIZE_FIELDS
        [Header("Scripts")]
        [SerializeField] private ShipDamageable enemyShipDamageableScript = null;
        [SerializeField] private EnemyStateMachine enemyStateMachine = null;

        [Header("Components")]
        [SerializeField] private Transform enemyTransform = null;
        [SerializeField] private Transform cannonTransform = null;
        [SerializeField] private Rigidbody2D rigidBody2d = null;
        [SerializeField] private NavMeshAgent navMeshAgent = null;
        [SerializeField] private Collider2D shipCollider = null;
        #endregion

        #region PROPERTIES
        public ShipComponents Components => components;
        public ShipDamageable EnemyShipDamageableScript => enemyShipDamageableScript;
        public EnemyStateMachine EnemyStateMachine => enemyStateMachine;
        #endregion

        #region PRIVATE_FIELDS
        private ShipComponents components = null;
        #endregion

        #region UNITY_METHODS
        private void OnEnable()
        {
            EnableStateMachine(true);
        }

        private void Awake()
        {
            SetComponents();
            SetNavMeshAgent();
            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }
        #endregion

        #region PRIVATE_METHODS
        private void SetNavMeshAgent()
        {
            navMeshAgent.updateUpAxis = false;
            navMeshAgent.updateRotation = false;
            navMeshAgent.updatePosition = false;
        }

        private void EnableStateMachine(bool enable)
        {
            enemyStateMachine.enabled = enable;
        }
      
        private void EnemyDied()
        {
            EnableStateMachine(false);
        }

        private void ReturnToPool()
        {
            GenericObjectPool.Instance.ReturnObjectToPool(this);
        }

        private void SubscribeToEvents()
        {
            if (enemyShipDamageableScript != null)
            {
                enemyShipDamageableScript.Died += EnemyDied;
                enemyShipDamageableScript.Died += GameManager.Instance.EnemyDied;
                enemyShipDamageableScript.ReturnToPool += ReturnToPool;
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (enemyShipDamageableScript != null)
            {
                enemyShipDamageableScript.Died -= EnemyDied;
                enemyShipDamageableScript.Died -= GameManager.Instance.EnemyDied;
                enemyShipDamageableScript.ReturnToPool -= ReturnToPool;
            }
        }

        private void SetComponents()
        {
            if (components == null)
                components = new ShipComponents(enemyTransform, rigidBody2d, navMeshAgent, cannonTransform, shipCollider);
        }
        #endregion

        #region PUBLIC_METHODS
        public void EnableCannon(bool enable)
        {
            cannonTransform.gameObject.SetActive(enable);
        }
        #endregion
    }

    #region ENCAPSULED_CLASS
    public class ShipComponents
    {
        public Transform enemyTransform { get; private set; }
        public Transform cannonTransform { get; private set; }
        public Collider2D shipCollider { get; private set; }
        public Rigidbody2D rigidBody2d { get; private set; }
        public NavMeshAgent navMeshAgent { get; private set; }

        public ShipComponents(Transform enemyTransform, Rigidbody2D rigidBody2d, NavMeshAgent navMeshAgent, Transform cannonTransform, Collider2D shipCollider)
        {
            this.enemyTransform = enemyTransform;
            this.rigidBody2d = rigidBody2d;
            this.navMeshAgent = navMeshAgent;
            this.cannonTransform = cannonTransform;
            this.shipCollider = shipCollider;
        }
    }
    #endregion
}
