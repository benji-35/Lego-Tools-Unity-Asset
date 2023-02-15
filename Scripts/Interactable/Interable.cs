using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace kap35
{
    namespace lego
    {
        public class Interable : MonoBehaviour
        {
            [SerializeField] private UnityEvent onInteract;
            [SerializeField] private bool isInteracting = false;
            private bool intercated = false;

            private void Update()
            {
                if (isInteracting && Input.GetKeyDown(KeyCode.E))
                {
                    intercated = true;
                    GameManger manager = GetGameManager();
                    if (manager != null)
                        manager.HideInteract();
                    onInteract.Invoke();
                    isInteracting = false;
                    OnInteract();
                }
            }

            protected virtual void OnInteract()
            {
            }

            private void OnTriggerEnter(Collider other)
            {
                if (other.tag != "Player")
                    return;
                intercated = false;
                GameManger manager = GetGameManager();
                if (manager != null)
                    manager.ShowInteract();
                isInteracting = true;
            }

            private void OnTriggerExit(Collider other)
            {
                if (other.tag != "Player")
                    return;
                GameManger manager = GetGameManager();
                if (!intercated && manager != null)
                    manager.HideInteract();
                isInteracting = false;
            }

            protected GameManger GetGameManager()
            {
                GameObject manager = GameObject.FindGameObjectWithTag("GameManager");
                if (manager == null)
                    return null;
                return manager.GetComponent<GameManger>();
            }

            public void AddOnInteractEvent(UnityAction action)
            {
                onInteract.AddListener(action);
            }
        }
    }
}
