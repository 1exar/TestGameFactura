using System;
using TestGameFactura.SceneTransition.Scripts.Controllers;
using TMPro;
using UnityEngine;

namespace TestGameFactura.Scripts.Managers.UIManager
{
    public class UIManager : MonoBehaviour, IUIManager
    {
        
        [SerializeField] private SceneTransitionController sceneTransitionController;
        
        [Header("End Game Panel")]
        [SerializeField] private GameObject endGamePanel;
        [SerializeField] private TMP_Text statusText;
        
        public event Action OnClickRestart;

        public void ShowTransition(bool hide)
        {
            sceneTransitionController.PlayTransition(hide);
        }

        public void ShowEndGamePanel(bool win)
        {
            endGamePanel.SetActive(true);
            statusText.text = win ? "YOU WIN!" : "YOU LOSE!";
        }

        public void OnClickRestartButton()
        {
            endGamePanel.gameObject.SetActive(false);
            OnClickRestart?.Invoke();
        }
    }
}