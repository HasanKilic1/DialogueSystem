using System.Collections.Generic;
using UnityEngine;

namespace BetterDialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "BetterDialogue/Dialogue" , order = 0)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField]
        List<DialogueNode> nodes = new List<DialogueNode>();

#if UNITY_EDITOR
        private void Awake()
        {
            if(nodes.Count == 0)
            {
                // Add blank node
                nodes.Insert(0, new DialogueNode());
            }
        }
#endif
        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }



    }
}
