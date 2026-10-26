using Core.ServiceLocator;
using Enums;
using UI;

namespace Core
{
    public interface IGamePopupsProvider : IService
    {
        bool TryOpen(PopupID popupID, bool force = false);
        BasicPopup GetCurrentPopup();
    }
}