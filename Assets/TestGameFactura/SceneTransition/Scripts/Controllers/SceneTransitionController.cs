using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace TestGameFactura.SceneTransition.Scripts.Controllers
{
    public class SceneTransitionController : MonoBehaviour
    {
        [Header("Twist Settings")]
        [SerializeField] private Material twistMaterial;
        [SerializeField] private float twistMin = 0f;
        [SerializeField] private float twistMax = 10f;
        [SerializeField] private float twistDuration = 3f;
        [SerializeField] private float twistSpeed = 1f;
        [SerializeField] private Ease twistEase = Ease.Linear;

        [Header("Scale Settings")]
        [SerializeField] private Transform targetToScale;
        [SerializeField] private Vector3 scaleMin = Vector3.one;
        [SerializeField] private Vector3 scaleMax = Vector3.one * 1.5f;
        [SerializeField] private float scaleDuration = 4.5f;
        [SerializeField] private Ease scaleEase = Ease.OutCubic;
        [SerializeField] private float scaleOvershoot = 1.2f;

        [Header("Transition Settings")]
        [SerializeField] private UnityEvent onTransitionComplete;

        private Tween twistTween;
        private Tween scaleTween;

        public void PlayTransition(bool isHide)
        {
            twistTween = DOTween.To(
                () => twistMaterial.GetFloat("_TwistStrength"),
                x => twistMaterial.SetFloat("_TwistStrength", x),
                isHide ? twistMax : twistMin,
                twistDuration / twistSpeed
            ).From(isHide ? twistMin : twistMax).SetEase(twistEase);

            scaleTween = targetToScale.DOScale(
                    isHide ? scaleMax : scaleMin,
                scaleDuration
            )
            .From(isHide ? scaleMin : scaleMax)
            .SetEase(scaleEase, scaleOvershoot)
            .OnComplete(() => onTransitionComplete?.Invoke());
        }

        private void OnDestroy()
        {
            twistTween?.Kill();
            scaleTween?.Kill();
        }
    }
}