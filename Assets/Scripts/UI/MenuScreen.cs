using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace AbandonMine.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MenuScreen : MonoBehaviour
    {
        public delegate void MenuScreenHandler();

        public event MenuScreenHandler OnShowEvent;
        public event MenuScreenHandler OnHideEvent;

        public bool IsVisible { get => gameObject.activeSelf; }
            
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void ShowScreen() => ShowScreen(null);

        public void ShowScreen(MenuScreenHandler OnShowCallback)
        {
            gameObject.SetActive(true);
            canvasGroup.alpha = 0.0f;
            canvasGroup.DOFade(1.0f, 0.3f).OnComplete(
                () =>
                {
                    OnShowEvent?.Invoke();
                    OnShowCallback?.Invoke();
                });
        }

        public void HideScreen() => HideScreen(null);

        public void HideScreen(MenuScreenHandler OnHideCallback)
        {
            canvasGroup.alpha = 1.0f;
            canvasGroup.DOFade(0.0f, 0.3f).OnComplete(
                () =>
                {
                    OnHideEvent?.Invoke();
                    OnHideCallback?.Invoke();
                    gameObject.SetActive(false);
                });
        }
    }
}
