using System;
using UnityEngine;
using ANSEI.Utils;
using UnityEngine.EventSystems;
using TMPro;

namespace ANSEI.GemsCollector.Game
{
    public class InputController : Singleton<InputController>
    {
        internal Action OnSelectFinished = null;

        [SerializeField]
        private string _gemTag = "gem";
        [SerializeField]
        private TMP_Text _logger;

        private Ray _ray;
        private RaycastHit _hit;

        internal bool IsSelecting { get; private set; }

        #region UnityMEFs

        protected override void Awake()
        {
            base.Awake();
            IsSelecting = false;
        }

        void Update()
        {
            if (GameFieldController.Instance.IsFieldLocked)
                return;

#if UNITY_EDITOR
            
            if (Input.GetMouseButtonUp(0) && IsSelecting)
            {
                IsSelecting = false;
                OnSelectFinished?.Invoke();
            }

#endif

#if UNITY_ANDROID
            
            if (Input.GetMouseButtonUp(0))
            {
                if (Input.touchCount == 1 && IsSelecting)
                {
                    IsSelecting = false;
                    OnSelectFinished?.Invoke();
                }
            }
#endif
        }

        #endregion

        #region MEFs

        internal void StartSelect()
        {
            IsSelecting = true;
        }

        #endregion
    }
}