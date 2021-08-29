using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Vector3 movementVector = new Vector3(10f,10f,10f);
    [Range(0, 1)] [SerializeField] float movementFactor;
    Vector3 startingPos;
    [SerializeField] float period = 2f;

    void Start()
    {
        startingPos = transform.position;    
    }

    // Update is called once per frame
    void Update()
    {
        if(period<=Mathf.Epsilon)
        {
            return;
        }

        float cycles = Time.time / period; //Calculating number of cycles that grows from 0 continuously.
        const float tau = Mathf.PI*2; //6.28 as tau is 2*pi
        Vector3 offset = movementVector * movementFactor;
        float rawSinWave = Mathf.Sin(cycles * tau);
        movementFactor = rawSinWave / 2f + 0.5f;
        transform.position = startingPos + offset;
        movementFactor += 0.1f;
        if(movementFactor==1f)
        {
            movementFactor = -1;
        }
    }
}
