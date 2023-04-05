using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ANSEI.GemsCollector.UI
{
    public class TextColorChanger : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _text = null;
        [SerializeField]
        private List<Color> _colors = null;
        [SerializeField]
        private Vector2 _delay = Vector2.zero;

        private float _currentTime = 0.0f;
        private float _currentDelay = 0.0f;

        #region MEFs

        internal void StartTwinkle()
        {
            StartCoroutine(Twinkling());
        }

        private IEnumerator Twinkling()
        {
            _currentDelay = Random.Range(_delay.x, _delay.y);
            Color nextColor;

            do
            {
                nextColor = _colors[Random.Range(0, _colors.Count)];
            } while (nextColor == _text.color);


            var startColor = _text.color;

            while(_currentTime < _currentDelay)
            {
                yield return null;
                _currentTime += Time.deltaTime;
                _text.color = Color.Lerp(startColor, nextColor, _currentTime / _currentDelay);
            }

            _currentTime = 0;
            StartCoroutine(Twinkling());
        }

        internal void StopTwinkle()
        {
            StopAllCoroutines();
            _text.color = _colors[0];
        }

        #endregion
    }
}