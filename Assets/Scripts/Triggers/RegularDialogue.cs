using Behaviors;
using UnityEngine;

namespace Triggers
{
    public class RegularDialogue : MonoBehaviour
    {
        public Dialogue dialogue;
        private bool _dialogueShown;
        private void OnTriggerStay(Collider other)
        {
            var playerGameObject = other.gameObject;
            if (playerGameObject.CompareTag("Player"))
            {
                var showDialogue = playerGameObject.GetComponent<PlayerActionsBehaviour>().showDialogue;
                if (!_dialogueShown && showDialogue)
                {
                    FindObjectOfType<DialogueManager>().StartDialogue(dialogue, true);
                    _dialogueShown = true;
                }
            }
        }
    }
}