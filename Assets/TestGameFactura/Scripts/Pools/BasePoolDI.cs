using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace TestGameFactura.Scripts.Pools
{
    [Serializable]
    public abstract class BasePoolDI<T> : ScriptableObject where T : MonoBehaviour
    {

        [SerializeField] private int maxSize;
        [SerializeField] private T behaviour;
        
        private DiContainer _container;
        private int _currentSize;
        private Queue<T> _pool = new();
        private List<T> _activeBehaviours = new();

        public Action OnObjectReleased;
        public List<T> ActiveBehaviours => _activeBehaviours;
        
        public void Init(DiContainer container)
        {
            _container = container;
        }
        
        public int CurrentSize => _activeBehaviours.Count;
        
        public void ClearPool()
        {
            _pool.Clear();
            _activeBehaviours.Clear();
            _currentSize = 0;
        }

        public T Get()
        {
            T obj;
            if (_pool.Count > 0)
            {
                obj = _pool.Dequeue();
                _activeBehaviours.Add(obj);
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
                    _activeBehaviours.Add(obj);
                }
            }
            
            obj.gameObject.SetActive(true);
            
            return obj;
        }

        public void Release(T obj, bool silent = false)
        {
            if (_pool.Count > maxSize)
            {
                if(_activeBehaviours.Contains(obj))
                    _activeBehaviours.Remove(obj);
                Destroy(obj);
            }
            else
            {
                _currentSize--;
                obj.gameObject.SetActive(false);
                _activeBehaviours.Remove(obj);
                _pool.Enqueue(obj);
            }
            
            if(silent == false)
                OnObjectReleased?.Invoke();
        }
    }
}
