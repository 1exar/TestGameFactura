using DG.Tweening;
using UnityEngine;

namespace TestGameFactura.Scripts.Managers.CameraManager
{
    public class CameraManager : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField] private Vector3 endPos;
        [SerializeField] private Vector3 endRotation;
        [SerializeField] private float duration;

        public void AnimateCamera()
        {
            transform.DORotate(endRotation, duration);
            transform.DOLocalMove(endPos, duration);
        }
    }
}