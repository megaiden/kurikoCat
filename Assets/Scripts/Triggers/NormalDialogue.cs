using UnityEngine;

namespace Triggers
{
    public class NormalDialogue : MonoBehaviour
    {
        public Dialogue dialogue;
        private bool _dialogueShown;

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (!_dialogueShown)
                {
                    FindObjectOfType<DialogueManager>().StartDialogue(dialogue, true);
                    _dialogueShown = true;
                }
            }
        }
    }
}