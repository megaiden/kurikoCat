using Behaviors;
using UnityEngine;
using UnityEngine.AI;


public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    public Rigidbody enemySpriteRigidBody;
    public Transform enemySpritetransform;
    private NavMeshAgent nav;
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
    }

    private void FixedUpdate()
    {
        var _moveVector = new Vector3(transform.position.x,enemySpritetransform.position.y,transform.position.z) ;
        
        //transform.parent.position = transform.position;
        enemySpriteRigidBody.MovePosition(_moveVector);
        
    }
    void Update()
    {
        nav.SetDestination(target.position);
    }

    private void StopLight(bool shouldStop)
    {
        nav.stoppingDistance = shouldStop ? 8f : 0f;
    }
}
