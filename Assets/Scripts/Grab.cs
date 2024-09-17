using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Grab : MonoBehaviour
{
    public Rig Grab_rig;
    public Transform Aim;

    void Start()
    {
        Aim.localRotation = Quaternion.Euler(-129.3f, 80.8f, -56.4f);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("heh");
            if(Grab_rig.weight < 1){
                Grab_rig.weight += 0.02f;
            }
        } else {
            if(Grab_rig.weight > 0){
                Grab_rig.weight -= 0.02f;
            }
        }
    }
}
