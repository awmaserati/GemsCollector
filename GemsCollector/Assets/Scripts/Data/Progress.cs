using System.Collections;
using ANSEI.GemsCollector.Game;
using UnityEngine;

namespace ANSEI.GemsCollector.Data
{
    public class Progress
    {
        private const string LVLKEY = "lvl";
        private const string LOCOSKEY = "locos";

        internal int Level;
        internal LevelData CurrentLevel;

        internal Progress()
        {
            Level = 1;
        }

        internal void Load()
        {
            if(PlayerPrefs.HasKey(LVLKEY))
            {
                Level = PlayerPrefs.GetInt(LVLKEY);
                SetupLevel();
                return;
            }

            Level = 1;
            SetupLevel();
        }

        internal void Save()
        {
            PlayerPrefs.SetInt(LVLKEY, CurrentLevel.Number);
        }

        internal void SetupLevel()
        {
            CurrentLevel = new LevelData(Level);
        }

        internal void NextLevel()
        {
            Level++;
            CurrentLevel.NextLevel();
            Save();
        }
    }
}
