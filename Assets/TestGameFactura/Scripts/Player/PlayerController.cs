using UnityEngine;

namespace TestGameFactura.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;

        private bool _isMoving = false;

        public void StartMoving()
        {
            _isMoving = true;
        }

        public void StopMoving()
        {
            _isMoving = false;
        }

        private void Update()
        {
            if (_isMoving)
            {
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }

        }
    }
}
