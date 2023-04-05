using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ANSEI.GemsCollector.Controllers;

namespace ANSEI.GemsCollector.UI
{
    public class MainUI : MonoBehaviour
    {
        [SerializeField]
        private Button _startBtn = null;
        [SerializeField]
        private Button _settingsBtn = null;
        [SerializeField]
        private TextColorChanger _coloredText = null;
        [SerializeField]
        private WindowBase _settingsPopup = null;

        private bool _isSettingsShow = false;

        #region UnityMEFs

        private void OnEnable()
        {
            _startBtn.onClick.AddListener(StartGame);
            _settingsBtn.onClick.AddListener(ShowSettings);
            _coloredText.StartTwinkle();
        }

        private void OnDisable()
        {
            _startBtn.onClick.RemoveListener(StartGame);
            _settingsBtn.onClick.RemoveListener(ShowSettings);
            _coloredText.StopTwinkle();
        }

        #endregion

        #region MEFs

        private void StartGame()
        {
            Jukebox.Instance.PlayBubble();
            Jukebox.Instance.PlayGameTheme();
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }

        private void ShowSettings()
        {
            Jukebox.Instance.PlayBubble();

            if(!_isSettingsShow)
            {
                _settingsPopup.Show();
                _isSettingsShow = true;
                return;
            }

            _isSettingsShow = false;
            _settingsPopup.Hide();
        }

        #endregion
    }
}
