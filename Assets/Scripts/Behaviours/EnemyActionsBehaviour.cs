using UnityEngine;

namespace Behaviors
{
    public class EnemyActionsBehaviour : MonoBehaviour
    {
        public Transform enemy;
        private Animator _animator;
        private string _currentState;
        private Vector3 _moveVector;
        /*Animations assigned as const*/
        private const string WALKLEFT = "walkLeft";
        private const string WALKRIGHT = "walkRight";
        private const string WALKUP = "walkUp";
        private const string WALKDOWN = "walkDown";
        private Vector3 _startingPosition;
        private bool followPlayer = true;

        private void Start()
        {
            GameManager.OnVictory -= StopFollowingPlayer;
            GameManager.OnVictory += StopFollowingPlayer;
            GameManager.OnRestart -= Restart;
            GameManager.OnRestart += Restart;
            _animator = GetComponent<Animator>();
            _startingPosition = transform.position;
        }

        private void Restart()
        {
            transform.position = _startingPosition;
            enemy.position = _startingPosition;
            followPlayer = true;
        }

        private void StopFollowingPlayer()
        {
            followPlayer = false;    
        }

        
        private void Update()
        {
            if (!followPlayer)
                return;

            _moveVector = transform.position;
            switch (_moveVector.x)
            {
                case > 0 when _moveVector.z is 0: // movimiento horizontal
                    ChangeAnimationState(WALKRIGHT);
                    break;
                case < 0 when _moveVector.z is 0: // movimiento horizontal
                    ChangeAnimationState(WALKLEFT);
                    break;
                case > 0 when _moveVector.z is > 0: // movimiento vertical/ horizontal derecha direccion arriba
                    ChangeAnimationState(WALKUP);
                    break;
                case < 0 when _moveVector.z is > 0: // movimiento vertical/ horizontal izquierda direccion arriba
                    ChangeAnimationState(WALKUP);
                    break;
                case > 0 when _moveVector.z is < 0: // movimiento vertical/ horizontal derecha direccion abajo
                    ChangeAnimationState(WALKRIGHT);
                    break;
                case < 0 when _moveVector.z is < 0: // movimiento vertical/ horizontal derecha direccion abajo
                    ChangeAnimationState(WALKLEFT);
                    break;
                default:
                    switch (_moveVector.z)
                    {
                        case > 0 when _moveVector.x is 0: // movimiento arriba
                            ChangeAnimationState(WALKUP);
                            break;
                        case < 0 when _moveVector.x is 0: // movimiento abajo
                            ChangeAnimationState(WALKDOWN);
                            break;
                        default:
                            _animator.speed = 0;
                            break;
                    }
 
                    break;
            }
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