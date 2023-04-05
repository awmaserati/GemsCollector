using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ANSEI.GemsCollector.Game
{
    public class GemView : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
    {
        internal Action<GemData> OnSelected = null;
        internal Action<GemView> OnDestroyed = null;
        internal Action<GemData> OnMoveFinished = null;

        [SerializeField]
        private List<Sprite> _sprites = null;
        [SerializeField]
        private Image _sprite = null;
        [SerializeField]
        private GameObject _frame = null;
        [Header("Animations")]
        [SerializeField]
        private float _scaleTime = 0.21f;
        [SerializeField]
        private AnimationCurve _scaleCurve = null;
        [SerializeField]
        private float _moveTime = 0.21f;
        [SerializeField]
        private AnimationCurve _moveCurve = null;

        internal GemData Data = new GemData();

        #region UnityMEFs

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!InputController.Instance.IsSelecting)
                return;

            OnSelected?.Invoke(Data);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnSelected?.Invoke(Data);
            _frame.SetActive(true);
            InputController.Instance.StartSelect();
        }

        #endregion

        #region MEFs

        internal void Init(int color, int row, int column)
        {
            transform.SetParent(GameFieldController.Instance.Parent);
            transform.localScale = Vector3.one;
            Data.Init(row, column, color);
            _sprite.sprite = _sprites[color];
            gameObject.name = string.Format("{0}x{1}", row, column);
        }

        internal void MoveDown(Vector3 newPosition)
        {
            LeanTween.moveLocal(gameObject, newPosition, _moveTime).setEase(_moveCurve);
            gameObject.name = string.Format("{0}x{1}", Data.Row, Data.Column);
        }

        internal void Select()
        {
            _frame.SetActive(true);
        }

        internal void Deselect()
        {
            _frame.SetActive(false);
        }

        internal void Kill()
        {
            Deselect();
            LeanTween.scale(gameObject, Vector3.zero, _scaleTime).setEase(_scaleCurve).setOnComplete(
                () => { OnDestroyed?.Invoke(this); });
            
        }

        #endregion
    }
}