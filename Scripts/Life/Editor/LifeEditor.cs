#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using kap35.lego;

[CanEditMultipleObjects]
[CustomEditor(typeof(Life))]
public class LifeEditor : Editor {
    #region properties

        SerializedProperty damageLayer;
        SerializedProperty maxLife;
        SerializedProperty destroyOnDeath;
        SerializedProperty onLifeChage;
        SerializedProperty onDeath;
        
        SerializedProperty coin;
        SerializedProperty yellowCoins;
        SerializedProperty blueCoins;
        SerializedProperty purpleCoins;

    #endregion

    #region folders

        private bool showCoinSettings = false;
        private bool showEventsSettings = false;
        int tagSelected = 0;

        #endregion
    
    #region methods

        private void OnEnable() {
            maxLife = serializedObject.FindProperty("maxLife");
            destroyOnDeath = serializedObject.FindProperty("destroyOnDeath");
            onDeath = serializedObject.FindProperty("onDeath");
            onLifeChage = serializedObject.FindProperty("onLifeChage");
            damageLayer = serializedObject.FindProperty("damageTags");
            
            coin = serializedObject.FindProperty("coin");
            yellowCoins = serializedObject.FindProperty("yellowCoins");
            blueCoins = serializedObject.FindProperty("blueCoins");
            purpleCoins = serializedObject.FindProperty("purpleCoins");
        }

        public override void OnInspectorGUI() {
            Life life = (Life) target;
            serializedObject.Update();
            
            var style = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = 20
            };
            GUILayout.Label("Life Settings", style);
            
            EditorGUILayout.PropertyField(maxLife, new GUIContent("Max Life"));
            displayTags(life);
//            EditorGUILayout.PropertyField(damageLayer, new GUIContent("Damage Tags"));
            
            EditorGUILayout.Space(20);
            displayEvents();
            EditorGUILayout.Space(20);
            displayCoins(life);
            serializedObject.ApplyModifiedProperties();
        }

        private void displayEvents() {
            showEventsSettings = EditorGUILayout.Foldout(showEventsSettings, "Events Settings");
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (showEventsSettings) {
                EditorGUILayout.PropertyField(destroyOnDeath, new GUIContent("Destroy On Death"));
                EditorGUILayout.PropertyField(onLifeChage, new GUIContent("On Life Change"));
                EditorGUILayout.PropertyField(onDeath, new GUIContent("On Death"));
            }
        }

        private void displayCoins(Life life) {
            showCoinSettings = EditorGUILayout.Foldout(showCoinSettings, "Coin Settings");
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (showCoinSettings) {
                EditorGUILayout.PropertyField(coin, new GUIContent("Coin"));
                if (life.GetCoin() != null) {
                    EditorGUILayout.PropertyField(yellowCoins, new GUIContent("Yellow Coins"));
                    EditorGUILayout.PropertyField(blueCoins, new GUIContent("Blue Coins"));
                    EditorGUILayout.PropertyField(purpleCoins, new GUIContent("Purple Coins"));
                } else {
                    EditorGUILayout.HelpBox("You need to set a coin prefab if you want to drop coins", MessageType.Warning);
                }
            }
        }

        private void displayTags(Life life)
        {
            EditorGUILayout.LabelField("Damage Tags");
            string[] currentTags = life.GetDamageTags().ToArray();
            foreach (var tag in currentTags) {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(tag);
                if (GUILayout.Button("Remove", getButtonStyle())) {
                    life.removeDamageTag(tag);
                }
                EditorGUILayout.EndHorizontal();
            }
            string[] availableTags = UnityEditorInternal.InternalEditorUtility.tags;
            
            List<string> newTags = new List<string>();
            foreach (var tag in availableTags) {
                if (!currentTags.Contains(tag)) {
                    newTags.Add(tag);
                }
            }
            if (tagSelected > newTags.Count - 1)
                tagSelected = newTags.Count - 1;
            tagSelected = EditorGUILayout.Popup(tagSelected, newTags.ToArray());
            if (GUILayout.Button("Add Damage Tag", getButtonStyle())) {
                life.addDamageTag(newTags[tagSelected]);
            }
            EditorGUILayout.HelpBox("Add tags to the list to make the object damageable by objects with the same tag", MessageType.Info);
        }
        
        private GUILayoutOption[] getButtonStyle() {
            return new GUILayoutOption[] {
                GUILayout.Height(25),
                GUILayout.Width(150)
            };
        }
        
        #endregion
}

#endif
