using UnityEngine;
using ANSEI.Utils;
using ANSEI.GemsCollector.Game;

namespace ANSEI.GemsCollector.Controllers
{
    public class Jukebox : Singleton<Jukebox>
    {
        [SerializeField]
        private AudioClip _btnClick = null;
        [SerializeField]
        private AudioClip _gemsCollected = null;
        [SerializeField]
        private AudioClip _menuSound = null;
        [SerializeField]
        private AudioClip _gameSound = null;

        private AudioSource _jukebox = null;

        #region UnityMEFs

        private void Start()
        {
            _jukebox = GetComponent<AudioSource>();
            _jukebox.loop = true;
            _jukebox.clip = _menuSound;

            if (GameSettings.Instance.IsSound)
                Play();
        }

        //private void OnLevelWasLoaded(int level)
        //{
        //    _jukebox.Stop();

        //    switch(level)
        //    {
        //        case 0:
        //            _jukebox.clip = _menuSound;
        //            break;
        //        case 1:
        //            _jukebox.clip = _gameSound;
        //            break;
        //    }

        //    if (!GameSettings.Instance.IsSound)
        //        return;

        //    _jukebox.Play();
        //}

        #endregion

        #region MEFs

        internal void Play()
        {
            _jukebox.Play();
        }

        internal void Stop()
        {
            _jukebox.Stop();
        }

        internal void PlayMenuTheme()
        {
            if (!GameSettings.Instance.IsSound)
                return;

            _jukebox.clip = _menuSound;
            _jukebox.Play();
        }

        internal void PlayGameTheme()
        {
            if (!GameSettings.Instance.IsSound)
                return;

            _jukebox.clip = _gameSound;
            _jukebox.Play();
        }

        internal void PlayBubble()
        {
            if (!GameSettings.Instance.IsSound)
                return;

            _jukebox.PlayOneShot(_btnClick);
        }

        internal void OnCollect()
        {
            if (!GameSettings.Instance.IsSound)
                return;

            _jukebox.PlayOneShot(_gemsCollected);
        }

        #endregion
    }
}