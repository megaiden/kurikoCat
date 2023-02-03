using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class electricManager : MonoBehaviour
{
    public float MinTime;
    public float MaxTime;
    public float dischargeDuration;
    private float currentTime;
    SpriteRenderer spriteR;
    public Light myLight;
    private bool isDischarging = false;
    // Start is called before the first frame update
    void Start()
    {
        spriteR = GetComponent<SpriteRenderer>();
        StartCoroutine(Discharge());        
    }

   IEnumerator Discharge()
    {
        while (true)
        {
            if (!isDischarging)
            {

                isDischarging = true;
                for (int i = 0; i < Random.Range(1,3); i++)
                {
                    spriteR.enabled = true;
                    myLight.enabled = true;
                    yield return new WaitForSeconds(dischargeDuration);
                    spriteR.enabled = false;
                    myLight.enabled = false;
                    yield return new WaitForSeconds(dischargeDuration);
                }
                
                isDischarging = false;
            }
            yield return new WaitForSeconds(Random.Range(MinTime,MaxTime));
        }
    }
}
