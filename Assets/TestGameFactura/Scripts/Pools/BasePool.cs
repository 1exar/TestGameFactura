using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace TestGameFactura.Scripts.Pools
{
    [Serializable]
    public abstract class BasePool<T> : ScriptableObject where T : MonoBehaviour
    {

        [SerializeField] private int maxSize;
        [SerializeField] private T behaviour;
        
        public Action OnObjectReleased;
        
        private DiContainer _container;
        private int _currentSize;
        private Queue<T> _pool = new();

        public void Init(DiContainer container)
        {
            _container = container;
        }
        
        public int CurrentSize => _currentSize;
        
        public void ClearPool()
        {
            _pool.Clear();
            _currentSize = 0;
        }

        public T Get()
        {
            T obj;
            if (_pool.Count > 0)
            {
                obj = _pool.Dequeue();
            }
            else
            {
                if (_currentSize >= maxSize)
                {
                    Debug.LogError("pool is full");
                    return null;
                }
                else
                {
                    _currentSize++;
                    obj = _container.InstantiatePrefab(behaviour).GetComponent<T>();
                }
            }
            
            obj.gameObject.SetActive(true);
            
            return obj;
        }

        public void Release(T obj)
        {
            if (_pool.Count > maxSize)
            {
                Destroy(obj);
            }
            else
            {
                _currentSize--;
                obj.gameObject.SetActive(false);
                _pool.Enqueue(obj);
            }
            
            OnObjectReleased?.Invoke();
        }
    }
}
