using System.Collections;
using UnityEngine;
using ANSEI.Utils;

namespace ANSEI.GemsCollector.Game
{
    public class GameSettings : Singleton<GameSettings>
    {
        private const string SOUNDKEY = "sound";

        [SerializeField]
        internal Vector2 FieldSize = Vector2.zero;
        [SerializeField]
        internal float GemOffset = 0;
        [SerializeField]
        internal int GemsCount = 4;
        [SerializeField]
        internal int MinChainLength = 3;
        [SerializeField]
        internal int StartGoalScore = 500;
        [SerializeField]
        internal int GemCost = 5;
        [SerializeField]
        internal int ChainLengtCoeff = 1;
        [SerializeField]
        internal int LevelGoalCoeff;
        [SerializeField]
        internal int LevelGoalMultiplier = 13;
        [SerializeField]
        internal int StartTurns;
        [SerializeField]
        internal int TurnsCoeff;
        [SerializeField]
        internal int Levels2ChangeLocos = 3;

        internal bool IsSound;

        protected override void Awake()
        {
            base.Awake();

            Load();
        }

        private void Load()
        {
            if(!PlayerPrefs.HasKey(SOUNDKEY))
            {
                IsSound = true;
                PlayerPrefs.SetInt(SOUNDKEY, 1);
                return;
            }

            IsSound = PlayerPrefs.GetInt(SOUNDKEY) == 1 ? true : false;
        }

        internal void Save()
        {
            PlayerPrefs.SetInt(SOUNDKEY, IsSound ? 1 : 0);
        }
    }
}