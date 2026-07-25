using Core.ServiceLocator;
using Enums;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PausePopup : BasicPopup
    {
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Button _quitButton;
        private GameProgressor _gameProgressor;

        private void Awake()
        {
            _resumeButton.onClick.AddListener(Resume);
            _saveButton.onClick.AddListener(Save);
            _loadButton.onClick.AddListener(Load);
            _quitButton.onClick.AddListener(Quit);
        }

        private void Init()
        {
            _gameProgressor = Services.Get<GameProgressor>();
            
        }

        public override void Open()
        {
            base.Open();
            _gameProgressor.Pause();
        }

        public override void Close()
        {
            base.Close();
            _anim.onComplete += () => _gameProgressor.Resume();
        }
        
        private void Resume()
        {
            Close();
        }

        private void Save()
        {
            Close();
        }

        private void Load()
        {
            Close();
        }


        private void Quit()
        {
            Application.Quit();
        }
    }
}