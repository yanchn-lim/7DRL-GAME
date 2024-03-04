using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Patterns
{
    public class PoolingPattern<T> where T : MonoBehaviour //where it is a game object
    {
        private GameObject prefab;
        private Queue<T> queue;

        private Transform parentCache;
        //the output of it does not really matter
        private Func<T, T> initCommand;

        public PoolingPattern(GameObject prefab)
        {
            queue = new Queue<T>();
            this.prefab = prefab;
        }

        public void Init(int numberOfItems)
        {
            for (int i = 0; i < numberOfItems; i++)
            {
                Add();
            }
        }

        public void InitWithParent(int numberOfItems, Transform parent)
        {
            parentCache = parent;
            for (int i = 0; i < numberOfItems; i++)
            {
                Add(parent);
            }
        }
        public void InitWithParent(int numberOfItems, Transform parent , Func<T, T> initCommand)
        {
            this.initCommand = initCommand;
            parentCache = parent;

            for (int i = 0; i < numberOfItems; i++)
            {
                Add(parent);
            }
        }

        public void Init(int numberOfItems, Func<T, T> initCommand)
        {
            this.initCommand = initCommand;
            for (int i = 0; i < numberOfItems; i++)
            {
                Add();
            }
        }

        public void Add()
        {
            GameObject initObject = GameObject.Instantiate(prefab);
            initObject.SetActive(false);
            T component = initObject.GetComponent<T>();
            TryAddInitCommand(component);
            queue.Enqueue(component);
        }

        public void Add(Transform parent)
        {
            GameObject initObject = GameObject.Instantiate(prefab, parent);
            initObject.SetActive(false);
            T component = initObject.GetComponent<T>();
            TryAddInitCommand(component);
            queue.Enqueue(component);
        }

        private void TryAddInitCommand(T component)
        {
            if(initCommand != null)
            {
                initCommand(component);
            }
        }

        public T Get()
        {
            if (queue.Count == 0)
            {
                if(parentCache != null)
                {
                    Add(parentCache);
                }
                else
                {
                    Add();  
                }
            }
            var initObject = queue.Dequeue();
            initObject.gameObject.SetActive(true);
            return initObject;
        }

        public void Retrieve(T initObject)
        {
            initObject.gameObject.SetActive(false);
            queue.Enqueue(initObject);
        }

    }
}