using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ANSEI.GemsCollector.Game;
using ANSEI.GemsCollector.Controllers;

namespace ANSEI.GemsCollector.UI
{
    public class SettingsPopup : WindowBase
    {
        [SerializeField]
        private Button _soundBtn = null;
        [SerializeField]
        private Image _on = null;
        [SerializeField]
        private Image _off = null;

        internal override void Show()
        {
            _on.gameObject.SetActive(GameSettings.Instance.IsSound);
            _off.gameObject.SetActive(!GameSettings.Instance.IsSound);
            _soundBtn.onClick.AddListener(OnSoundBtnClicked);

            base.Show();
        }

        internal override void Hide()
        {
            _soundBtn.onClick.RemoveAllListeners();

            base.Hide();
        }

        private void OnSoundBtnClicked()
        {
            Jukebox.Instance.PlayBubble();
            GameSettings.Instance.IsSound = !GameSettings.Instance.IsSound;
            _on.gameObject.SetActive(GameSettings.Instance.IsSound);
            _off.gameObject.SetActive(!GameSettings.Instance.IsSound);
            GameSettings.Instance.Save();

            if (GameSettings.Instance.IsSound)
                Jukebox.Instance.Play();
            else
                Jukebox.Instance.Stop();
        }
    }
}