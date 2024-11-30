using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace BetterDialogue
{
    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;


        [MenuItem("Window/BetterDialogue/DialogueEditor")]
        public static void ShowEditorWindow() 
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAsset(1)]
        public static bool DialogueAssetOpenCallback(int instanceID , int line)
        {
            Object obj = EditorUtility.InstanceIDToObject(instanceID);
            if(obj is Dialogue)
            {
                ShowEditorWindow();
                return true;
            }
            return false;
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;
        }

        private void OnGUI()
        {
            RefreshLabels();
        }

        private void OnSelectionChanged()
        {
            Dialogue dialogue = Selection.activeObject as Dialogue;
            if(dialogue != null )
            {
                selectedDialogue = dialogue;
                Repaint();
            }
        }

        private void RefreshLabels()
        {
            if (selectedDialogue == null)
            {
                EditorGUILayout.LabelField($"No Dialogue selected.");
            }
            else
            {
                GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 16,
                    normal = { textColor = Color.white },
                    padding = new RectOffset(10, 10, 5, 5),
                    fixedHeight = 30
                };

                GUILayout.BeginArea(new Rect(20, 20, 200, 300) , "Dialogue node" , headerStyle);
                GUILayout.Space(20);
                GUILayout.Button("Click");
                GUILayout.Space(10);

                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    EditorGUI.BeginChangeCheck();

                    EditorGUILayout.LabelField("Node:");
                    string newID = EditorGUILayout.TextField(node.uniqueID);
                    string newText = EditorGUILayout.TextField(node.text);
                    
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(selectedDialogue, "Update dialogue text"); //Record the objects current status
                        node.uniqueID = newID;
                        node.text = newText;
                    }
                }
                GUILayout.EndArea();
            }
        }
    }
}

