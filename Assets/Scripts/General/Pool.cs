using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] private GameObject _Prefab;
    [SerializeField] private int _Size;

    private GameObject[] _Pool;
    private bool[] _Availability;

    private void Awake()
    {
        Assert.IsFalse(_Size <= 0);
        Assert.IsNotNull(_Prefab.GetComponent<IPoolable>());

        _Pool = new GameObject[_Size];
        _Availability = new bool[_Size];

        for (int i = 0; i < _Size; i++)
        {
            GameObject poolable = Instantiate(_Prefab, transform);
            poolable.SetActive(false);

            _Pool[i] = poolable;
            _Availability[i] = true;
        }
    }

    public GameObject GetElement()
    {
        for (int i = 0; i < _Size; i++)
        {
            if (_Availability[i])
            {
                _Availability[i] = false;
                return _Pool[i];
            }
        }

        return null;
    }

    public void ReturnElement(GameObject element)
    {
        Assert.IsFalse(!_Pool.Contains(element));

        for (int i = 0; i < _Size; i++)
        {
            if (_Pool[i] == element)
            {
                element.SetActive(false);
                _Availability[i] = true;
                return;
            }
        }
    }
}

