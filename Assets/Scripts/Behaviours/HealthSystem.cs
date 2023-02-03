using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviors
{
    public class HealthSystem : MonoBehaviour
    {
        [SerializeField] public int maxHealth;
        [SerializeField] public int currentHealth;
        [SerializeField] public HealthBarBehaviour EnergyBar;

        public bool isDead;
        public float DelayDamage;
        [SerializeField] private float baseDelayDamage = 1;

        public static Action OnHealthAtZero;
        public static Action OnEnemyGrab;

        private bool _waitingForDamage, _stopLightDamage = true;


        private void Start()
        {
            DelayDamage = baseDelayDamage;
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
                       StartCoroutine(ReduceHealthBySecond(10));
                       break;
                   default:
                       StartCoroutine(ReduceHealthBySecond(DelayDamage));
                       break;
               }
            }
        }

        private void OnCollisionStay(Collision other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                OnEnemyGrab?.Invoke();
                isDead = true;
            }
        }

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
            currentHealth -= damage;
            //healthBar.ForEach(x => x.SetHealth(currentHealth));
            EnergyBar.SetHealth(currentHealth);
        }
    }
}