using UnityEngine.UI;
using UnityEngine;
using TMPro;
using ANSEI.GemsCollector.Game;
using ANSEI.GemsCollector.Controllers;

namespace ANSEI.GemsCollector.UI
{
    public class StartScreen : WindowBase
    {
        [SerializeField]
        private Button _goBtn = null;
        [SerializeField]
        private TMP_Text _level = null;
        [SerializeField]
        private TMP_Text _score = null;
        [SerializeField]
        private TMP_Text _turns = null;

        internal override void Show()
        {
            _goBtn.onClick.AddListener(Hide);
            _level.text = string.Format("Level {0}", GameController.Instance.Progress.CurrentLevel.Number);
            _score.text = GameController.Instance.Progress.CurrentLevel.GoalScore.ToString();
            _turns.text = GameController.Instance.Progress.CurrentLevel.Turns.ToString();

            base.Show();
        }

        internal override void Hide()
        {
            Jukebox.Instance.PlayBubble();
            _goBtn.onClick.RemoveAllListeners();
            GameFieldController.Instance.IsFieldLocked = false;

            base.Hide();
        }
    }
}