using Constants;
using UnityEngine;

namespace Manager
{
    public class ConfigManager : MonoBehaviour
    {
        #region INSTANCE_FIELD
        public static ConfigManager Instance { get; private set; }
        #endregion

        #region PRIVATE_FIELDS
        private int timeSession = 0;
        private int timeEnemySpawn = 0;
        #endregion

        #region UNITY_METHODS
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else 
            {
                Destroy(gameObject);
            }

            SetDefaultValues();
        }
        #endregion

        #region PRIVATE_METHODS
        private void SetDefaultValues()
        {
            timeSession = ConstantNumbers.TimeSession;
            timeEnemySpawn = ConstantNumbers.TimeEnemySpawn;
        }
        #endregion

        #region PUBLIC_METHODS
        public int GetTimeSession()
        {
            return timeSession;
        }

        public int GetTimeEnemySpawn()
        {
            return timeEnemySpawn;
        }

        public void SetTimeValues(int timeSession, int timeEnemySpawn)
        {
            this.timeSession = timeSession;
            this.timeEnemySpawn = timeEnemySpawn;
        }
        #endregion
    }
}
