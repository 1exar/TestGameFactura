using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TestGameFactura.Scripts.UI.Slider
{
    public class CustomSlider : MonoBehaviour
    {

        [SerializeField] private Image redImage;
        [SerializeField] private Image yellowImage;

        [SerializeField] private float yellowDelay;
        [SerializeField] private float yellowTweenDuration;
        
        private int _maxValue;

        private float _currentValue;
        private Tween _yellowTween;
        private Coroutine _yellowCoroutine;

        public void Init(int maxHealth)
        {
            _maxValue = maxHealth;
            _currentValue = maxHealth;
        }
        
        public void SetValue(int value)
        {
            value = Mathf.Clamp(value, 0, _maxValue);
            _currentValue = value;

            float fillAmount = (float)_currentValue / _maxValue;
            fillAmount = Mathf.Clamp01(fillAmount);

            redImage.fillAmount = fillAmount;

            if (_yellowCoroutine != null) StopCoroutine(_yellowCoroutine);
            if (_yellowTween != null && _yellowTween.IsActive()) _yellowTween.Kill();

            _yellowCoroutine = StartCoroutine(DelayedYellowUpdate(fillAmount));
        }

        private IEnumerator DelayedYellowUpdate(float targetFill)
        {
            yield return new WaitForSeconds(yellowDelay);

            _yellowTween = yellowImage.DOFillAmount(targetFill, yellowTweenDuration).SetEase(Ease.Linear);
        }
    }
}