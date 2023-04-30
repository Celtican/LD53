using System;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEnterInvoke : MonoBehaviour
{
    public UnityEvent action;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        action.Invoke();
    }
}