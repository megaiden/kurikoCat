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
        private Vector3 position;
       
        
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
            
            position  = transform.position;
            
            if (Math.Abs(position.x - _startingPosition.x) < Math.Abs(position.z - _startingPosition.z) ) // if the movement in Y axis is more than the one in X we move vertically
            {
                if (_startingPosition.z > position.z)
                {
                    ChangeAnimationState(WALKDOWN);
                }
                else if (_startingPosition.z < position.z)
                {
                    ChangeAnimationState(WALKUP);
                }
            }
            else if (Math.Abs(position.x - _startingPosition.x)  > Math.Abs(position.z - _startingPosition.z))
            {
                if (_startingPosition.x > position.x )
                {
                    ChangeAnimationState(WALKLEFT);
                }
                else if (_startingPosition.x < position.x)
                {
                    ChangeAnimationState(WALKRIGHT);  
                }
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