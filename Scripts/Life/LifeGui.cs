using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace kap35 {
    namespace lego {

        [RequireComponent(typeof(Life))]
        public class LifeGui : MonoBehaviour {
            [SerializeField] Image lifeBar;
            [SerializeField] TextMeshProUGUI lifeText;
            [SerializeField] LifeBarTextType lifeBarTextType = LifeBarTextType.Percentage;
            [SerializeField] string _name = "Life";
            
            private Life life;
            void Start() {
                life = GetComponent<Life>();
                life.AddEventOnLifeChange(UpdateBar);
            }

            private void UpdateBar() {
                float percentage = life.GetLife() / life.GetMaxLife();
                if (lifeBar != null) {
                    lifeBar.fillAmount = percentage;
                }

                if (lifeText != null && lifeBarTextType != LifeBarTextType.None) {
                    switch (lifeBarTextType) {
                        case LifeBarTextType.Percentage:
                            lifeText.text = (percentage * 100).ToString("0") + "%";
                            break;
                        case LifeBarTextType.CurrentLife:
                            lifeText.text = life.GetLife().ToString();
                            break;
                        case LifeBarTextType.Name:
                            lifeText.text = _name;
                            break;
                    }
                }
            }
            
            public LifeBarTextType GetLifeBarTextType() {
                return lifeBarTextType;
            }
            
            public TextMeshProUGUI GetLifeText() {
                return lifeText;
            }
        }
        
        [Serializable]
        public enum LifeBarTextType {
            None,
            Percentage,
            CurrentLife,
            Name,
        }
    }
}
