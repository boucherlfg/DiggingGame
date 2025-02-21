using System;
using System.Collections;
using UnityEngine;

public class TextIndicator : MonoBehaviour
{
    [SerializeField] private float time = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.position += Vector3.up * Time.deltaTime / 3;
    }
}
