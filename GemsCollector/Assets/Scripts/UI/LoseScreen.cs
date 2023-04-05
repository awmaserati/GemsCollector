using UnityEngine;
using UnityEngine.UI;
using ANSEI.GemsCollector.Game;

namespace ANSEI.GemsCollector.UI
{
    public class LoseScreen : WindowBase
    {
        [SerializeField]
        private Button _restartBtn = null;

        internal override void Show()
        {
            _restartBtn.onClick.AddListener(Hide);

            base.Show();
        }

        internal override void Hide()
        {
            GameFieldController.Instance.IsFieldLocked = false;
            _restartBtn.onClick.RemoveAllListeners();

            base.Hide();
        }
    }
}