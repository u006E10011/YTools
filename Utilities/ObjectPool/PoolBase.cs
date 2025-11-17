using System;
using System.Collections.Generic;
using UnityEngine;

namespace YTools
{
    public class PoolBase<T>
    {
        private readonly Func<T> _preload;
        private readonly Action<T> _getItem;
        private readonly Action<T> _returnItem;

        public Queue<T> Pool { get; private set; } = new();
        public readonly List<T> ActiveItem = new();
        public readonly List<T> AllItems = new();

        private readonly bool _isAutoPreload;

        public PoolBase(Func<T> preload, Action<T> getItem, Action<T> returnItem, int preloadCount, bool isAutoPreload = true)
        {
            _preload = preload;
            _getItem = getItem;
            _returnItem = returnItem;
            _isAutoPreload = isAutoPreload;

            if (preload == null)
            {
                Debug.LogError("Preload Func is null");
                return;
            }

            for (int i = 0; i < preloadCount; i++)
                Return(CreateItem());
        }

        public T Get(Action<T> created = null)
        {
            T item = default;

            if (Pool.Count > 0)
                item = Pool.Dequeue();
            else if (_isAutoPreload)
            {
                item = _preload();
                AllItems.Add(item);
                created?.Invoke(item);
            }
            else
                return default;

            _getItem(item);
            ActiveItem.Add(item);

            return item;
        }

        public void Return(T item)
        {
            _returnItem(item);
            Pool.Enqueue(item);
            ActiveItem.Remove(item);
        }

        public void ReturnAll()
        {
            foreach (T item in ActiveItem.ToArray())
                Return(item);
        }

        public void Set(Queue<T> queue)
        {
            Pool = queue;
        }

        private T CreateItem()
        {
            var item = _preload();
            AllItems.Add(item);
            return item;
        }
    }
}