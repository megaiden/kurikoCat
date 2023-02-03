using UnityEngine;

public class AlertLightManager : MonoBehaviour
{
    public float intensity;
    public float speedBlink = 1;
     Light myLight;
    // Start is called before the first frame update
    void Start()
    {
        myLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        myLight.intensity = Mathf.PingPong(Time.time*speedBlink, intensity);
    }
}
