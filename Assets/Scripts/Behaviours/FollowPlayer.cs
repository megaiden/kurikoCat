using System;
using Behaviors;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace Behaviours
{
    public class FollowPlayer : MonoBehaviour
    {
        [SerializeField] public GameObject catsFound;
        public Transform target;
        public Rigidbody SpriteRigidBody;
        public Transform SpriteTransform;
        private NavMeshAgent nav;
        private float _closeDistance = 2f;
        private float _awayDistance = 2f;
        private Animator _animator;
        private PlayerActionsBehaviour _playerActionsBehaviourComponent;
        private TextMeshProUGUI _catsFoundText;
        private void OnEnable()
        {
            PlayerActionsBehaviour.OnStopLightDamage += StopLight;
        }
        
        private void OnDisable()
        {
            PlayerActionsBehaviour.OnStopLightDamage -= StopLight;
        }
    
        void Start()
        {
            nav = GetComponent<NavMeshAgent>();
            _catsFoundText = catsFound.GetComponent<TextMeshProUGUI>();
            _animator = SpriteTransform.GetComponent<Animator>();
            _playerActionsBehaviourComponent = target.GetComponent<PlayerActionsBehaviour>();
        }

        private void FixedUpdate()
        {
            //transform.parent.position = transform.position;
            SpriteRigidBody.MovePosition(transform.position);
        }

        private void Update()
        {
            if (nav.remainingDistance <= 5.5f)
            {
                _animator.speed = nav.remainingDistance <= 2? 0 : 1;
                nav.SetDestination(target.position);
            }
            else
            {
                _animator.speed = 0;
                nav.isStopped = true;
            }
            
        }

        private void StopLight(bool shouldStop)
        {
            nav.stoppingDistance = shouldStop ? _closeDistance :_awayDistance;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))  
            {
    
                _playerActionsBehaviourComponent._catsFoundCount++;
                _catsFoundText.text = _playerActionsBehaviourComponent._catsFoundCount.ToString();
                
                nav.isStopped = false;
                _animator.speed = 1;
                nav.ResetPath();

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _playerActionsBehaviourComponent._catsFoundCount--;
                _catsFoundText.text = _playerActionsBehaviourComponent._catsFoundCount.ToString();
            }
        }
    }
}
