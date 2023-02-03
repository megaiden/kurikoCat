using UnityEngine;

public class VictoryPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("We are passing through the door. We WIN!!");
            GameManager.instance.Victory();
        }
    }
}
