using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private readonly Queue<T> _availableObjects = new Queue<T>();
    private readonly List<T> _allObjects = new List<T>();
    private readonly GameObject _prefab;
    private readonly Transform _parent;

    public ObjectPool(GameObject prefab, Transform parent, int initialCount)
    {
        _prefab = prefab;
        _parent = parent;

        for (int i = 0; i < initialCount; i++)
        {
            var obj = CreateObject();
            _availableObjects.Enqueue(obj);
        }
    }

    private T CreateObject()
    {
        GameObject newObj = Object.Instantiate(_prefab, _parent);
        T component = newObj.GetComponent<T>();
        _allObjects.Add(component);
        newObj.SetActive(false);
        return component;
    }

    public T GetObject()
    {
        T obj = _availableObjects.Count > 0 ? _availableObjects.Dequeue() : CreateObject();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false);
        _availableObjects.Enqueue(obj);
    }

    public void ReturnAllObjects()
    {
        foreach (var obj in _allObjects)
        {
            obj.gameObject.SetActive(false);
            _availableObjects.Enqueue(obj);
        }
    }
}