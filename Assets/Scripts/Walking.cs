using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class Walking : MonoBehaviour
{
    public PlayerController PC;

    public Transform LeftFootTarget, RightFootTarget;
    public Transform LeftHandTarget, rightHandTarget;
    public Transform Player, Armature, Camera;
    public Rigidbody PlayerRB;
    public float StepSpeed, StepHight;


    private Vector3 LeftFootTargetPoint, RightFootTargetPoint;

    private Vector3      Body_position_Last;
    private Vector3  LeftFoot_position_Last;
    private Vector3 RightFoot_position_Last;
    private Vector3  LeftHand_position_Last;
    private Vector3 RightHand_position_Last;
    
    private Vector3  LeftFoot_touch_point;
    private Vector3 RightFoot_touch_point;

    private Quaternion      Body_rotation_Last;
    private Quaternion  LeftFoot_rotation_Last;
    private Quaternion RightFoot_rotation_Last;
    private Quaternion  LeftHand_rotation_Last;
    private Quaternion RightHand_rotation_Last;

    private float       Body_progress;
    private float       LeftFoot_progress;
    private float       RightFoot_progress;
    private float       Hands_progress;

    private int     BodyMoveStage       = 0;
    private int     LeftFootMoveStage    = 0;
    private int     RightFootMoveStage   = 1;
    private int     HandsMoveStage   = 0;
    
    private bool    LeftFootGrounded      = true;
    private bool    RightFootGrounded     = true;

    private float    Body_maxSpeed           = 0.03f;
    private float    Foots_maxSpeed          = 0.03f;
    private float    Hands_maxSpeed           = 0.03f;

    public float    Body_Minimum_Speed           = 0.03f;
    public float    Foot_Minimum_Speed            = 0.03f;
    public float    Hands_Minimum_Speed           = 0.03f;

    // Start is called before the first frame update
    void Start()
    {
        Body_position_Last         = Armature.localPosition;
    }

    void BodyOscilation(){
        
        Vector3 Target, Start;
        Quaternion TargetR, StartR;
        if(         BodyMoveStage == 0 ){
            if(Body_progress <= Body_maxSpeed){
                Body_position_Last = Armature.localPosition;
                Body_rotation_Last = Armature.localRotation;
                HandsMoveStage = 0;
                Hands_progress = 0;
            }
            Start = Body_position_Last;
            StartR = Body_rotation_Last;
            Target = Vector3.zero;
            TargetR = Quaternion.Euler(-90, -90, 90);
            Armature.localPosition = CalculatePoint(Start, Target, Body_progress);
            Armature.localRotation = CalculateRotation(StartR, TargetR, Body_progress);
            if(Body_progress >= 1){
                Body_progress = 1;
            }
        } else if(  BodyMoveStage == 1 ){
            if(Body_progress <= Body_maxSpeed){
                Body_position_Last = Armature.localPosition;
                Body_rotation_Last = Armature.localRotation;
                HandsMoveStage = 1;
                Hands_progress = 0;
            }
            Start = Body_position_Last;
            StartR = Body_rotation_Last;
            Target = Vector3.zero;
            Target = Target + Vector3.up*0.06f + Vector3.right*0.02f;
            TargetR = Quaternion.Euler(-88.5f, -90, 105);
            Armature.localPosition = CalculatePoint(Start, Target, Body_progress);
            Armature.localRotation = CalculateRotation(StartR, TargetR, Body_progress);
            if(Body_progress >= 1){
                Body_progress = 1;
            }
        } else if(  BodyMoveStage == 2 ){
            if(Body_progress <= Body_maxSpeed){
                Body_position_Last = Armature.localPosition;
                Body_rotation_Last = Armature.localRotation;
                HandsMoveStage = 2;
                Hands_progress = 0;
            }
            Start = Body_position_Last;
            StartR = Body_rotation_Last;
            Target = Vector3.zero;
            TargetR = Quaternion.Euler(-90, -90, 90);
            Armature.localPosition = CalculatePoint(Start, Target, Body_progress);
            Armature.localRotation = CalculateRotation(StartR, TargetR, Body_progress);
            if(Body_progress >= 1){
                Body_progress = 1;
            }
        } else if(  BodyMoveStage == 3 ){
            if(Body_progress <= Body_maxSpeed){
                Body_position_Last = Armature.localPosition;
                Body_rotation_Last = Armature.localRotation;
                HandsMoveStage = 3;
                Hands_progress = 0;
            }
            Start = Body_position_Last;
            StartR = Body_rotation_Last;
            Target = Vector3.zero;
            Target = Target  + Vector3.up*0.06f - Vector3.right*0.02f;
            TargetR = Quaternion.Euler(-91.5f, -90, 75);
            Armature.localPosition = CalculatePoint(Start, Target, Body_progress);
            Armature.localRotation = CalculateRotation(StartR, TargetR, Body_progress);
            if(Body_progress >= 1){
                Body_progress = 1;
            }
        }
        if(Body_progress <= 1){
            Body_progress += Body_maxSpeed;
        }
    }

    void HandsWalkingOscilation(){
        
        Vector3     LTarget,     LStart;
        Vector3     RTarget,     RStart;
        Quaternion  LTargetR,    LStartR;
        Quaternion  RTargetR,    RStartR;
        if(         HandsMoveStage == 0 ){
            if(Hands_progress <= Hands_maxSpeed){
                LeftHand_position_Last = LeftHandTarget.localPosition;
                LeftHand_rotation_Last = LeftHandTarget.localRotation;

                RightHand_position_Last = rightHandTarget.localPosition;
                RightHand_rotation_Last = rightHandTarget.localRotation;
            }

            LStart = LeftHand_position_Last;
            LStartR = LeftHand_rotation_Last;

            LTarget = new Vector3(-0.281f, 0.856f, 0.039f);
            LTargetR = Quaternion.Euler(0, 0, 0);

            RStart = RightHand_position_Last;
            RStartR = RightHand_rotation_Last;

            RTarget = new Vector3(0.281f, 0.856f, 0.039f);
            RTargetR = Quaternion.Euler(0, 0, 0);

            LeftHandTarget.localPosition = CalculatePoint(LStart, LTarget, Hands_progress);
            LeftHandTarget.localRotation = CalculateRotation(LStartR, LTargetR, Hands_progress);

            rightHandTarget.localPosition = CalculatePoint(RStart, RTarget, Hands_progress);
            rightHandTarget.localRotation = CalculateRotation(RStartR, RTargetR, Hands_progress);

            if(Hands_progress >= 1){
                Hands_progress = 1;
            }
        } else if(  HandsMoveStage == 1 ){
            if(Hands_progress <= Hands_maxSpeed){
                LeftHand_position_Last = LeftHandTarget.localPosition;
                LeftHand_rotation_Last = LeftHandTarget.localRotation;

                RightHand_position_Last = rightHandTarget.localPosition;
                RightHand_rotation_Last = rightHandTarget.localRotation;
            }

            LStart = LeftHand_position_Last;
            LStartR = LeftHand_rotation_Last;

            LTarget = new Vector3(-0.20f, 0.923f, 0.102f);
            LTargetR = Quaternion.Euler(-22.4f, 28.9f, -14.1f);

            RStart = RightHand_position_Last;
            RStartR = RightHand_rotation_Last;

            RTarget = new Vector3(0.37f, 0.915f, -0.099f);
            RTargetR = Quaternion.Euler(6f, 0f, 28.8f);

            LeftHandTarget.localPosition = CalculatePoint(LStart, LTarget, Hands_progress);
            LeftHandTarget.localRotation = CalculateRotation(LStartR, LTargetR, Hands_progress);

            rightHandTarget.localPosition = CalculatePoint(RStart, RTarget, Hands_progress);
            rightHandTarget.localRotation = CalculateRotation(RStartR, RTargetR, Hands_progress);

            if(Hands_progress >= 1){
                Hands_progress = 1;
            }
        } else if(  HandsMoveStage == 2 ){
            if(Hands_progress <= Hands_maxSpeed){
                LeftHand_position_Last = LeftHandTarget.localPosition;
                LeftHand_rotation_Last = LeftHandTarget.localRotation;

                RightHand_position_Last = rightHandTarget.localPosition;
                RightHand_rotation_Last = rightHandTarget.localRotation;
            }

            LStart = LeftHand_position_Last;
            LStartR = LeftHand_rotation_Last;

            LTarget = new Vector3(-0.281f, 0.856f, 0.039f);
            LTargetR = Quaternion.Euler(0, 0, 0);

            RStart = RightHand_position_Last;
            RStartR = RightHand_rotation_Last;

            RTarget = new Vector3(0.281f, 0.856f, 0.039f);
            RTargetR = Quaternion.Euler(0, 0, 0);

            LeftHandTarget.localPosition = CalculatePoint(LStart, LTarget, Hands_progress);
            LeftHandTarget.localRotation = CalculateRotation(LStartR, LTargetR, Hands_progress);

            rightHandTarget.localPosition = CalculatePoint(RStart, RTarget, Hands_progress);
            rightHandTarget.localRotation = CalculateRotation(RStartR, RTargetR, Hands_progress);

            if(Hands_progress >= 1){
                Hands_progress = 1;
            }
        } else if(  HandsMoveStage == 3 ){
            if(Hands_progress <= Hands_maxSpeed){
                LeftHand_position_Last = LeftHandTarget.localPosition;
                LeftHand_rotation_Last = LeftHandTarget.localRotation;

                RightHand_position_Last = rightHandTarget.localPosition;
                RightHand_rotation_Last = rightHandTarget.localRotation;
            }

            LStart = LeftHand_position_Last;
            LStartR = LeftHand_rotation_Last;
            

            LTarget = new Vector3(-0.37f, 0.915f, -0.099f);
            LTargetR = Quaternion.Euler(6f, 0f, -28.8f);

            RStart = RightHand_position_Last;
            RStartR = RightHand_rotation_Last;

            RTarget = new Vector3(0.20f, 0.923f, 0.102f);
            RTargetR = Quaternion.Euler(-22.4f, -28.9f, 14.1f);

            LeftHandTarget.localPosition = CalculatePoint(LStart, LTarget, Hands_progress);
            LeftHandTarget.localRotation = CalculateRotation(LStartR, LTargetR, Hands_progress);

            rightHandTarget.localPosition = CalculatePoint(RStart, RTarget, Hands_progress);
            rightHandTarget.localRotation = CalculateRotation(RStartR, RTargetR, Hands_progress);

            if(Hands_progress >= 1){
                Hands_progress = 1;
            }
        }
        if(Hands_progress <= 1){
            Hands_progress += Hands_maxSpeed;
        }
    }

    void LeftFootLogic(){
        
        Vector3 LTarget, LStart;
        Quaternion LTargetR, LStartR;
        RaycastHit Lhit;
        if(         LeftFootMoveStage == 0){
            if(LeftFoot_progress <= Foots_maxSpeed){
                LeftFoot_position_Last = LeftFootTarget.localPosition;
                LeftFoot_rotation_Last = LeftFootTarget.localRotation;
                BodyMoveStage = 3;
                Body_progress = 0;
            }
            LeftFootGrounded = false;
            LStart = LeftFoot_position_Last;
            LStartR = LeftFoot_rotation_Last;
            LTarget = new Vector3(-0.1f, 0.07f, 0) + new Vector3(PC.curSpeedY/12, 0, PC.curSpeedX/3)*0.5f;
            LTargetR = Quaternion.Euler(-18, 0, 0);
            LeftFootTarget.localPosition = CalculatePoint(LStart, LTarget, LeftFoot_progress);
            LeftFootTarget.localRotation = CalculateRotation(LStartR, LTargetR, LeftFoot_progress);
            if(LeftFoot_progress >= 1){
                LeftFoot_progress = 1;
            }
        } else if(  LeftFootMoveStage == 1){
            if(LeftFoot_progress <= Foots_maxSpeed){
                LeftFoot_position_Last = LeftFootTarget.position;
                LeftFoot_rotation_Last = LeftFootTarget.localRotation;
                BodyMoveStage = 0;
                Body_progress = 0;
                if (Physics.Raycast(LeftFootTarget.position + 0.2f*Vector3.up, Vector3.down, out Lhit, Mathf.Infinity))
                {
                    
                    LeftFoot_touch_point = Lhit.point;
                }
            }
            LeftFootGrounded = false;
            LStart = LeftFoot_position_Last;
            LStartR = LeftFoot_rotation_Last;
            LTarget = LeftFoot_touch_point;
            LTargetR = Quaternion.Euler(0, 0, 0);
            LeftFootTarget.position = CalculatePoint(LStart, LTarget, LeftFoot_progress);
            LeftFootTarget.localRotation = CalculateRotation(LStartR, LTargetR, LeftFoot_progress);
            if(LeftFoot_progress >= 1){
                LeftFootMoveStage = 2;
                LeftFoot_progress = 0;
                RightFootMoveStage = 0;
                RightFoot_progress = 0;
            }
        } else if(  LeftFootMoveStage == 2 ){

            LeftFootGrounded = true;

            LeftFootTarget.position = LeftFoot_touch_point;

            if(LeftFootTarget.localPosition.z <= 0){
                LeftFootTarget.localRotation = Quaternion.Euler(Mathf.Exp(0.55f * (-LeftFootTarget.localPosition.z)) - 1, 0, 0);
            } else {
                LeftFootTarget.localRotation = Quaternion.Euler(0, 0, 0);
            }

            LeftFoot_progress = 0;

            if(Vector3.Distance(LeftFootTarget.position, Armature.position) >= 0.2){
                Debug.Log(LeftFootMoveStage + "   " + RightFootMoveStage);
                if(RightFootMoveStage != 1 && RightFoot_progress >= 1){
                    RightFootMoveStage = 1;
                    RightFoot_progress = 0;
                }
            }

        }
        if(LeftFoot_progress <= 1 && LeftFootMoveStage != 2){
            LeftFoot_progress += Foots_maxSpeed;
        }
    }

    void RightFootLogic(){
        
        Vector3 RTarget, RStart;
        Quaternion RTargetR, RStartR;
        RaycastHit Rhit;
        if(         RightFootMoveStage == 0){
            if(RightFoot_progress <= Foots_maxSpeed){
                RightFoot_position_Last = RightFootTarget.localPosition;
                RightFoot_rotation_Last = RightFootTarget.localRotation;
                BodyMoveStage = 1;
                Body_progress = 0;
            }
            RightFootGrounded = false;
            RStart = RightFoot_position_Last;
            RStartR = RightFoot_rotation_Last;
            RTarget = new Vector3(0.1f, 0.07f, 0) + new Vector3(PC.curSpeedY/12, 0, PC.curSpeedX/3)*0.5f;
            RTargetR = Quaternion.Euler(-18, 0, 0);
            RightFootTarget.localPosition = CalculatePoint(RStart, RTarget, RightFoot_progress);
            RightFootTarget.localRotation = CalculateRotation(RStartR, RTargetR, RightFoot_progress);
            if(RightFoot_progress >= 1){
                RightFoot_progress = 1;
            }
        } else if(  RightFootMoveStage == 1){
            if(RightFoot_progress <= Foots_maxSpeed){
                RightFoot_position_Last = RightFootTarget.position;
                RightFoot_rotation_Last = RightFootTarget.localRotation;
                BodyMoveStage = 2;
                Body_progress = 0;
                if (Physics.Raycast(RightFootTarget.position + 0.2f*Vector3.up, Vector3.down, out Rhit, Mathf.Infinity))
                {
                    RightFoot_touch_point = Rhit.point;
                }
            }
            RightFootGrounded = false;
            RStart = RightFoot_position_Last;
            RStartR = RightFoot_rotation_Last;
            RTarget = RightFoot_touch_point;
            RTargetR = Quaternion.Euler(0, 0, 0);
            RightFootTarget.position = CalculatePoint(RStart, RTarget, RightFoot_progress);
            RightFootTarget.localRotation = CalculateRotation(RStartR, RTargetR, RightFoot_progress);
            if(RightFoot_progress >= 1){
                RightFootMoveStage = 2;
                RightFoot_progress = 0;
                LeftFootMoveStage = 0;
                LeftFoot_progress = 0;
            }
        } else if(  RightFootMoveStage == 2 ){

            RightFootGrounded = true;

            RightFootTarget.position = RightFoot_touch_point;

            if(RightFootTarget.localPosition.z <= 0){
                RightFootTarget.localRotation = Quaternion.Euler(Mathf.Exp(0.55f * (-RightFootTarget.localPosition.z)) - 1, 0, 0);
            } else {
                RightFootTarget.localRotation = Quaternion.Euler(0, 0, 0);
            }

            RightFoot_progress = 0;

            if(Vector3.Distance(RightFootTarget.position, Armature.position) >= 0.2){
                Debug.Log(LeftFootMoveStage + "   " + RightFootMoveStage);
                if(LeftFootMoveStage != 1 && LeftFoot_progress >= 1){
                    LeftFootMoveStage = 1;
                    LeftFoot_progress = 0;
                }
            }

        }
        if(RightFoot_progress <= 1 && RightFootMoveStage != 2){
            RightFoot_progress += Foots_maxSpeed;
        }
    }

    private Vector3 CalculatePoint(
        Vector3   start, 
        Vector3   end,
        float       progress
        )
    {

        // Clamp progress between 0 and 1
        progress = Mathf.Clamp01(progress);

        // Interpolate the localPosition based on the smoothed progress
        Vector3 interpolatedPosition = Vector3.Lerp(start, end, progress);

        return interpolatedPosition;
    }
    private Quaternion CalculateRotation(
        Quaternion   start, 
        Quaternion   end,
        float       progress
        )
    {

        // Clamp progress between 0 and 1
        progress = Mathf.Clamp01(progress);

        // Interpolate the localRotation based on the smoothed progress
        Quaternion interpolatedRotation = Quaternion.Lerp(start, end, progress);

        return interpolatedRotation;
    }

    // Update is called once per frame
    void Update()
    {
        Body_maxSpeed = 3f*Mathf.Abs(PC.curSpeedX + PC.curSpeedY)/200f;
        if( Body_maxSpeed < Body_Minimum_Speed ){
            Body_maxSpeed = Body_Minimum_Speed;
        }

        Hands_maxSpeed = 3f*Mathf.Abs(PC.curSpeedX + PC.curSpeedY)/200f;
        if( Hands_maxSpeed < Hands_Minimum_Speed ){
            Hands_maxSpeed = Hands_Minimum_Speed;
        }

        Foots_maxSpeed = 3f*Mathf.Abs(PC.curSpeedX + PC.curSpeedY)/200f;
        if( Foots_maxSpeed < Foot_Minimum_Speed ){
            Foots_maxSpeed = Foot_Minimum_Speed;
        }

        if(PC.isMoving){
            HandsWalkingOscilation();
            BodyOscilation();
            
            LeftFootLogic();
            RightFootLogic();
        }
    }
}
