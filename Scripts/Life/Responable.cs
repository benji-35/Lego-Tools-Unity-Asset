using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace kap35 {
    namespace lego {
        public class Responable : Life {
            [Header("Respawn Settings")]
            [SerializeField] private float respawnTime = 3f;
            
            [Header("Render Settings")]
            [SerializeField] private Renderer[] renderers;
            [SerializeField] private bool useChildrenRenderer = true;
            
            [Header("Collider Settings")]
            [SerializeField] private Collider[] colliders;
            [SerializeField] private bool useChildrenCollider = true;
            
            protected override void OnStart() {
                base.OnStart();
                destroyOnDeath = false;
                if (useChildrenRenderer) {
                    renderers = GetComponentsInChildren<Renderer>();
                }
                if (useChildrenCollider)
                    colliders = GetComponentsInChildren<Collider>();
            }
            
            protected override void OnDeath() {
                base.OnDeath();
                StartCoroutine(respawn());
            }
            
            IEnumerator respawn() {
                foreach (Renderer renderer in renderers) {
                    renderer.enabled = false;
                }
                foreach (Collider collider in colliders) {
                    collider.enabled = false;
                }
                yield return new WaitForSeconds(respawnTime);
                SetLife(GetMaxLife());
                foreach (Renderer renderer in renderers) {
                    renderer.enabled = true;
                }
                foreach (Collider collider in colliders) {
                    collider.enabled = true;
                }
            }
        }
    }
}
