using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RaceCountdown : MonoBehaviour
{
    private float countdownTime = 3f;
    [SerializeField] private GameObject[] cars;

    void Start()
    {
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        FreezeCars(true);
        for (int i = (int)countdownTime; i > 0; i--)
        {
            Debug.Log(i);
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("GO!");
        yield return new WaitForSeconds(0.5f);
        FreezeCars(false);
    }

    void FreezeCars(bool freeze)
    {
        foreach (GameObject car in cars)
        {
            Rigidbody rb = car.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = freeze;
            }
        }
    }
}