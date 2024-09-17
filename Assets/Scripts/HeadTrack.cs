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
        transform.position = Camera.position + Camera.forward*1;
        transform.localRotation = Camera.localRotation;
    }
}
