using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BetterDialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "BetterDialogue/Dialogue" , order = 0)]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        List<DialogueNode> nodes = new List<DialogueNode>();

#if UNITY_EDITOR
        private void Awake()
        {
            if (nodes.Count == 0) // Make sure there is a root node
            {
                CreateNode(null);
            }
        }
#endif
        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            List<DialogueNode> children = new List<DialogueNode>();
            if(parentNode.GetChildren() != null)
            {
                foreach (string childID in parentNode.GetChildren())
                {
                    if (childID == null) continue;
                    foreach (DialogueNode node in GetAllNodes())
                    {
                        if (node.name == childID) children.Add(node);
                    }
                }
            }

            return children;
        }

#if UNITY_EDITOR
        public void CreateNode(DialogueNode parent)
        {
            DialogueNode newNode = MakeNode(parent);
            
            Undo.RegisterCreatedObjectUndo(newNode, "Created dialogue Node");
            Undo.RecordObject(this, "Added dialogue node");

            nodes.Add(newNode);
        }

        private DialogueNode MakeNode(DialogueNode parent)
        {
            DialogueNode newNode = CreateInstance<DialogueNode>();
            newNode.name = Guid.NewGuid().ToString();
            if (parent != null)
            {
                newNode.SetPosition(parent.GetRect().position + Vector2.right * parent.GetRect().width);
                parent.AddChild(newNode.name);

                newNode.isPlayerSpeaking = !parent.isPlayerSpeaking;
            }

            return newNode;
        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
            Undo.RecordObject(this, "Deleted dialogue node");
            nodes.Remove(nodeToDelete);
            CleanDanglingChildren(nodeToDelete);
            Undo.DestroyObjectImmediate(nodeToDelete);
        }

        private void CleanDanglingChildren(DialogueNode nodeToDelete)
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                node.RemoveChild(nodeToDelete.name);
            }
            //Also remove child nodes
            //nodes.RemoveAll(n => nodeToDelete.children.Contains(n.uniqueID));
        }

        public void OnBeforeSerialize()
        {
            if(nodes.Count == 0)
            {
                DialogueNode newNode = MakeNode(null);
                AddNode(newNode);
            }
            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach(DialogueNode node in GetAllNodes())
                {
                    if(AssetDatabase.GetAssetPath(node) == "")
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
        }

        private void AddNode(DialogueNode newNode)
        {
            nodes.Add(newNode);
        }

        public void OnAfterDeserialize()
        {}
    }
#endif
}
