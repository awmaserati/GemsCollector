using System;

namespace ANSEI.GemsCollector.Game
{
    public class LevelData
    {
        internal Action<bool> OnLevelEnd = null;

        internal int Number;
        internal int GoalScore;
        internal int CurrentScore;
        internal int Turns;

        public LevelData(int number)
        {
            Number = number;
            SetupLevelSettings();
        }

        internal void SetupLevelSettings()
        {
            GoalScore = GameSettings.Instance.StartGoalScore + (Number - 1) * GameSettings.Instance.LevelGoalCoeff
                + (Number  - 1) * GameSettings.Instance.LevelGoalMultiplier;
            Turns = GameSettings.Instance.StartTurns + (Number - 1) * GameSettings.Instance.TurnsCoeff;
            CurrentScore = 0;
        }

        internal void NextLevel()
        {
            Number++;
            SetupLevelSettings();
        }

        internal void CalculateScore(int chainLength)
        {
            var bonusGemsCount = chainLength - GameSettings.Instance.MinChainLength;
            var totalScore = GameSettings.Instance.MinChainLength * GameSettings.Instance.GemCost;

            for(var i = 1; i <= bonusGemsCount; i++)
            {
                totalScore += i * GameSettings.Instance.ChainLengtCoeff + GameSettings.Instance.GemCost;
            }

            CurrentScore += totalScore;
            Turns--;

            if (CurrentScore >= GoalScore)
            {
                OnLevelEnd?.Invoke(true);
                return;
            }

            if (Turns == 0)
                OnLevelEnd?.Invoke(false);
        }
    }
}