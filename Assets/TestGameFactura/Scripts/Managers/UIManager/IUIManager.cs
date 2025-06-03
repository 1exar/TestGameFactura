using System;

namespace TestGameFactura.Scripts.Managers.UIManager
{
    public interface IUIManager
    {
        public event Action OnClickRestart;
        public void ShowTransition(bool hide);
        public void ShowEndGamePanel(bool win);

    }
}
