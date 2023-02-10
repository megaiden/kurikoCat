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
            
            if (distance <= 8)
            {
                nav.SetDestination(target.position);
                nav.speed = 2.5f;
            }
            else if(_hasDestinationPatrolSet == false)
            {
                nav.SetDestination(pointsOfPatrol[Random.Range(0,8)].transform.position);
                nav.speed = 1.8f;
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
    
        private void OnTriggerEnter(Collider other)
        {
           /* if(other.CompareTag("Player"))
            {
                nav.isStopped = false;
                nav.ResetPath();
                _animator.speed = 1;
            }*/
        }
        
    }
}