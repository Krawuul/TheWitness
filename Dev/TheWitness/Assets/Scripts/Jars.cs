using Manager;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Jars : MonoBehaviour
{
    [SerializeField] private GameObject[] jars;
    [SerializeField] private GameObject tongue;
    private bool look = false;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameManager.instance.GameCheckPoint >= 16)
        {
            jars[0].SetActive(true);
            jars[1].SetActive(true);
            tongue.transform.LookAt(Camera.main.transform.position);
        }

        if (GameManager.instance.GameCheckPoint == 16 && !look)
        {
            Vector3 a = Camera.main.transform.position;
            Vector3 b = jars[1].transform.position;
            var c = Physics.RaycastAll(a, b - a, 1);
            if (c.Length == 2 && c[1].collider.gameObject == jars[1])
            {
                SubtitleManager.instance.InvokeSubTitle("E16", "The Phone");
                look = true;
            }
        }
        if (look)
        {
            Camera.main.transform.LookAt(jars[1].transform.position);
            GameManager.instance.Player.Interacting = true;
            StartCoroutine(GameManager.instance.EndGame());
        }
    }
}