using Constants;
using Control.UI.Minigame;
using EnemyShip.Manager;
using PlayerShip;
using Ship.Damageable;
using UnityEngine;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        #region INSTANCE_FIELD
        public static GameManager Instance = null;
        #endregion

        #region SERIALIZE_FIELDS
        [Header("Player Components")]
        [SerializeField] private Transform playerTransform = null;
        [SerializeField] private PlayerShipController playerShipController = null;
        [SerializeField] private PlayerMovement playerMovement = null;
        [SerializeField] private PlayerAttack playerAttack = null;
        [SerializeField] private PlayerCannonRotation playerCannonRotation = null;

        [Header("Scripts")]
        [SerializeField] private UIMinigameControl uiMinigameControlScript = null;
        [SerializeField] private EnemyManager enemyManagerScript = null;
        #endregion

        #region PRIVATE_FIELDS
        private bool isPlayerDead = false;
        private bool isMinigameFinished = false;
        private float sessionTime = 0;
        private int scoreGame = 0;
        #endregion

        #region PROPERTIES
        public Transform PlayerTransform => playerTransform;
        #endregion

        #region UNITY_METHODS
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }

            SetSessionConfig();
            SetEnemySpawnConfig();
        }

        private void Update()
        {
            SessionTimer();
        }
        #endregion

        #region PRIVATE_METHODS
        private void SetSessionConfig()
        {
            if (ConfigManager.Instance != null)
                sessionTime = ConfigManager.Instance.GetTimeSession();
            else
                sessionTime = ConstantNumbers.TimeSession;
        }

        private void SetEnemySpawnConfig()
        {
            if (ConfigManager.Instance != null)
                enemyManagerScript.SetTimeEnemySpawn(ConfigManager.Instance.GetTimeEnemySpawn());
            else
                enemyManagerScript.SetTimeEnemySpawn(ConstantNumbers.TimeEnemySpawn);
        }

        private void SessionTimer()
        {
            if (!isMinigameFinished)
            {
                sessionTime -= Time.deltaTime;
                sessionTime = Mathf.Clamp(sessionTime, 0, sessionTime);
                uiMinigameControlScript.SetSessionTime(sessionTime);

                if (sessionTime <= 0)
                {
                    isMinigameFinished = true;
                    StopMinigame();
                }
            }
        }

        private void StopPlayer()
        {
            playerShipController.enabled = false;
            playerMovement.enabled = false;
            playerAttack.enabled = false;
            playerCannonRotation.enabled = false;
        }

        private void StopSpawn()
        {
            enemyManagerScript.StopSpawn();
        }

        private void MinigameFinished()
        {
            isMinigameFinished = true;
        }

        private void StopMinigame()
        {
            MinigameFinished();
            StopPlayer();
            StopSpawn();
            AppearPopUp();
        }

        private void AppearPopUp()
        {
            uiMinigameControlScript.AppearEndGamePopUp();
        }
        #endregion

        #region PUBLIC_METHODS
        public void PlayerDied()
        {
            isPlayerDead = true;
            StopMinigame();
        }

        public void EnemyDied()
        {
            scoreGame++;
            uiMinigameControlScript.SetScore(scoreGame);
        }

        public bool CheckMinigameFinished()
        {
            return isMinigameFinished || isPlayerDead;
        }
        #endregion
    }
}