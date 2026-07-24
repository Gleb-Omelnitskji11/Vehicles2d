using Core.ServiceLocator;
using UI;
using UnityEngine;

namespace Core
{
    public class GamePopupsProvider : MonoBehaviour, IService
    {
        [SerializeField] private ResultPopup _resultPopup;
        [SerializeField] private PausePopup _pausePopup;
        
        //private BasicPopup _currentPopup;
        
        public ResultPopup ResultPopup => _resultPopup;
        public PausePopup PausePopup => _pausePopup;
    }
}