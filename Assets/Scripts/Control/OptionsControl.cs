using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Control.UI.Menu
{
    public class OptionsControl : MonoBehaviour
    {
        #region SERIALIZE_FIELDS
        [Header("Components")]
        [SerializeField] private Button OkButton = null;
        [SerializeField] private Slider sliderSessionTime = null;
        [SerializeField] private Slider sliderFieldSpawnTime = null;
        [SerializeField] private TMP_Text secondsSessionTimeText = null;
        [SerializeField] private TMP_Text secondsSpawnTimeText = null;
        [SerializeField] private GameObject optionsMenu = null;
        #endregion

        #region UNITY_METHODS
        private void Start()
        {
            OkButton.onClick.AddListener(SaveInputFieldData);
            sliderSessionTime.onValueChanged.AddListener(SessionTimeValueChanged);
            sliderFieldSpawnTime.onValueChanged.AddListener(SpawnTimeValueChanged);
            SetInputTimeValues();
        }
        #endregion

        #region PRIVATE_METHODS
        private void SetInputTimeValues()
        {
            sliderSessionTime.value = ConfigManager.Instance.GetTimeSession();
            sliderFieldSpawnTime.value = ConfigManager.Instance.GetTimeEnemySpawn();
        }
        #endregion

        #region BUTTON_METHODS
        private void SaveInputFieldData()
        {
            int timeSession =  int.Parse(secondsSessionTimeText.text);
            int timeEnemySpawn = int.Parse(secondsSpawnTimeText.text);
            ConfigManager.Instance.SetTimeValues(timeSession, timeEnemySpawn);
            optionsMenu.SetActive(false);
        }
        #endregion

        #region SLIDER_METHODS
        private void SessionTimeValueChanged(float value)
        {
            secondsSessionTimeText.text = value.ToString();
        }

        private void SpawnTimeValueChanged(float value)
        {
            secondsSpawnTimeText.text = value.ToString();
        }
        #endregion
    }
}
