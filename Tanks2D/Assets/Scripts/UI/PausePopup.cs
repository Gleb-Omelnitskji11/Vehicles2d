using Core;
using Core.SaveLoadSystem;
using Core.ServiceLocator;
using Enums;
using Gameplay;
using Gameplay.UpdatingInstance;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PausePopup : BasicPopup
    {
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Button _quitButton;
        private GameProgressor _gameProgressor;
        private GamePopupsProvider _gamePopupsProvider;
        private ISaveSystem _saveSystem;

        private void Awake()
        {
            _resumeButton.onClick.AddListener(Resume);
            _restartButton.onClick.AddListener(Restart);
            _saveButton.onClick.AddListener(Save);
            _loadButton.onClick.AddListener(Load);
            _quitButton.onClick.AddListener(Quit);
        }

        private void Start()
        {
            _gameProgressor = Services.Get<GameProgressor>();
            _gamePopupsProvider = Services.Get<GamePopupsProvider>();
            _saveSystem = Services.Get<ISaveSystem>();

            UpdateLoadButton();
        }

        public void Update()
        {
            var  status = _gameProgressor.GameStatus;
            if (status == GameStatus.Lose || status == GameStatus.Win)
                return;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(IsOpen)
                    Close();
                else
                    _gamePopupsProvider.TryOpen(PopupID.PausePopup);
            }
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
        
        private void Restart()
        {
            _gameProgressor.RepeatGame();
            Close();
        }

        private void Save()
        {
            _saveSystem.Save();
            UpdateLoadButton();
        }

        private void UpdateLoadButton()
        {
            _loadButton.interactable = _saveSystem.HasSaved();
        }

        private void Load()
        {
            Close();
            _gameProgressor.Clear();
            var data = _saveSystem.GetSaveData();
            _gameProgressor.LoadGame(data);
        }


        private void Quit()
        {
            Application.Quit();
        }
    }
}