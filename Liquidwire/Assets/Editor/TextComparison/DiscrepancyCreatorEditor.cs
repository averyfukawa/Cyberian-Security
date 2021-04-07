using UnityEditor;
using UnityEngine;

namespace Editor.TextComparison
{
    [CustomEditor(typeof(TextCreator))]
    public class DiscrepancyCreatorEditor : UnityEditor.Editor
    {
        private TextCreator _textCreator;
        
        private void OnEnable()
        {
            _textCreator = FindObjectOfType<TextCreator>();
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Create discrepancy"))
            {
                _textCreator.StartSentence();
                
            }
            
            if (GUILayout.Button("Save"))
            {
                _textCreator.SetAnswers(_textCreator.discrepancyField);
                _textCreator.SetText();
            }
        }
        
    }
    
}