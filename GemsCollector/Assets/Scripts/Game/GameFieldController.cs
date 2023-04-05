using System;
using System.Collections.Generic;
using UnityEngine;
using ANSEI.Utils;
using ANSEI.GemsCollector.Controllers;
using Random = UnityEngine.Random;

namespace ANSEI.GemsCollector.Game
{
    public class GameFieldController : Singleton<GameFieldController>
    {
        internal Action<int> OnCollected = null;

        [SerializeField]
        private GameObject _gemPrefab = null;
        internal Transform Parent { get; private set; }
        internal bool IsFieldLocked;

        private Vector2 _fieldSize = Vector2.zero;
        private int _gemsCount = 0;
        private float _offset = 0;
        private int _minChainLength = 0;

        private GemView[,] _gems = null;
        private List<GemData> _selectedGems = null;
        private int _deletedChainCount = 0;

        #region UnityMEFs

        protected override void Awake()
        {
            base.Awake();
            Parent = transform;
            InitSettings();
            _gems = new GemView[(int)_fieldSize.x, (int)_fieldSize.y];
            _selectedGems = new List<GemData>();
            InputController.Instance.OnSelectFinished += DestroyGems;
        }

        #endregion


        #region MEFs

        private void InitSettings()
        {
            _fieldSize = GameSettings.Instance.FieldSize;
            _gemsCount = GameSettings.Instance.GemsCount;
            _offset = GameSettings.Instance.GemOffset;
            _minChainLength = GameSettings.Instance.MinChainLength;
        }

        internal void CreateGameField()
        {
            for (var i = 0; i < _fieldSize.x; i++)
            {
                for (var j = 0; j < _fieldSize.y; j++)
                {
                    var colorIndex = Random.Range(0, _gemsCount);
                    var go = Instantiate(_gemPrefab);
                    go.transform.SetParent(Parent);
                    go.transform.localPosition = new Vector3(i * _offset, j * _offset, 0);
                    var view = go.GetComponent<GemView>();
                    view.Init(colorIndex, j, i);
                    _gems[i, j] = view;
                    AddListeners(view);
                }
            }
        }

        internal void RebuildGameField()
        {
            ClearGameField();
            CreateGameField();
        }

        internal void ClearGameField()
        {
            for (var i = 0; i < _fieldSize.x; i++)
            {
                for (var j = 0; j < _fieldSize.y; j++)
                {
                    Destroy(_gems[i, j].gameObject);
                }
            }

            _gems = new GemView[(int)_fieldSize.x, (int)_fieldSize.y];
        }

        private void AddListeners(GemView view)
        {
            view.OnSelected += SelectGem;
            view.OnDestroyed += CreateGem;
        }

        private void RemoveListeners()
        {
            foreach(var gem in _gems)
            {
                gem.OnSelected -= SelectGem;
                gem.OnDestroyed -= CreateGem;
            }
        }

        internal void CreateGem(GemView view)
        {
            Generator.Instance.SetPosition(view);
            var colorIndex = Random.Range(0, _gemsCount);
            view.gameObject.SetActive(true);
            _gems[view.Data.Column, view.Data.Row] = null;
            view.Init(colorIndex, view.Data.Row, (int)_fieldSize.x);
            _deletedChainCount--;

            if (_deletedChainCount == 0)
            {
                MoveDownGems();
            }
        }

        private void MoveDownGems()
        {
            _selectedGems = new List<GemData>();
            
            //move down gems on the field
            for (var i = 0; i < _fieldSize.x; i++)
            {
                for (var j = 0; j < _fieldSize.y; j++)
                {
                    if (_gems[i, j] == null)
                    {
                        if(j + 1 == _fieldSize.y)
                        {
                            break;
                        }
                        
                        for(var y = j + 1; y < _fieldSize.y; y++)
                        {
                            if(_gems[i, y] != null)
                            {
                                _gems[i, y].Data.UpdateData(j, i);
                                _gems[i, j] = _gems[i, y];
                                _gems[i, y] = null;
                                _gems[i, j].MoveDown(new Vector3(i * _offset, j * _offset, 0));
                                break;
                            }
                        }
                    }
                }
            }
            //move down gems from generator
            for(var i = 0; i < _fieldSize.x; i++)
            {
                for(var j = 0; j < _fieldSize.y; j++)
                {
                    if (_gems[i, j] != null)
                        continue;

                    Generator.Instance.MoveDownGem(i, j, ref _gems[i, j]);
                }
            }
        }

        private void SelectGem(GemData data)
        {
            if(_selectedGems.Count == 0)
            {
                Jukebox.Instance.PlayBubble();
                _selectedGems.Add(data);
                _gems[data.Column, data.Row].Select();
                return;
            }

            var last = _selectedGems[_selectedGems.Count - 1];

            if (last.Equals(data))
            {
                _selectedGems.Remove(data);
                _gems[data.Column, data.Row].Deselect();
                return;
            }

            if (_selectedGems.Count >= 2)
            {
                var prev = _selectedGems[_selectedGems.Count - 2];

                if (prev.Equals(data))
                {
                    _selectedGems.Remove(last);
                    _gems[last.Column, last.Row].Deselect();
                    return;
                }
            }

            if (_selectedGems.Contains(data))
                return;

            if (last.Nearby(data))
            {
                Jukebox.Instance.PlayBubble();
                _selectedGems.Add(data); 
                _gems[data.Column, data.Row].Select();
            }
        }

        private void DestroyGems()
        {
            if (_selectedGems.Count < _minChainLength)
            {
                foreach(var gem in _selectedGems)
                {
                    _gems[gem.Column, gem.Row].Deselect();
                }

                _selectedGems = new List<GemData>();

                return;
            }

            _deletedChainCount = _selectedGems.Count;
            Jukebox.Instance.OnCollect();
            OnCollected?.Invoke(_deletedChainCount);

            foreach (var gem in _selectedGems)
            {
                _gems[gem.Column, gem.Row].Kill();
            }
        }

        #endregion
    }
}
