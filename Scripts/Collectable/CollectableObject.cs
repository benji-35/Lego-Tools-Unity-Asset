using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace kap35
{
    namespace lego
    {
        public class CollectableObject : MonoBehaviour
        {
            [SerializeField] private UnityEvent onCollect;

            public void AddEventOnCollect(UnityAction action)
            {
                onCollect.AddListener(action);
            }

            private void OnTriggerEnter(Collider other)
            {
                if (other.tag == "Player")
                {
                    onCollect.Invoke();
                    Destroy(gameObject);
                }
            }
        }
    }
}
