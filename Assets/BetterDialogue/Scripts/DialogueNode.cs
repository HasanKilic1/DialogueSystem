using System;

namespace BetterDialogue
{
    [Serializable]
    public class DialogueNode
    {
        public string uniqueID;
        public string text;
        public string[] children;
    }

}
