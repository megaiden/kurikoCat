using Behaviors;
using UnityEngine;
using UnityEngine.AI;


public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    public Rigidbody SpriteRigidBody;
    public Transform SpriteTransform;
    private NavMeshAgent nav;
    private float _closeDistance = 2f;
    private float _awayDistance = 2f;
    private Animator _animator;
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
        if (nav.remainingDistance <= 8)
        {
            _animator.speed = 1;
            nav.SetDestination(target.position);
        }
        else
        {
            nav.isStopped = true;
            _animator.speed = 0;
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
            nav.isStopped = false;
            nav.ResetPath();
            _animator.speed = 1;
        }
    }
}
