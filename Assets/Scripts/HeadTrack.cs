using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTrack : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Transform Camera;

    void Update()
    {
        transform.position = Camera.position + Camera.forward*(0.7f - 0.31f*transform.localPosition.y + 0.717f);
        transform.localRotation = Camera.localRotation;
    }
}
