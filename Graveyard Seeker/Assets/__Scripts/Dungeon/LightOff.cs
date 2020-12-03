using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightOff : MonoBehaviour
{
    public GameObject Player;
    private Light2D roomLight;
    private float speed = 0f;
    private void Start()
    {
        roomLight = Player.GetComponent<Player>().roomLight.GetComponent<Light2D>();
    }
    private void OnTriggerEnter(Collider other)
    {
        speed = 0.2f;
    }
    private void Update()
    {
       roomLight.intensity = Mathf.Lerp(roomLight.intensity, 0.0f, speed * Time.deltaTime);
       if (roomLight.intensity <= 0.05)
           Destroy(gameObject);
    }
}
