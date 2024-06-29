using UnityEditor;
using UnityEngine;

namespace TMPro.EditorUtilities
{
    [CustomEditor(typeof(Tadadak_TMPUGUI), true)]
    [CanEditMultipleObjects]
    public class Tadadak_EditorPanelUI : TMP_EditorPanelUI
    {
        private static readonly GUIContent STYLE_LABEL = new GUIContent("Text Style", "The style from a style sheet to be applied to the text.");


        //Inspector Labels
        private SerializedProperty fullText;
        private SerializedProperty bePrintedText;
        private SerializedProperty progressivelyPrintable;
        private SerializedProperty displayPause;
        private SerializedProperty displaySpeed;
        //나중에 텍스트 하이라이팅 옵션 추가하자
        protected override void OnEnable()
        {
            base.OnEnable();

            this.fullText = this.serializedObject.FindProperty("_fullText");
            this.bePrintedText = this.serializedObject.FindProperty("_bePrintedText");
            this.progressivelyPrintable = this.serializedObject.FindProperty("_progressivelyPrintable");
            this.displayPause = this.serializedObject.FindProperty("_displayPause");
            this.displaySpeed = this.serializedObject.FindProperty("_displaySpeed");
        }

        public override void OnInspectorGUI()
        {
            if (this.IsMixSelectionTypes())
                return;

            this.serializedObject.Update();

            this.DrawTadadakSettings();

            this.DrawMainSettings();

            this.DrawExtraSettings();

            EditorGUILayout.Space();

            if(this.m_HavePropertiesChanged)
            {
                EditorUtility.SetDirty(this.target);
            }

            this.serializedObject.ApplyModifiedProperties();
        }

        protected void DrawTadadakSettings()
        {
            EditorGUILayout.Space();

            Rect rect = EditorGUILayout.GetControlRect(false, 22);
            GUI.Label(rect, new GUIContent("<b>Tadadak Settings</b>"), TMP_UIStyleManager.sectionHeader);
            EditorGUI.indentLevel = 0;

            //Full Text 설정
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(this.fullText);
            if (EditorGUI.EndChangeCheck())
            {
                this.m_HavePropertiesChanged = true;
            }

            //Be Printed Text 설정
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(this.bePrintedText);
            if (EditorGUI.EndChangeCheck())
            {
                this.m_HavePropertiesChanged = false;
            }

            //Text Style 설정
            if (this.m_StyleNames != null)
            {
                rect = EditorGUILayout.GetControlRect(false, 17);

                this.m_TextStyleIndexLookup.TryGetValue(this.m_TextStyleHashCodeProp.intValue, out this.m_StyleSelectionIndex);

                EditorGUI.BeginChangeCheck();
                this.m_StyleSelectionIndex = EditorGUI.Popup(rect, Tadadak_EditorPanelUI.STYLE_LABEL, this.m_StyleSelectionIndex, this.m_StyleNames);

                if (EditorGUI.EndChangeCheck())
                {
                    this.m_TextStyleHashCodeProp.intValue = this.m_Styles[this.m_StyleSelectionIndex].hashCode;
                    this.m_TextComponent.textStyle = this.m_Styles[this.m_StyleSelectionIndex];
                    this.m_HavePropertiesChanged = true;
                }
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Display Options", EditorStyles.boldLabel);
            ++EditorGUI.indentLevel;
            {
                EditorGUILayout.PropertyField(this.progressivelyPrintable);
                EditorGUILayout.PropertyField(this.displayPause);
                EditorGUILayout.PropertyField(this.displaySpeed);
            }
            --EditorGUI.indentLevel;

            EditorGUILayout.Space();

            this.serializedObject.ApplyModifiedProperties();
        }

        protected override void DrawExtraSettings()
        {
            base.DrawExtraSettings();

            //하이라이팅 관련 이벤트 넣기
        }
    }
}
