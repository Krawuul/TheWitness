using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jars : MonoBehaviour
{
    [SerializeField] GameObject[] jars;
    [SerializeField] GameObject tongue;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GameCheckPoint >= 15)
        {
            jars[0].SetActive(true);
            jars[1].SetActive(true);
            tongue.transform.LookAt(Camera.main.transform.position);
        }
    }
}
