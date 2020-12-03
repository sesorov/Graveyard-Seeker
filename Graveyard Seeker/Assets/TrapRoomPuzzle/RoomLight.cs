using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLight : MonoBehaviour
{

    public GameObject image1;
    public GameObject image2;
    public GameObject image3;
    public GameObject image4;
    private float a;
    private float k;

    void Start()
    {
      a = 1;
      k = 0;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K) && a == 1)
        {
          StartCoroutine(LightFor2Sec());
          a += 1;
          k = 3;
        }
        if(Input.GetKeyDown(KeyCode.L) && k<=2)
        {
          StartCoroutine(LightsForHalfSec());
          k += 1;
          a = 2;
        }
    }

    IEnumerator LightFor2Sec()
    {
      image1.SetActive(true);
      image2.SetActive(true);
      image3.SetActive(true);
      image4.SetActive(true);
      yield return new WaitForSeconds(2f);
      image1.SetActive(false);
      image2.SetActive(false);
      image3.SetActive(false);
      image4.SetActive(false);
    }

    IEnumerator LightsForHalfSec()
    {
      image1.SetActive(true);
      image2.SetActive(true);
      image3.SetActive(true);
      image4.SetActive(true);
      yield return new WaitForSeconds(0.5f);
      image1.SetActive(false);
      image2.SetActive(false);
      image3.SetActive(false);
      image4.SetActive(false);
    }
}
