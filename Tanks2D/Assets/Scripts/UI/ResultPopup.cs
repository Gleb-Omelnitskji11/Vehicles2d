using Core.SaveLoadSystem;
using Core.ServiceLocator;
using Enums;
using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ResultPopup : BasicPopup
    {
        [SerializeField] private TMP_Text _winText;
        [SerializeField] private TMP_Text _loseText;
        [SerializeField] private Button _repeatButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Button _exitButton;
        
        private GameProgressor _gameProgressor;
        private ISaveSystem _saveSystem;

        private void Start()
        {
            _gameProgressor = Services.Get<GameProgressor>();
            _saveSystem = Services.Get<ISaveSystem>();
            _repeatButton.onClick.AddListener(RepeatGame);
            _loadButton.onClick.AddListener(Load);
            _exitButton.onClick.AddListener(Quit);
        }

        public override void Open()
        {
            base.Open();
            Setup();
        }

        private void Setup()
        {
            bool isWin = _gameProgressor.MatchResult == MatchResult.Win;
            _winText.enabled = isWin;
            _loseText.enabled = !isWin;
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

        private void RepeatGame()
        {
            Close();
            _gameProgressor.RepeatGame();
        }

        private void Quit()
        {
            Application.Quit();
        }
    }
}