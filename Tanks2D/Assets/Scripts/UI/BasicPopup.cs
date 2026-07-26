using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace UI
{
    public class BasicPopup : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup CanvasGroup;
        [SerializeField] protected Canvas Canvas;
        [SerializeField] protected float _appearTime = .5f;
        [SerializeField] protected float _closeTime = .3f;
        protected TweenerCore<float, float, FloatOptions> _anim;

        public bool IsOpen => Canvas.enabled;

        public virtual void Open()
        {
            if (_anim.IsActive())
                return;
            Canvas.enabled = true;
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
            _anim = CanvasGroup.DOFade(1f, _appearTime);
            OnShow?.Invoke(this);
        }


        public virtual void Close()
        {
            if (_anim.IsActive())
                return;

            _anim = CanvasGroup.DOFade(0f, _closeTime);
            _anim.onComplete += () =>
            {
                Canvas.enabled = false;
                CanvasGroup.interactable = false;
                CanvasGroup.blocksRaycasts = false;
            };

            OnClose?.Invoke(this);
        }

        public event Action<BasicPopup> OnShow;
        public event Action<BasicPopup> OnClose;
    }
}