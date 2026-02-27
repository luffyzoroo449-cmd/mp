using UnityEngine;
using System;

namespace ShadowRace.UI
{
    [Serializable]
    public struct DialogueLine
    {
        [TextArea(3, 10)]
        public string text;
        public Sprite emotionPortrait; // Optional override for this specific line
    }

    [CreateAssetMenu(fileName = "New Dialogue", menuName = "ShadowRace/Dialogue Data")]
    public class DialogueData : ScriptableObject
    {
        public string npcName;
        public Sprite npcPortrait;
        public DialogueLine[] lines;
    }
}
