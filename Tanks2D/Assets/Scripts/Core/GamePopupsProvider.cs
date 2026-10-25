using System;
using System.Linq;
using Core.ServiceLocator;
using Enums;
using UI;
using UnityEngine;

namespace Core
{
    public class GamePopupsProvider : MonoBehaviour, IService
    {
        [SerializeField] private PopupData[] _popups = new PopupData[0];

        private BasicPopup _currentPopup;

        private void Start()
        {
            BlockCursor();
            foreach (var popup in _popups)
            {
                popup.popup.OnShow += OnPopupOpened;
                popup.popup.OnClose += OnPopupClosed;
            }
        }

        private void OnPopupOpened(BasicPopup popup)
        {
            _currentPopup = popup;
            UnlockCursor();
        }

        private void OnPopupClosed(BasicPopup popup)
        {
            _currentPopup = null;
            BlockCursor();
        }

        private void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void BlockCursor()
        {
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }

        public bool TryOpen(PopupID popupID, bool force = false)
        {
            var popup = _popups.FirstOrDefault(x=>x.id == popupID);
            if(popup == null)
            {
                Debug.LogError($"Could not find data for {popupID}");
                return false;
            }
            
            if (_currentPopup != null)
            {
                if (force)
                {
                    _currentPopup.Close();
                }
                else
                {
                    Debug.LogWarning($"Close {_currentPopup.gameObject} to open {popupID}");
                    return false;
                }
            }
            
            popup.popup.Open();
            return true;
        }

        [Serializable]
        private class PopupData
        {
            public PopupID id;
            public BasicPopup popup;
        }
    }
}