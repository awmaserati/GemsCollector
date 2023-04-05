using System.Collections.Generic;
using UnityEngine;
using ANSEI.Utils;

namespace ANSEI.GemsCollector.Game
{
    public class Generator : Singleton<Generator>
    {
        internal Transform Parent { get; private set; }

        private GemView[,] _generatorColumns;
        private Vector2 _fieldSize;
        private float _offset;

        #region UnityMEFs

        protected override void Awake()
        {
            base.Awake();
            Parent = transform;
            InitSettings();
            _generatorColumns = new GemView[(int)_fieldSize.x, (int)_fieldSize.y];
        }

        #endregion

        #region MEFs

        private void InitSettings()
        {
            _fieldSize = GameSettings.Instance.FieldSize;
            _offset = GameSettings.Instance.GemOffset;
        }

        internal void SetPosition(GemView view)
        {
            view.transform.SetParent(Parent);

            for(var i = 0; i < _fieldSize.x; i++)
            {
                if(i == view.Data.Column)
                {
                    for(var j = 0; j < _fieldSize.y; j++)
                    {
                        if (_generatorColumns[i, j] == null)
                        {
                            view.transform.localPosition = new Vector3(i * _offset, j * _offset, 0);
                            _generatorColumns[i, j] = view;
                            break;
                        }
                    }

                    break;
                }
            }

            
            //view.OnMoveFinished += RemoveFromGenerator;
        }

        internal void MoveDownGem(int x, int y, ref GemView gem)
        {
            for (var i = 0; i < _fieldSize.y; i++)
            {
                if (_generatorColumns[x, i] != null)
                {
                    gem = _generatorColumns[x, i];
                    gem.Data.UpdateData(y, x);
                    gem.MoveDown(new Vector3(x * _offset, y * _offset, 0));
                    _generatorColumns[x, i] = null;
                    break;
                }
            }
        }

        private void RemoveFromGenerator(GemView gem)
        {
            
        }

        #endregion
    }
}