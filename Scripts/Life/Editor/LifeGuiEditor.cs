#if UNITY_EDITOR

using System;
using UnityEngine;
using UnityEditor;
using kap35.lego;

[CanEditMultipleObjects]
[CustomEditor(typeof(LifeGui))]
public class LifeGuiEditor : Editor
{
    
    #region properties

        private SerializedProperty lifeBar;
        private SerializedProperty lifeText;
        private SerializedProperty lifeBarTextType;
        private SerializedProperty _name;
    
    #endregion
    
    #region folders
    #endregion
    
    #region methods

        private void OnEnable()
        {
            lifeBar = serializedObject.FindProperty("lifeBar");
            lifeText = serializedObject.FindProperty("lifeText");
            lifeBarTextType = serializedObject.FindProperty("lifeBarTextType");
            _name = serializedObject.FindProperty("name");
        }

        public override void OnInspectorGUI()
        {
            LifeGui lifeGui = (LifeGui) target;
            serializedObject.Update();
            
            
            var style = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = 20
            };
            GUILayout.Label("Life GUI", style);
            
            EditorGUILayout.PropertyField(lifeBar);
            EditorGUILayout.PropertyField(lifeText);
            if (lifeGui.GetLifeText() != null)
            {
                EditorGUILayout.PropertyField(lifeBarTextType);
                if (lifeGui.GetLifeBarTextType() == LifeBarTextType.Name)
                    EditorGUILayout.PropertyField(_name);
            }

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
}

#endif