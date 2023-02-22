using System;
using System.Collections;
using UnityEngine;

namespace Behaviours
{
    public class HearthItemBehaviour : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var otherGameObject = other.gameObject;
            if (otherGameObject.CompareTag("Player"))
            {
                var hearthsPlayerComponent = otherGameObject.GetComponent<HearthsBehaviour>();
                if (hearthsPlayerComponent.GetActivePlayerHearths() < 3)
                {
                    AudioManager.PlaySound(AudioManager.Sound.rightSelection);
                    hearthsPlayerComponent.OnRecoveringHearthPlayer();
                    StartCoroutine(WaitBeforeRemove(1f));
                }
            }
        }
        
        private IEnumerator WaitBeforeRemove(float waitTime)
        {
            yield return new WaitForSeconds( waitTime );
            Destroy(gameObject);        
        }
    }
}
