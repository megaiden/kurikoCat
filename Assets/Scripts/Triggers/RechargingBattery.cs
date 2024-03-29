﻿using System.Collections;
using Behaviors;
using UnityEngine;

namespace Triggers
{
    public class RechargingBattery : MonoBehaviour
    {
        [SerializeField] public HealthBarBehaviour EnergyBar;
        private bool _waitingForRecharging;
        
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("LampKuriko"))
            {
 
                if (!_waitingForRecharging)
                { 
                    other.gameObject.SetActive(true);
                    StartCoroutine(GiveEnergyBySecond(.3f));
                }
            }
        }
        
        private IEnumerator GiveEnergyBySecond(float waitTime)
        {
            _waitingForRecharging = true;
            yield return new WaitForSeconds( waitTime );
            ReceiveEnergy();
            _waitingForRecharging = false;
        }

        private void ReceiveEnergy()
        {
            var currentEnergy = EnergyBar.GetCurrentHealth();
            currentEnergy++;
            EnergyBar.SetHealth(currentEnergy);
        }
    }
}