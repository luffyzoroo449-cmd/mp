using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

namespace ShadowRace.UI
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance { get; private set; }

        [Header("UI Elements")]
        public GameObject dialogueBox;
        public TextMeshProUGUI npcNameText;
        public TextMeshProUGUI dialogueText;
        public Image npcPortrait;
        
        [Header("Settings")]
        public float typingSpeed = 0.05f;
        
        private Queue<DialogueLine> lines;
        private bool isTyping = false;
        private string currentSentence = "";

        private void Awake()
        {
            if (Instance == null) Instance = this;
            lines = new Queue<DialogueLine>();
        }

        public void StartDialogue(DialogueData dialogueData)
        {
            dialogueBox.SetActive(true);
            npcNameText.text = dialogueData.npcName;
            
            if (dialogueData.npcPortrait != null && npcPortrait != null)
            {
                npcPortrait.sprite = dialogueData.npcPortrait;
                npcPortrait.gameObject.SetActive(true);
            }
            else if (npcPortrait != null)
            {
                npcPortrait.gameObject.SetActive(false);
            }

            lines.Clear();

            foreach (DialogueLine line in dialogueData.lines)
            {
                lines.Enqueue(line);
            }

            // Freeze player Time, or disable PlayerInputHandler
            Time.timeScale = 0f; 

            DisplayNextLine();
        }

        public void DisplayNextLine()
        {
            if (isTyping)
            {
                // Force finish typing current line
                StopAllCoroutines();
                dialogueText.text = currentSentence;
                isTyping = false;
                return;
            }

            if (lines.Count == 0)
            {
                EndDialogue();
                return;
            }

            DialogueLine line = lines.Dequeue();
            currentSentence = line.text;
            
            // Optional: Change portrait per-line if provided in the struct
            if (line.emotionPortrait != null && npcPortrait != null)
            {
                npcPortrait.sprite = line.emotionPortrait;
            }

            StartCoroutine(TypeSentence(line.text));
        }

        private IEnumerator TypeSentence(string sentence)
        {
            dialogueText.text = "";
            isTyping = true;
            
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSecondsRealtime(typingSpeed);
            }
            
            isTyping = false;
        }

        private void EndDialogue()
        {
            dialogueBox.SetActive(false);
            Time.timeScale = 1f; // Unfreeze
            Debug.Log("Dialogue Ended.");
        }
    }
}
