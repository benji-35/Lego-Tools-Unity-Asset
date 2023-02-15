#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using kap35.lego;

[CustomEditor(typeof(Discuss))]
[CanEditMultipleObjects]
public class DiscussEditor : Editor
{
    #region vars
        private SerializedProperty talkerProperty;
        private SerializedProperty textProperty;
        private SerializedProperty nextDiscussionProperty;
        private bool showEdit = false;
    #endregion
    
    private void OnEnable() {
        talkerProperty = serializedObject.FindProperty("talker");
        if (talkerProperty == null)
            Debug.Log("talkerProperty is null");
        textProperty = serializedObject.FindProperty("text");
        if (textProperty == null)
            Debug.Log("textProperty is null");
        nextDiscussionProperty = serializedObject.FindProperty("nextDiscussion");
        if (nextDiscussionProperty == null)
            Debug.Log("nextDiscussionProperty is null");
    }
    
    public override void OnInspectorGUI() {
        Discuss discuss = (Discuss)target;
        serializedObject.Update();
        if (showEdit)
        {
            EditorGUILayout.LabelField("Edit discussion", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(talkerProperty, new GUIContent("Talker"));
            EditorGUILayout.PropertyField(textProperty, new GUIContent("Text"));
            EditorGUILayout.PropertyField(nextDiscussionProperty, new GUIContent("Next discussion"));
            if(GUILayout.Button("Save")) {
                showEdit = false;
            }
        } else {
            EditorGUILayout.LabelField(discuss.GetTalker() + " : ", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(discuss.GetText(), MessageType.None, true);
            if (discuss.GetNextDiscussion() != null)
            {
                EditorGUILayout.LabelField("Next discussion : ", EditorStyles.boldLabel);
                EditorGUILayout.HelpBox(discuss.GetNextDiscussion().GetTalker() + " : " + discuss.GetNextDiscussion().GetText(), MessageType.None, true);
            }
            if(GUILayout.Button("Edit")) {
                showEdit = true;
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}

#endif
