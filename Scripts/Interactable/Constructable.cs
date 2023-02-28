using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kap35 {
    namespace lego {
        public class Constructable : MonoBehaviour {
            [SerializeField] private GameObject _destructObject;
            [SerializeField] private GameObject _constructObject;
            [SerializeField] private bool isDestroyed = false;
            private Life _life;
            private Interactable _interactable;
            private Dictionary<int, Vector3> startDestructChildrenPos = new Dictionary<int, Vector3>();
            private Dictionary<int, Vector3> startDestructChildrenRot = new Dictionary<int, Vector3>();

            // Start is called before the first frame update
            void Start() {
                _life = _constructObject.GetComponent<Life>();
                _interactable = _destructObject.GetComponent<Interactable>();
                foreach (var tr in _destructObject.transform.GetComponentsInChildren<Transform>()) {
                    startDestructChildrenPos.Add(tr.gameObject.GetInstanceID(), tr.position);
                    startDestructChildrenRot.Add(tr.gameObject.GetInstanceID(), tr.rotation.eulerAngles);
                }
                if (_life != null) {
                    _life.AddEventOnDeath(DestructObject);
                }
                if (_interactable != null) {
                    _interactable.AddOnInteractEndEvent(ConstructObject);
                }
                if (isDestroyed) {
                    DestructObject();
                }
            }

            private void DestructObject() {
                foreach (var tr in _destructObject.transform.GetComponentsInChildren<Transform>()) {
                    int id = tr.gameObject.GetInstanceID();
                    Vector3 pos = startDestructChildrenPos[id];
                    Vector3 rot = startDestructChildrenRot[id];
                    if (pos != null && rot != null) {
                        tr.SetPositionAndRotation(pos, Quaternion.Euler(rot));
                    }
                }
                _destructObject.SetActive(true);
                _constructObject.SetActive(false);
            }
            
            private void ConstructObject() {
                if (_life != null) {
                    _life.SetLife(_life.GetMaxLife());
                }
                _destructObject.SetActive(false);
                _constructObject.SetActive(true);
            }
        }
    }
}
