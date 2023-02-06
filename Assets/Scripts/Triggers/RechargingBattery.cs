﻿
using System;
using System.Collections;
using UnityEngine;

namespace Triggers
{
    public class RechargingBattery : MonoBehaviour
    {
        [SerializeField] public HealthBarBehaviour EnergyBar;
        private bool _waitingForRecharging;
        
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Lamp"))
            {
                if (!_waitingForRecharging)
                {
                    StartCoroutine(GiveEnergyBySecond(1));
                }
            }
        }
        
        private IEnumerator GiveEnergyBySecond(float waitTime)
        {
            _waitingForRecharging = true;
            yield return new WaitForSeconds( waitTime );
            ReceiveEnergy(1);
            _waitingForRecharging = false;
        }

        private void ReceiveEnergy(int energy)
        {
            var currentEnergy = EnergyBar.GetCurrentHealth();
            currentEnergy++;
            EnergyBar.SetHealth(currentEnergy);
        }
    }
}