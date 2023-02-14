using System.Collections;
using Rewired;
using Unity.VisualScripting;
using UnityEngine;

namespace Behaviors
{
    public class PlayerActionsBehaviour : MonoBehaviour
    {

        [SerializeField] 
        public int playerId; // rewired player Id of this character
        public float moveSpeed;
        public bool isPlayerHit, isGameOver;

        
        [SerializeField]
        private float BaseMovementSpeed;
        [SerializeField]
        private Light _lightComponent;
        [SerializeField]
        private Light _lightInRobotComponent;
        [SerializeField]
        private float BaseRangeLight;
        
        private Player _player;
        public int _catsFoundCount;
        private float _moveDirection;
        private Vector3 _moveVector;
        private bool  _isAction, _animationComplete, _isChest;
        private OpenChest OpenChest;
        private Rigidbody _playerRigidBody;

        private string _currentState;
        private Vector2 _startingPosition;
        private Animator _animator;
        /*Animations assigned as const*/
        private const string WALKLEFT = "walkLeft";
        private const string WALKRIGHT = "walkRight";
        private const string WALKUP = "walkUp";
        private const string WALKDOWN = "walkDown";
 
        private HealthSystem _healthSystemBodyComponent;
        /* coroutines*/
        private IEnumerator coroutine;
        
        /*Events*/
        public delegate void StopLightDamage(bool shouldStopDamage);
        public static event StopLightDamage OnStopLightDamage;


        #region MonoBehaviour Functions from unity
        private void Start()
        {
            _healthSystemBodyComponent = GetComponent<HealthSystem>();
            _animator = GetComponent<Animator>();
            _player = ReInput.players.GetPlayer(playerId);
            _playerRigidBody = GetComponent<Rigidbody>();
            _startingPosition = _playerRigidBody.position;
           // lampRigidBodyComponent = _lightInRobotComponent.transform.parent.GetComponent<Rigidbody>();
            var spawnPlayers = GameObject.Find("SpawnPlayers");
            if (spawnPlayers)
            {
                transform.parent = spawnPlayers.transform;
            }

            GameManager.OnRestart -= Restart;
            GameManager.OnRestart += Restart;
            HealthSystem.OnHealthAtZero -= TurnOffLights;
            HealthSystem.OnHealthAtZero += TurnOffLights;
            GameManager.OnVictory -= TurnOnLights;
            GameManager.OnVictory += TurnOnLights;
            moveSpeed = BaseMovementSpeed;
            _lightComponent.range = BaseRangeLight;
            _animator.speed = 1;
        }

        private void Restart()
        {
            _playerRigidBody.position = _startingPosition;
            _lightComponent.gameObject.SetActive(true);
            OnStopLightDamage?.Invoke(true);
        }

        private void FixedUpdate()
        {
            if (_moveVector != Vector3.zero)
            {
                ProcessMovementInput();
            }
        }

        private void Update()
        {
            GetInput();
        }

        #endregion
        
        #region Functions created for the actions of the player

        private void GetInput()
        {
            /*moveVector.x = player.GetAxis("Move Horizontal");
            moveVector.y = player.GetAxis("Vertical");*/
            _moveVector = Vector3.zero;
            if (!isGameOver)
            {
                _moveVector.x = Input.GetAxisRaw("Horizontal");
                _moveVector.z = Input.GetAxisRaw("Vertical");    
            }
            
            var movement = new Vector3( _moveVector.x, 0, _moveVector.z).normalized;
            
            if (movement == Vector3.zero)
            {
                _animator.speed = isPlayerHit ? 1 : 0;
                return;
            }
            _animator.speed = 1;
            /*  Quaternion targetRotation = Quaternion.LookRotation(movement);
              
              targetRotation = Quaternion.RotateTowards(
                  _lightInRobotComponent.transform.parent.rotation,
                  targetRotation,
                  360 * Time.fixedDeltaTime);
             
              lampRigidBodyComponent.MovePosition(transform.position);
              lampRigidBodyComponent.MoveRotation(targetRotation);*/
            
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

            //_isAction =_player.GetButtonDown("Action");
            _isAction = Input.GetButtonDown("Fire1");
            if (_isAction && !GameManager.instance.uiOnScreen())
            {
                var lightSwitch = !_lightComponent.gameObject.activeSelf;
                _lightComponent.gameObject.SetActive(lightSwitch);
                OnStopLightDamage?.Invoke(lightSwitch);
                AudioManager.PlaySound(AudioManager.Sound.LightSwitch, false);
            }
            
            _isChest = Input.GetButtonDown("Jump");
            if (_isChest && OpenChest != null)
            {
               var augment = OpenChest.GetAugment();
                Debug.Log(augment._NameAugment);
               switch (augment._NameAugment)
               {
                case "SpeedBoost":
                    moveSpeed=moveSpeed*1.5f;
                    break;
                case "BetterLight":
                    _lightComponent.range *= 2;
                    break;
                case "MoreEnergy":
                    _healthSystemBodyComponent.delayDamage *=1.15f;
                    break;
               }
            }
        }

        private void ProcessMovementInput()
        {
            _playerRigidBody.MovePosition(
                transform.position + _moveVector * (moveSpeed * Time.fixedDeltaTime));
        }

        #endregion
        
        // change the animation and reassign the current one
        public void ChangeAnimationState(string newState)
        {
            _animator.speed = 1;
            AudioManager.PlaySound(AudioManager.Sound.PlayerMove, false);
            if (_currentState == newState) return;
            
            _animator.Play(newState);

            _currentState = newState;
        }

        private void TurnOffLights()
        {
            _lightComponent.gameObject.SetActive(false);
            OnStopLightDamage?.Invoke(false);
        }

        private void TurnOnLights()
        {
            _lightComponent.gameObject.SetActive(true);
            OnStopLightDamage?.Invoke(true);
        }

        void OnTriggerEnter(Collider other){
            if(other.CompareTag("Chest"))
            {
                var openChestComponent = other.GetComponent<OpenChest>();
                openChestComponent.OnEnter();
                OpenChest = openChestComponent;
            }
        }
        void OnTriggerExit(Collider other){
            if(other.CompareTag("Chest")){
                if (OpenChest)
                {
                    OpenChest.OnExit();
                    OpenChest = null;
                }
            }
        }
    }
}
