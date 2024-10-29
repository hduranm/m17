using System;
using UnityEngine;


public interface IPoolable
{
    public event Action<GameObject> OnReturn;
}
