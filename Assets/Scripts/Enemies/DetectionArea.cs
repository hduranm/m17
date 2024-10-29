using System;
using UnityEngine;

public class DetectionArea : MonoBehaviour
{
    public event Action<GameObject> OnEnter;
    public event Action<GameObject> OnExit;

    public void OnTriggerEnter2D(Collider2D collider){
        OnEnter?.Invoke(collider.gameObject);
    }

    public void OnTriggerExit2D(Collider2D collider){
        OnExit?.Invoke(collider.gameObject);
    }
}
