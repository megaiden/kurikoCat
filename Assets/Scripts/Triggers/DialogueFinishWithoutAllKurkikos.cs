using Behaviors;
using UnityEngine;

namespace Triggers
{
    public class DialogueFinishWithoutAllKurkikos : MonoBehaviour
    {
        public Dialogue dialogue;
        private void OnTriggerEnter(Collider other)
        {
            var playerGameObject = other.gameObject;
            if (playerGameObject.CompareTag("Player"))
            {
                var catsFound = playerGameObject.GetComponent<PlayerActionsBehaviour>()._catsFoundCount;
                if (catsFound < 4)
                {
                    FindObjectOfType<DialogueManager>().StartDialogue(dialogue, true);
                }
            }
        }
    }
}