using System;
using UnityEngine;
using UnityEngine.AI;

namespace Behaviors
{
    public class EnemyActionsBehaviour : MonoBehaviour
    {
        public Transform gameObjectAgent;
        private NavMeshAgent _gameObjectAgent;
        private Animator _animator;
        private string _currentState;
        private Vector3 _moveVector;
       
        
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
            _startingPosition = transform.position;
            GameManager.OnVictory -= StopFollowingPlayer;
            GameManager.OnVictory += StopFollowingPlayer;
            GameManager.OnRestart -= Restart;
            GameManager.OnRestart += Restart;
            _animator = GetComponent<Animator>();
            _gameObjectAgent = gameObjectAgent.GetComponent<NavMeshAgent>();

        }

        private void Restart()
        {
            transform.position = _startingPosition;
            gameObjectAgent.position = _startingPosition;
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
            
            if (_moveVector.x > _startingPosition.x )
            {
                ChangeAnimationState(WALKRIGHT);
            }
            else if (_moveVector.x < _startingPosition.x)
            {
                ChangeAnimationState(WALKLEFT);  
            }
            

            if (_gameObjectAgent.remainingDistance < 2)
            {
                //ChangeAnimationState(IDDLE);
                _animator.speed = 0;
            }
            _startingPosition = transform.position;
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