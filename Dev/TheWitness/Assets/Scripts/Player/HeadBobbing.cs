using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeadBobbing : MonoBehaviour
{
    #region Properties

    [SerializeField] private float frequency;
    [SerializeField] private float xAmplitude;
    [SerializeField] private float yAmplitude;

    private float currentFrequency;
    private float currentXAmplitude;
    private float currentYAmplitude;

    private float timer;
    private bool loop =true;

    #endregion

    #region Methods

    private void Start()
    {
        SetModifier(1f, 1f, 1f);
    }

    private void Update()
    {
        if (loop)
        {
            timer += Time.deltaTime * currentFrequency;
        } 
        else
        {
            timer -= Time.deltaTime * currentFrequency;
        }

        if (timer > Mathf.PI)
        {
            loop = false;
        }
        else if (timer <= 0)
        {
            loop = true;
        }

        timer = Mathf.Clamp(timer, 0, Mathf.PI);

        float x = Mathf.Cos(timer + Mathf.PI) * currentXAmplitude;
        float y = Mathf.Sin(timer + Mathf.PI) * currentYAmplitude;

        transform.localPosition = new Vector3(x, y, 0);
    }

    public void SetModifier(float frequencyMod, float xAmplitudeMod, float yAmplitudeMod)
    {
        currentFrequency = frequency * frequencyMod;
        currentXAmplitude = xAmplitude * xAmplitudeMod;
        currentYAmplitude = yAmplitude * yAmplitudeMod;
    }

    #endregion
}
