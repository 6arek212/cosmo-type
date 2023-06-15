using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  this script shows a hint for some time and disable the object
/// </summary>
public class Hint : MonoBehaviour
{
    [SerializeField]
    float showFor = 3f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(startTimer());
    }

    // Update is called once per frame
    void Update() { }

    IEnumerator startTimer()
    {
        yield return new WaitForSeconds(showFor);
        gameObject.SetActive(false);
    }
}
