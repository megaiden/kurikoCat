using System;
using System.Collections;
using Rewired;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviors
{
    public class PlayerActionsBehaviour : MonoBehaviour
    {

        [SerializeField] 
        public int playerId; // rewired player Id of this character
        public float moveSpeed;
        public bool isPlayerHit, isGameOver;
        public bool showDialogue;
        public bool clickNextDialogue;
        [SerializeField] 
        public Camera mainCamera;
        
        [SerializeField]
        private float BaseMovementSpeed;
        [SerializeField]
        public Light lightComponent;
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
        private const string LOOKSIDES = "lookingSides";
        private const string CONFUSED = "confused";
        private const string CHESTHIT = "chestHit";
        private const string CELEBRATE = "celebrate";
 
        private HealthSystem _healthSystemBodyComponent;
        private Vector2 _startPos;
        private Vector2 _direction;
        private bool _startEventEnded, _shouldStopMoving;

        /* coroutines*/
        private IEnumerator coroutine;
        
        /*Events*/
        public delegate void StopLightDamage(bool shouldStopDamage);
        public static event StopLightDamage OnStopLightDamage;


        #region MonoBehaviour Functions from unity
        private void Start()
        {
            AudioManager.PlayStageMusic(AudioManager.Sound.StageMusic, true);
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
            lightComponent.range = BaseRangeLight;
            _animator.speed = 1;
            EventStart();
        }

        private void Restart()
        {
            _playerRigidBody.position = _startingPosition;
            lightComponent.gameObject.SetActive(true);
            OnStopLightDamage?.Invoke(true);
        }

        private void FixedUpdate()
        {
            // if (_moveVector != Vector3.zero)
            // {
            //     ProcessMovementInput();
            // }
            
            if (_moveVector != Vector3.zero)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    _shouldStopMoving = true;
                    return;
                }

                _shouldStopMoving = false;
                transform.position = Vector3.Lerp( transform.position, _moveVector, Time.fixedDeltaTime*.8f);
            }
        }

        private void Update()
        {

            if (_startEventEnded)
            {
                GetInput();
            }

        }

        #endregion

        #region Coroutines
        private IEnumerator EventThenWait()
        {
            yield return new WaitForSeconds( 3f );
            AudioManager.PlaySound(AudioManager.Sound.CatMeow5);
            yield return new WaitForSeconds( 1f );
            ChangeAnimationState(LOOKSIDES);
            yield return new WaitForSeconds( 2f );
            ChangeAnimationState(CONFUSED);
            showDialogue = true;
            yield return new WaitUntil( () => clickNextDialogue );
            clickNextDialogue = false;
            ChangeAnimationState(CHESTHIT);
            yield return new WaitUntil( () => clickNextDialogue );
            clickNextDialogue = false;
            ChangeAnimationState(CELEBRATE); 
            yield return new WaitUntil( () => clickNextDialogue );
            ChangeAnimationState(WALKDOWN); 
            _animator.speed = 0;
        }

        public void ClickContinueDialogue()
        {
            clickNextDialogue = true;
        }

        private IEnumerator WaitForAnimation() 
        {
            yield return StartCoroutine(EventThenWait());
            _startEventEnded = true;
        }
        

        #endregion
        
        #region Functions created for the actions of the player

        private void EventStart()
        {
            StartCoroutine(WaitForAnimation());
        }
        
        private void GetInput()
        {                
            var position = transform.position;
            
           if (isGameOver) return;
           
            //Check if the left mouse button was clicked
            if (Input.GetMouseButton(0))
            {
                // Check if the mouse was clicked over a UI element
               if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    var screenPosition = new Vector3(Input.mousePosition.x,Input.mousePosition.y, Input.mousePosition.z);
                    var worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);
                    _moveVector = new Vector3(worldPosition.x, position.y, worldPosition.z);
                }
            }
            
            if (Math.Abs(position.x - _moveVector.x) < .50f && Math.Abs(position.z - _moveVector.z) < .50f )
            {
                _animator.speed = isPlayerHit ? 1 : 0;
                return;
            }
            
            if (_moveVector != Vector3.zero && !isPlayerHit)
            {
                _animator.speed = 1;
                DirectionMovement(_moveVector);
                AudioManager.PlaySound(AudioManager.Sound.PlayerMove);
            }
            
            if (_shouldStopMoving && !isPlayerHit)
            {
                _animator.speed = 0;
            }

            /*var movement = new Vector3( _moveVector.x, 0, _moveVector.z).normalized;

            if (movement == Vector3.zero)
            {
                _animator.speed = isPlayerHit ? 1 : 0;
                return;
            }*/

            /*  Quaternion targetRotation = Quaternion.LookRotation(movement);
              
              targetRotation = Quaternion.RotateTowards(
                  _lightInRobotComponent.transform.parent.rotation,
                  targetRotation,
                  360 * Time.fixedDeltaTime);
             
              lampRigidBodyComponent.MovePosition(transform.position);
              lampRigidBodyComponent.MoveRotation(targetRotation);*/

            //DirectionMovement(_moveVector);

            //_isAction =_player.GetButtonDown("Action");
            // _isAction = Input.GetButtonDown("Fire1");
            // if (_isAction && !GameManager.instance.uiOnScreen())
            // {
            //     var lightSwitch = !_lightComponent.gameObject.activeSelf;
            //     _lightComponent.gameObject.SetActive(lightSwitch);
            //     OnStopLightDamage?.Invoke(lightSwitch);
            //     AudioManager.PlaySound(AudioManager.Sound.LightSwitch, false);
            // }
            //
            // _isChest = Input.GetButtonDown("Jump");
            // if (_isChest && OpenChest != null)
            // {
            //     var augment = OpenChest.GetAugment();
            //     Debug.Log(augment._NameAugment);
            //     switch (augment._NameAugment)
            //     {
            //         case "SpeedBoost":
            //             moveSpeed=moveSpeed*1.5f;
            //             break;
            //         case "BetterLight":
            //             _lightComponent.range *= 2;
            //             break;
            //         case "MoreEnergy":
            //             _healthSystemBodyComponent.delayDamage *=1.15f;
            //             break;
            //     }
            // }
        }

        private void DirectionMovement(Vector3 vectorToMove)
        {
            // switch (vectorToMove.x)
            // {
            //     case > 0 when vectorToMove.z is 0: // movimiento horizontal
            //         ChangeAnimationState(WALKRIGHT);
            //         break;
            //     case < 0 when vectorToMove.z is 0: // movimiento horizontal
            //         ChangeAnimationState(WALKLEFT);
            //         break;
            //     case > 0 when vectorToMove.z is > 0: // movimiento vertical/ horizontal derecha direccion arriba
            //         ChangeAnimationState(WALKUP);
            //         break;
            //     case < 0 when vectorToMove.z is > 0: // movimiento vertical/ horizontal izquierda direccion arriba
            //         ChangeAnimationState(WALKUP);
            //         break;
            //     case > 0 when vectorToMove.z is < 0: // movimiento vertical/ horizontal derecha direccion abajo
            //         ChangeAnimationState(WALKRIGHT);
            //         break;
            //     case < 0 when vectorToMove.z is < 0: // movimiento vertical/ horizontal derecha direccion abajo
            //         ChangeAnimationState(WALKLEFT);
            //         break;
            //     default:
            //         switch (vectorToMove.z)
            //         {
            //             case > 0 when vectorToMove.x is 0: // movimiento arriba
            //                 ChangeAnimationState(WALKUP);
            //                 break;
            //             case < 0 when vectorToMove.x is 0: // movimiento abajo
            //                 ChangeAnimationState(WALKDOWN);
            //                 break;
            //             default:
            //                 _animator.speed = 0;
            //                 break;
            //         }
            //         break;
            // }
            
            var position = transform.position;

            if (Math.Abs(position.x - _moveVector.x) < Math.Abs(position.z - _moveVector.z) ) // if the movement in Y axis is more than the one in X we move vertically
            {
                if (_moveVector.z >= position.z)
                {
                    ChangeAnimationState(WALKUP);
                }
                else if (_moveVector.z < position.z)
                {
                    ChangeAnimationState(WALKDOWN);
                }
            }
            else if (Math.Abs(position.x - _moveVector.x)  > Math.Abs(position.z - _moveVector.z))
            {
                if (_moveVector.x > position.x )
                {
                    ChangeAnimationState(WALKRIGHT);
                }
                else if (_moveVector.x < position.x)
                {
                    ChangeAnimationState(WALKLEFT);  
                }
            }
        }

        #endregion
        
        // change the animation and reassign the current one
        public void ChangeAnimationState(string newState)
        {
            _animator.speed = 1;
         
            if (_currentState == newState) return;
            
            _animator.Play(newState);

            _currentState = newState;
        }

        private void TurnOffLights()
        {
            lightComponent.gameObject.SetActive(false);
            OnStopLightDamage?.Invoke(false);
        }

        private void TurnOnLights()
        {
            lightComponent.gameObject.SetActive(true);
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
