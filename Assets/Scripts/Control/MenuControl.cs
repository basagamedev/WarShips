using Constants;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Control.UI.Menu
{
    public class MenuControl : MonoBehaviour
    {
        #region SERIALIZE_FIELDS
        [Header("Components")]
        [SerializeField] private Button playButton = null;
        [SerializeField] private Button optionButton = null;
        [SerializeField] private GameObject optionsMenu = null;
        #endregion

        #region UNITY_METHODS
        private void Start()
        {
            playButton.onClick.AddListener(LoadMinigameScene);
            optionButton.onClick.AddListener(OpenOptionsMenu);
        }
        #endregion

        #region BUTTON_METHODS
        private void LoadMinigameScene()
        {
            SceneManager.LoadScene(ConstantStrings.MinigameScene);
        }

        private void OpenOptionsMenu()
        {
            optionsMenu.SetActive(true);
        }
        #endregion
    }
}
