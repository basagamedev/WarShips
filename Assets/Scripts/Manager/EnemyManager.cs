using Constants;
using EnemyShip.State;
using PoolSystem;
using System.Collections;
using UnityEngine;
using static Constants.ConstantEnums;

namespace EnemyShip.Manager
{
    public class EnemyManager : MonoBehaviour
    {
        #region SERIALIZE_FIELDS
        [Header("Spawn Data")]
        [SerializeField] private Transform[] spawnPositions = null;

        [Header("Ship Data")]
        [SerializeField] private EnemyStyleScriptables[] shipScriptables = null;
        #endregion

        #region PRIVATE_FIELDS
        private Coroutine spawnEnemyCoroutine = null;
        private float delayToSpawn = 30f;
        private int minimumNumberEnemy = 2;
        #endregion

        #region UNITY_METHODS
        private void Start()
        {
            spawnEnemyCoroutine = StartCoroutine(SpawnEnemy());
        }
        #endregion

        #region IENUMERATOR_METHODS
        private IEnumerator SpawnEnemy()
        {
            Spawn();
            yield return new WaitForSeconds(delayToSpawn);
            spawnEnemyCoroutine = StartCoroutine(SpawnEnemy());
        }
        #endregion

        #region PUBLIC_METHODS
        public void StopSpawn()
        {
            StopCoroutine(spawnEnemyCoroutine);
        }

        public void SetTimeEnemySpawn(int delayToSpawn)
        {
            this.delayToSpawn = delayToSpawn;
        }
        #endregion

        #region PRIVATE_METHODS
        private void Spawn()
        {
            int randomNumberSpawn = Random.Range(minimumNumberEnemy, spawnPositions.Length);

            for (int i = 0; i < randomNumberSpawn; i++)
            {
                EnemyShipController enemyShip = GenericObjectPool.Instance.GetPooledObject<EnemyShipController>();
                SetEnemyPosition(enemyShip, i);
                SetShip(enemyShip);
                enemyShip.gameObject.tag = ConstantStrings.EnemyTag;
                enemyShip.gameObject.SetActive(true);
            }
        }

        private void SetEnemyPosition(EnemyShipController enemyShip, int index)
        {
            enemyShip.transform.position = spawnPositions[index].position;
        }

        private void SetShip(EnemyShipController enemyShip)
        {
            int randomEnemyStyle = Random.Range(0, shipScriptables.Length);

            EnemyStyleScriptables chosenEnemyStyle = shipScriptables[randomEnemyStyle];

            int randomEnemyData = Random.Range(0, chosenEnemyStyle.enemyData.Length);

            ShipStatsScriptable statsScriptable = chosenEnemyStyle.enemyData[randomEnemyData].shipStatsScriptable;
            ShipDamageableStats damageableStats = chosenEnemyStyle.enemyData[randomEnemyData].shipStatsScriptable.GetShipDamageableStats();

            enemyShip.EnemyShipDamageableScript.SetDamageableStats(damageableStats);
            enemyShip.EnemyShipDamageableScript.SetShipSkin(chosenEnemyStyle.enemyData[randomEnemyData].shipSkinScriptable);

            switch (chosenEnemyStyle.enemyStyle)
            {
                case EnemyStyle.Shooter:
                    SetShooterShip(enemyShip, statsScriptable);
                    break;
                case EnemyStyle.Chaser:
                    SetChaserShip(enemyShip, statsScriptable);
                    break;
            }
        }

        private void SetShooterShip(EnemyShipController enemyShip, ShipStatsScriptable statsScriptable)
        {
            enemyShip.EnableCannon(true);

            ShipMovementStats movementStats = statsScriptable.GetShipMovementStats();
            ShipCannonStats cannonStats = statsScriptable.GetShipCannonStats();
            ShipCannonBallStats cannonBallStats = statsScriptable.GetShipCannonBallStats();
            ShipAttackStats shipAttackStats = statsScriptable.GetShipAttackStats();

            EnemyMovement enemyMovement = null;
            EnemyShooterAttack enemyAttack = null;
           
            CreatingShooterMachineState();
            SettingShooterVariableState();

            void CreatingShooterMachineState()
            {
                enemyMovement = new(enemyShip.EnemyStateMachine);
                enemyAttack = new(enemyShip.EnemyStateMachine);
                enemyShip.EnemyStateMachine.SetStates(enemyMovement, enemyAttack);
            }

            void SettingShooterVariableState()
            {
                enemyMovement.SetMovementStats(movementStats);
                enemyMovement.SetAttackStats(shipAttackStats);
                enemyMovement.SetComponents(enemyShip.Components);

                enemyAttack.SetMovementStats(cannonStats, movementStats, cannonBallStats, shipAttackStats);
                enemyAttack.SetComponents(enemyShip.Components);
            }
        }

        private void SetChaserShip(EnemyShipController enemyShip, ShipStatsScriptable statsScriptable)
        {
            enemyShip.EnableCannon(false);

            ShipMovementStats movementStats = statsScriptable.GetShipMovementStats();
            ShipAttackStats shipAttackStats = statsScriptable.GetShipAttackStats();

            EnemyMovement enemyMovement = null;
            EnemyChaseAttack enemyAttack = null;

            CreatingChaserMachineState();
            SettingChaserVariableState();

            void CreatingChaserMachineState()
            {
                enemyMovement = new(enemyShip.EnemyStateMachine);
                enemyAttack = new(enemyShip.EnemyStateMachine);
                enemyShip.EnemyStateMachine.SetStates(enemyMovement, enemyAttack);
            }

            void SettingChaserVariableState()
            {
                enemyMovement.SetMovementStats(movementStats);
                enemyMovement.SetAttackStats(shipAttackStats);
                enemyMovement.SetComponents(enemyShip.Components);

                enemyAttack.SetMovementStats(movementStats, shipAttackStats);
                enemyAttack.SetComponents(enemyShip.Components);
            }
        }
        #endregion

        #region SERIALIZEBLE_FIELD
        [System.Serializable]
        public class EnemyStyleScriptables
        {
            public EnemyStyle enemyStyle = EnemyStyle.Shooter;
            public EnemyDataScriptables[] enemyData = null;
        }

        [System.Serializable]
        public class EnemyDataScriptables
        {
            public ShipSkinScriptable shipSkinScriptable = null;
            public ShipStatsScriptable shipStatsScriptable = null;
        }
        #endregion
    }
}
