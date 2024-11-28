using System;
using UnityEngine;
using UnityEngine.Events;

public class ItemEvent : MonoBehaviour, IInteractable
{
    public event Action OnInteract;
    [SerializeField] UnityEvent m_eventOnInteract;

    void IInteractable.Interact()
    {
        OnInteract?.Invoke();
        m_eventOnInteract?.Invoke();
    }
}
