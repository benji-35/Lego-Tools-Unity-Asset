using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace kap35 {
    namespace lego {
        [DisallowMultipleComponent]
        public class Life : MonoBehaviour {
            [Header("Life Settings")]
            [SerializeField] private int maxLife = 100;
            [SerializeField] protected bool destroyOnDeath = false;
            [SerializeField] private List<string> damageTags = new List<string>();

            private int life = 100;
            [SerializeField] private UnityEvent onDeath;
            [SerializeField] private UnityEvent onLifeChage;

            [Header("Coin Settings")] [SerializeField]
            private GameObject coin;

            [SerializeField] private int yellowCoins = 10;
            [SerializeField] private int blueCoins = 5;
            [SerializeField] private int purpleCoins = 1;


            // Start is called before the first frame update
            void Start()
            {
                life = maxLife;
                OnStart();
            }

            public void AddEventOnDeath(UnityAction action)
            {
                onDeath.AddListener(action);
            }
            
            public void AddEventOnLifeChange(UnityAction action)
            {
                onLifeChage.AddListener(action);
            }

            private void GenerateCoin()
            {
                if (coin == null)
                    return;
                for (int i = 0; i < yellowCoins; i++)
                {
                    GameObject obj = Instantiate(coin, transform.position, Quaternion.identity);
                    int amount = UnityEngine.Random.Range(1, 100);
                    obj.GetComponent<Coin>().SetCoinValue(amount);
                }

                for (int i = 0; i < blueCoins; i++)
                {
                    GameObject obj = Instantiate(coin, transform.position, Quaternion.identity);
                    int amount = UnityEngine.Random.Range(100, 10000);
                    obj.GetComponent<Coin>().SetCoinValue(amount);
                }

                for (int i = 0; i < purpleCoins; i++)
                {
                    GameObject obj = Instantiate(coin, transform.position, Quaternion.identity);
                    int amount = UnityEngine.Random.Range(10000, 1000000);
                    obj.GetComponent<Coin>().SetCoinValue(amount);
                }
            }

            private void OnTriggerEnter(Collider other) {
                if (damageTags.Contains(other.tag)) {
                    TakeDamage(10);
                    Destroy(other.gameObject);
                }
            }

            public GameObject GetCoin()
            {
                return coin;
            }
            
            protected virtual void OnStart() {}
            
            protected virtual void OnDeath() {}
            
            protected virtual void OnLifeChange() {}
            
            public void SetLife(int life) {
                this.life = life;
            }
            
            public int GetMaxLife() {
                return maxLife;
            }
            
            public int GetLife() {
                return life;
            }
            
            public void addDamageTag(string tag) {
                damageTags.Add(tag);
            }
            
            public void removeDamageTag(string tag) {
                damageTags.Remove(tag);
            }
            
            public List<string> GetDamageTags() {
                return damageTags;
            }

            public void TakeDamage(int damages) {
                life -= damages;
                onLifeChage.Invoke();
                OnLifeChange();
                if (life <= 0) {
                    GenerateCoin();
                    onDeath.Invoke();
                    OnDeath();
                    if (destroyOnDeath)
                        Destroy(gameObject);
                }
            }
        }
    }
}
