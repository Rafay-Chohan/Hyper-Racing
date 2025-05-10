using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class RaceCountdown : MonoBehaviour
{
    private float countdownTime = 3f;
    [SerializeField] private GameObject[] cars;
    [SerializeField] private TextMeshPro flagTextBox;
    [SerializeField] private GameObject[] lights; 

    void Start()
    {
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        FreezeCars(true);
        yield return new WaitForSeconds(1f);
        for (int i = (int)countdownTime; i > 0; i--)
        {
            flagTextBox.text = i.ToString();
            yield return new WaitForSeconds(0.25f);
            flagTextBox.text += ".";
            yield return new WaitForSeconds(0.25f);
            flagTextBox.text += ".";
            yield return new WaitForSeconds(0.25f);
            flagTextBox.text += ".";
            yield return new WaitForSeconds(0.25f);
            // Debug.Log(i);
            // yield return new WaitForSeconds(1f);
        }
        foreach (GameObject light in lights)
        {
            light.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        flagTextBox.text = "GO";
        // Debug.Log("GO!");
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