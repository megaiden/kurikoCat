﻿using System;
using System.Collections;
using UnityEngine;

namespace Behaviors
{
    public class HealthSystem : MonoBehaviour
    {
        [SerializeField] public int maxHealth;
        [SerializeField] public int currentHealth;
        [SerializeField] public HealthBarBehaviour EnergyBar;
        [SerializeField]public float delayDamage;
        public bool isDead;
        public static Action OnHealthAtZero;
        public static Action OnEnemyGrab;

        private bool _waitingForDamage, _stopLightDamage = true;


        private void Start()
        {
            GameManager.OnRestart -= Restart;
            GameManager.OnRestart += Restart;
            Restart();
        }

        public void Restart()
        {
            isDead = false;
            currentHealth = maxHealth;
            EnergyBar.SetMaxHealth(maxHealth);
        }

        private void OnEnable()
        {
            PlayerActionsBehaviour.OnStopLightDamage += StopLightDamage;
        }
        
        private void OnDisable()
        {
            PlayerActionsBehaviour.OnStopLightDamage -= StopLightDamage;
        }

        private void StopLightDamage(bool shouldStopDamage)
        {
            _stopLightDamage = shouldStopDamage;
        }
        
        private void Update()
        {
            if (isDead)
                return;

            if (!_waitingForDamage && _stopLightDamage)
            {
               switch (currentHealth)
               {
                   case 0:
                       OnHealthAtZero?.Invoke();

                       return;
                   case 1:
                       //giving the last stretch of 10 seconds before going totally dark  
                       StartCoroutine(ReduceHealthBySecond(5));
                       break;
                   default:
                       StartCoroutine(ReduceHealthBySecond(delayDamage));
                       break;
               }
            }
        }

      /*  private void OnCollisionStay(Collision other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                OnEnemyGrab?.Invoke();
                isDead = true;
            }
        }*/

        private IEnumerator ReduceHealthBySecond(float waitTime)
        {
            _waitingForDamage = true;
            if(waitTime == 10f)
                AudioManager.PlaySound(AudioManager.Sound.PlayerOnePercent, false);
            
            yield return new WaitForSeconds( waitTime );
            TakeDamage(1);
            _waitingForDamage = false;
        }

        private void TakeDamage(int damage)
        {
            var currentHealth = EnergyBar.GetCurrentHealth();
            currentHealth -= damage;
            //healthBar.ForEach(x => x.SetHealth(currentHealth));
            EnergyBar.SetHealth(currentHealth);
        }
    }
}