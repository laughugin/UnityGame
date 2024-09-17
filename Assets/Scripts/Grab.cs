using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Grab : MonoBehaviour
{
    public Rig Grab_rig;
    public Rig Shouler_rig;
    public Transform Aim;
    public Transform Shouler_Aim;
    public Transform Sight;
    public Transform Guide;

    void Start()
    {
        Aim.localRotation = Quaternion.Euler(-129.3f, 80.8f, -56.4f);
        Shouler_Aim.localRotation = Quaternion.Euler(-6.75f, -15f, 66.4f);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if(Grab_rig.weight < 1){
                Grab_rig.weight += 0.02f;
            }
        } else {
            if(Grab_rig.weight > 0){
                Grab_rig.weight -= 0.02f;
            }
        }

        if (Input.GetMouseButton(0))
        {   
            Shouler_rig.weight = 1f/(1f+Mathf.Exp(-12.15f*Sight.position.y + 43.4f));
        } else {
            if(Shouler_rig.weight > 0){
                Shouler_rig.weight -= 0.02f;
            }
        }
        
        if (Sight.position.y > 3f && Input.GetMouseButton(0) ){
            Guide.localPosition = new Vector3(0.45f + 0.3f - Mathf.Pow((0.9f*Sight.position.y - 3.25f), 2f), 1.172f + Sight.position.y/2.5f - 1.2f, -0.367f);
        } else {
            Guide.localPosition = new Vector3(0.45f, 1.172f, -0.367f);
        }
    }
}
