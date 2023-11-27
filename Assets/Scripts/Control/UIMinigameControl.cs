using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Constants;

namespace Control.UI.Minigame
{
    public class UIMinigameControl : MonoBehaviour
    {
        #region SERIALIZE_FIELDS
        [Header("Session Components")]
        [SerializeField] private TMP_Text sessionTimeText = null;
        [SerializeField] private TMP_Text sessionScoreText = null;

        [Header("EndGame Components")]
        [SerializeField] private GameObject endGamePanel = null;
        [SerializeField] private Button playAgainButton = null;
        [SerializeField] private Button mainMenuButton = null;
        [SerializeField] private TMP_Text endGameScoreText = null;
        #endregion

        #region PRIVATE_FIELDS
        private readonly string showIntNumbers = "0";
        #endregion

        #region UNITY_METHODS
        private void Start()
        {
            playAgainButton.onClick.AddListener(PlayAgain);
            mainMenuButton.onClick.AddListener(MainMenu);
        }
        #endregion

        #region PUBLIC_METHODS
        public void SetSessionTime(float time)
        {
            sessionTimeText.text = time.ToString(showIntNumbers);
        }

        public void SetScore(int score)
        {
            sessionScoreText.text = score.ToString();
            endGameScoreText.text = score.ToString();
        }

        public void AppearEndGamePopUp()
        {
            endGamePanel.SetActive(true);
        }
        #endregion

        #region PRIVATE_METHODS
        private void PlayAgain()
        {
            SceneManager.LoadScene(ConstantStrings.MinigameScene);
        }

        private void MainMenu()
        {
            SceneManager.LoadScene(ConstantStrings.MainMenuScene);
        }
        #endregion
    }
}
