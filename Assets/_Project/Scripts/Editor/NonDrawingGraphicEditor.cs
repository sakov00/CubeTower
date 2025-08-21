using _Project.Scripts.Rendering;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace _Project.Scripts.Editor
{
    [CanEditMultipleObjects, CustomEditor(typeof(NonDrawingGraphic), false)]
    public class NonDrawingGraphicEditor : GraphicEditor
    {
        public override void OnInspectorGUI()
        {
            base.serializedObject.Update();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(base.m_Script, new GUILayoutOption[0]);
            EditorGUI.EndDisabledGroup();
            // skipping AppearanceControlsGUI
            base.RaycastControlsGUI();
            base.serializedObject.ApplyModifiedProperties();
        }
    }
}