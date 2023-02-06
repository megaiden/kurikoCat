using System;
using UnityEngine;
using UnityEngine.AI;

namespace Behaviors
{
    public class EnemyActionsBehaviour : MonoBehaviour
    {
        public Transform kurikoCat;
        private NavMeshAgent _gameObjectKurikoCatAgent;
        private Animator _animator;
        private string _currentState;
        private Vector3 _moveVector, _originalPos;
       
        
        /*Animations assigned as const*/
        private const string WALKLEFT = "walkLeft";
        private const string WALKRIGHT = "walkRight";
        private const string WALKUP = "walkUp";
        private const string WALKDOWN = "walkDown";
        private const string IDDLE = "Iddle";
        private Vector3 _startingPosition;
        private bool _followPlayer = true;

        private void Start()
        {
            var position = transform.position;
            _originalPos = position;
            GameManager.OnVictory -= StopFollowingPlayer;
            GameManager.OnVictory += StopFollowingPlayer;
            GameManager.OnRestart -= Restart;
            GameManager.OnRestart += Restart;
            _animator = GetComponent<Animator>();
            _gameObjectKurikoCatAgent = kurikoCat.GetComponent<NavMeshAgent>();
            _startingPosition = position;
        }

        private void Restart()
        {
            transform.position = _startingPosition;
            kurikoCat.position = _startingPosition;
            _followPlayer = true;
        }

        private void StopFollowingPlayer()
        {
            _followPlayer = false;    
        }

        
        private void Update()
        {
            if (!_followPlayer)
                return;
            

            _moveVector  = transform.position;
            // _animator.speed = 0;

            if (_moveVector.x > _originalPos.x )
            {
                ChangeAnimationState(WALKRIGHT);
            }
            else if (_moveVector.x < _originalPos.x)
            {
                ChangeAnimationState(WALKLEFT);  
            }
            
            
            // if (Mathf.Abs(_moveVector.x) > Mathf.Abs(_moveVector.z)) // if we are going more to the sides
            // {
            //     if (_moveVector.x > _originalPos.x )
            //     {
            //         ChangeAnimationState(WALKRIGHT);
            //     }
            //     else if (_moveVector.x < _originalPos.x)
            //     {
            //         ChangeAnimationState(WALKLEFT);  
            //     }
            // }
            // else if (Mathf.Abs(_moveVector.x) < Mathf.Abs(_moveVector.z)) // if we are going more to throw up/down
            // {
            //     if (_moveVector.z > _originalPos.z )
            //     {
            //         ChangeAnimationState(WALKUP);     
            //     }
            //     else if(_moveVector.z < _originalPos.z)
            //     {
            //         ChangeAnimationState(WALKDOWN);
            //     }
            // }

            if (_gameObjectKurikoCatAgent.remainingDistance < 2)
            {
                //ChangeAnimationState(IDDLE);
                _animator.speed = 0;
            }
            _originalPos = transform.position;
        }
        
        // change the animation and reassign the current one
        private void ChangeAnimationState(string newState)
        {
            _animator.speed = 1;
            if (_currentState == newState) return;
            
            _animator.Play(newState);

            _currentState = newState;
        }
        
    }
}