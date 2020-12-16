using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class BookshelfMover : MonoBehaviour
{
    public float speed;
    public GameObject coveredLight;
    [SerializeField]
    private bool isMoved;
    [SerializeField]
    private Vector2 startPos;
    [SerializeField]
    private Vector2 endPos;
    private float journeyLength;
    private float startTime;
    private float maxIntensity;
    private float desirednIntensity;

    private void Start()
    {
        isMoved = false;
        startPos = transform.position;
        endPos = startPos + new Vector2(2, 0);
        maxIntensity = coveredLight.GetComponent<Light2D>().intensity;
        desirednIntensity = 0.0f;
    }

    public void Move()  // +2 если не двигали, -2 если двигали
    {
        isMoved = true;
        startTime = Time.time;
        journeyLength = Vector2.Distance(startPos, endPos);
    }

    private void Update()
    {
        if (isMoved)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector2.Lerp(transform.position, endPos, fracJourney);
            coveredLight.GetComponent<Light2D>().intensity = Mathf.Lerp(coveredLight.GetComponent<Light2D>().intensity, desirednIntensity, fracJourney);
            if (Vector2.Distance(transform.position, endPos) < .001F)
            {
                isMoved = false;
                Vector2 temp = startPos;
                startPos = endPos;
                endPos = temp;
                desirednIntensity = coveredLight.GetComponent<Light2D>().intensity < 0.05f ? maxIntensity : 0.0f;
            }
        }
    }
}
