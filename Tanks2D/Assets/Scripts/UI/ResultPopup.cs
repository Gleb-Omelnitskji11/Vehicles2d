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
        private GameProgressor _gameProgressor;

        private void Start()
        {
            _gameProgressor = Services.Get<GameProgressor>();
            _repeatButton.onClick.AddListener(RepeatGame);
        }

        public override void Open()
        {
            base.Open();
            Setup();
        }

        private void Setup()
        {
            bool isWin = _gameProgressor.GameStatus == GameStatus.Win;
            _winText.enabled = isWin;
            _loseText.enabled = !isWin;
        }

        private void RepeatGame()
        {
            Close();
            _gameProgressor.RepeatGame();
        }
    }
}