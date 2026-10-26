using Core;
using Core.ServiceLocator;
using Enums;
using Gameplay.UpdatingInstance;
using UI;

namespace Gameplay
{
    public class KeyHolder : IService, IInit, IUpdatable
    {
        private IGamePlayerInput _gamePlayerInput;
        private IGamePopupsProvider _gamePopupsProvider;
        private GameProgressor _gameProgressor;
        public bool IgnorePause { get; } = true;

        public void Init()
        {
            _gamePlayerInput = Services.Get<IGamePlayerInput>();
            _gamePopupsProvider = Services.Get<IGamePopupsProvider>();
            _gameProgressor = Services.Get<GameProgressor>();
            GameInstance gameInstance = Services.Get<GameInstance>();
            gameInstance.TryAddNewUpdatable(this);
        }

        public void OnUpdate()
        {
            var  status = _gameProgressor.GameStatus;
            if (status == GameStatus.NotStarted) //if result popup opened
                return;

            var popup = _gamePopupsProvider.GetCurrentPopup();
            if (_gamePlayerInput.PausePressing)
            {
                if(popup != null && popup is PausePopup pausePopup && pausePopup.IsOpen)
                    pausePopup.Close();
                else
                    _gamePopupsProvider.TryOpen(PopupID.PausePopup);
            }
        }
    }
}