using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using ANSEI.GemsCollector.Game;

namespace ANSEI.GemsCollector.UI
{
    public class WinScreen : WindowBase
    {
        internal Action OnNextLevel = null;

        [SerializeField]
        private List<string> _words = null;
        [SerializeField]
        private TMP_Text _congoWord = null;
        [SerializeField]
        private Button _goBtn = null;
        [SerializeField]
        private TMP_Text _score = null;

        internal override void Show()
        {
            _score.text = GameController.Instance.LastScore.ToString();
            _congoWord.text = _words[UnityEngine.Random.Range(0, _words.Count)];
            _goBtn.onClick.AddListener(Hide);

            base.Show();
        }

        internal override void Hide()
        {
            OnNextLevel?.Invoke();
            _goBtn.onClick.RemoveAllListeners();

            base.Hide();
        }
    }
}