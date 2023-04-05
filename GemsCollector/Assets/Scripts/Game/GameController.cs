using System;
using UnityEngine;
using ANSEI.Utils;
using ANSEI.GemsCollector.Data;
using ANSEI.GemsCollector.UI;

namespace ANSEI.GemsCollector.Game
{
    public class GameController : Singleton<GameController>
    {
        internal Action<int, int> OnMoved = null;
        internal Action<bool> OnLevelEnded = null;

        internal Progress Progress = null;
        internal int LastScore;

        #region UnityMEFs

        private void Start()
        {
            Progress = new Progress();
            Progress.Load();
            Progress.CurrentLevel.OnLevelEnd += EndGame;
        }

        private void OnLevelWasLoaded(int level)
        {
            if(level == 1)
            {
                GameFieldController.Instance.OnCollected += CalculateScore;
                StartGame();
            }
        }

        #endregion

        #region MEFs

        internal void StartGame()
        {
            GameFieldController.Instance.CreateGameField();
            GameFieldController.Instance.IsFieldLocked = true;
        }

        internal void RestartGame()
        {
            GameFieldController.Instance.RebuildGameField();
            Progress.CurrentLevel.SetupLevelSettings();
        }

        internal void EndGame(bool isWin)
        {
            GameFieldController.Instance.IsFieldLocked = true;
            LastScore = Progress.CurrentLevel.CurrentScore;

            if(isWin)
            {
                Progress.NextLevel();
            }
            else
            {
                RestartGame();
            }

            OnLevelEnded?.Invoke(isWin);
        }

        internal void ClearLevel()
        {
            Progress.SetupLevel();
            GameFieldController.Instance.ClearGameField();
        }

        private void CalculateScore(int chainLength)
        {
            Progress.CurrentLevel.CalculateScore(chainLength);
            OnMoved?.Invoke(Progress.CurrentLevel.CurrentScore, Progress.CurrentLevel.Turns);
        }

        #endregion
    }
}