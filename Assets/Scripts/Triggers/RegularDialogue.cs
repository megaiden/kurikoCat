using UnityEngine;

namespace Triggers
{
    public class RegularDialogue : MonoBehaviour
    {
        public Dialogue dialogue;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                FindObjectOfType<DialogueManager>().StartDialogue(dialogue, true, "", DialogueManager.ActionType.None);
            }
        }
    }
}