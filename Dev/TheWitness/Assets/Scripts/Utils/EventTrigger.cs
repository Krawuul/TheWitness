namespace Utils
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    public class EventTrigger : MonoBehaviour
    {
        [SerializeField, Tooltip("Work on Enter and Exit")] private bool onlyTriggerOnce = false;

        private bool hasTriggeredEnter = false;
        private bool hasTriggeredEnd = false;
        int m_id;

        public event Action OnEnter;
        public event Action<Collider> OnEnterCollider;
        [SerializeField] UnityEvent m_eventOnEnterCollider;

        public event Action OnStay;
        public event Action<Collider> OnStayCollider;
        [SerializeField] UnityEvent m_eventOnStayCollider;

        public event Action OnExit;
        public event Action<Collider> OnExitCollider;
        [SerializeField] UnityEvent m_eventOnExitCollider;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                if (onlyTriggerOnce)
                {
                    if (!hasTriggeredEnter)
                    {
                        hasTriggeredEnter = true;

                        OnEnter?.Invoke();
                        OnEnterCollider?.Invoke(other);
                        m_eventOnEnterCollider?.Invoke();
                    }
                }
                else
                {
                    OnEnter?.Invoke();
                    OnEnterCollider?.Invoke(other);
                    m_eventOnEnterCollider?.Invoke();
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Player")
            {
                OnStay?.Invoke();
                OnStayCollider?.Invoke(other);
                m_eventOnStayCollider?.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                if (onlyTriggerOnce)
                {
                    if (!hasTriggeredEnd)
                    {
                        hasTriggeredEnd = true;

                        OnExit?.Invoke();
                        OnExitCollider?.Invoke(other);
                        m_eventOnExitCollider?.Invoke();
                    }
                }
                else
                {
                    OnExit?.Invoke();
                    OnExitCollider?.Invoke(other);
                    m_eventOnExitCollider?.Invoke();
                }
            }
        }

       
    }

}