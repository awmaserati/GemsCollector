using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ANSEI.GemsCollector.Game;
using ANSEI.GemsCollector.Controllers;
using TMPro;

namespace ANSEI.GemsCollector.UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField]
        private Button _backBtn = null;
        [SerializeField]
        private Button _restartBtn = null;
        [SerializeField]
        private TMP_Text _score = null;
        [SerializeField]
        private TMP_Text _turns = null;
        [SerializeField]
        private WindowBase _startScreen = null;
        [SerializeField]
        private WinScreen _winScreen = null;
        [SerializeField]
        private WindowBase _loseScreen = null;
        [SerializeField]
        private Image _bcg = null;
        [SerializeField]
        private List<Sprite> _locations = null;

        private int _currentLocosIndex = -1;

        #region UnityMEFs

        private void OnEnable()
        {
            UpdateUI(GameController.Instance.Progress.CurrentLevel.CurrentScore,
                GameController.Instance.Progress.CurrentLevel.Turns);
            _backBtn.onClick.AddListener(Back2Menu);
            _restartBtn.onClick.AddListener(RestartGame);
            GameController.Instance.OnMoved += UpdateUI;
            GameController.Instance.OnLevelEnded += EndLevel;
            _winScreen.OnNextLevel += ShowStartScreen;
            ShowStartScreen();
        }

        private void OnDisable()
        {
            GameController.Instance.OnMoved -= UpdateUI;
            _backBtn.onClick.RemoveListener(Back2Menu);
            _restartBtn.onClick.RemoveListener(RestartGame);
        }

        #endregion

        #region MEFs

        private void Back2Menu()
        {
            Jukebox.Instance.PlayBubble();
            Jukebox.Instance.PlayMenuTheme();
            GameController.Instance.ClearLevel();
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

        private void RestartGame()
        {
            Jukebox.Instance.PlayBubble();
            GameController.Instance.RestartGame();
            UpdateUI(GameController.Instance.Progress.CurrentLevel.CurrentScore,
                GameController.Instance.Progress.CurrentLevel.Turns);
        }

        private void ShowStartScreen()
        {
            _startScreen.Show();
            UpdateLocos();
        }

        private void UpdateUI(int score, int turns)
        {
            _score.text = score.ToString();
            _turns.text = turns.ToString();
        }

        private void UpdateLocos()
        {
            int index = GameController.Instance.Progress.CurrentLevel.Number / GameSettings.Instance.Levels2ChangeLocos;

            if (index >= _locations.Count)
                index = 0;

            if (_currentLocosIndex == index)
                return;

            _currentLocosIndex = index;
            _bcg.sprite = _locations[index];
        }

        private void EndLevel(bool isWin)
        {
            if (isWin)
                _winScreen.Show();
            else
                _loseScreen.Show();

            UpdateUI(GameController.Instance.Progress.CurrentLevel.CurrentScore,
                GameController.Instance.Progress.CurrentLevel.Turns);
        }

        #endregion
    }
}