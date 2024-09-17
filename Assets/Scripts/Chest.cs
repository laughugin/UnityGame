using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public PlayerController PC;
    public float BreathingSpeed = 0.01f;

    private float Y_Shift = 0;
    private bool increase = true;
    private float ActualBreathingSpeed = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Y_Shift > 1){
            increase = false;
        } else if (Y_Shift <= 0){
            increase = true;
        }
        ActualBreathingSpeed = BreathingSpeed - Mathf.Abs(Y_Shift - 0.5f)*BreathingSpeed*1.8f;
        if(increase){
            transform.localPosition += Vector3.up*ActualBreathingSpeed/20;
            Y_Shift += ActualBreathingSpeed;
        } else {
            transform.localPosition -= Vector3.up*ActualBreathingSpeed/20;
            Y_Shift -= ActualBreathingSpeed;
        }
    }
}
