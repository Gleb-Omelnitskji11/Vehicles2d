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
            _anim.onComplete += () => ScaleTime(0.001f);
        }

        public override void Close()
        {
            base.Close();
            ScaleTime(1f);
        }

        private void ScaleTime(float timeScale)
        {
            Time.timeScale = timeScale;
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