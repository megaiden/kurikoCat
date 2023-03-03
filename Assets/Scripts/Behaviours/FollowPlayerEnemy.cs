using System.Collections;
using Behaviours;
using UnityEngine;
using UnityEngine.AI;

namespace Behaviors
{
    public class FollowPlayerEnemy: MonoBehaviour
    {
        [SerializeField]
        public Transform target;
        [SerializeField]
        public Rigidbody SpriteRigidBody;
        [SerializeField]
        public Transform SpriteTransform;
        [SerializeField]
        private float _closeDistance = 2f;
        [SerializeField]
        private float _awayDistance = 2f;
        [SerializeField]
        public GameObject[] pointsOfPatrol;
        
        private NavMeshAgent nav;
        private Animator _animator;
        private bool _hasDestinationPatrolSet;
        private HearthsBehaviour _hearthsBehaviourComponent;
        private PlayerActionsBehaviour _playerActionsBehaviour;
        private bool _canReceiveDamage = true;
        
        /*Animations assigned as const*/
        private const string GETHIT = "getHit";
        
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
            _animator = SpriteTransform.GetComponent<Animator>();
            _hearthsBehaviourComponent = target.GetComponent<HearthsBehaviour>();
            _playerActionsBehaviour = target.GetComponent<PlayerActionsBehaviour>();
        }
    
        private void FixedUpdate()
        {
            //transform.parent.position = transform.position;
            SpriteRigidBody.MovePosition(transform.position);
        }
    
        private void Update()
        {
            var vectorPlayer = target.position;
            var vectorEnemy = transform.position;

            var distance = Vector3.Distance(vectorPlayer, vectorEnemy);
            
            if (distance <= 7)
            {
                nav.SetDestination(vectorPlayer);
                nav.speed = 2f;
            }
            else if(_hasDestinationPatrolSet == false)
            {
                nav.SetDestination(pointsOfPatrol[Random.Range(0,8)].transform.position);
                nav.speed = 1.5f;
                _hasDestinationPatrolSet = true;
            }
            else if (nav.remainingDistance <= 1)
            {
                _hasDestinationPatrolSet = false;
            }

 
        }
    
        private void StopLight(bool shouldStop)
        {
            nav.stoppingDistance = shouldStop ? _closeDistance :_awayDistance;
        }
    
        private void OnTriggerStay(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                if (_canReceiveDamage)
                {
                    StartCoroutine(ReduceHealthBySecond(3));
                }
            }
        }
        
        
        private IEnumerator ReduceHealthBySecond(float waitTime)
        {
            _canReceiveDamage = false;
            _playerActionsBehaviour.isPlayerHit = true;
            _playerActionsBehaviour.ChangeAnimationState(GETHIT);
            _hearthsBehaviourComponent.OnLosingHearth();
            yield return new WaitForSeconds( waitTime );
            _canReceiveDamage = true;
            _playerActionsBehaviour.isPlayerHit = false;
        }
        
    }
}