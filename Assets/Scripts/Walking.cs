using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class Walking : MonoBehaviour
{
    public PlayerController     PC;

    public Transform            LeftFootTarget, RightFootTarget;
    public Transform            LeftHandTarget, rightHandTarget;
    public Transform            Player, Armature, Camera;
    public Rigidbody            PlayerRB;
    public float                StepSpeed, StepHight;


    private Vector3             LeftFootTargetPoint, RightFootTargetPoint;

    private Vector3             Body_position_Last;
    private Vector3             LeftFoot_position_Last;
    private Vector3             RightFoot_position_Last;
    private Vector3             LeftHand_position_Last;
    private Vector3             RightHand_position_Last;
    
    private Vector3             LeftFoot_touch_point;
    private Vector3             RightFoot_touch_point;

    private Quaternion          Body_rotation_Last;
    private Quaternion          LeftFoot_rotation_Last;
    private Quaternion          RightFoot_rotation_Last;
    private Quaternion          LeftHand_rotation_Last;
    private Quaternion          RightHand_rotation_Last;

    private float               Body_progress;
    private float               LeftFoot_progress;
    private float               RightFoot_progress;
    private float               Hands_progress;

    private int                 BodyMoveStage               = 0;
    private int                 LeftFootMoveStage           = 0;
    private int                 RightFootMoveStage          = 1;
    private int                 HandsMoveStage              = 0;

    private int                 StartMoveCounter            = 0;
    private int                 StartDelay                  = 0;
    
    private bool                LeftFootGrounded            = true;
    private bool                RightFootGrounded           = true;

    private bool                RightLegRunOnce             = true;
    private bool                LeftLegRunOnce              = true;
    private bool                BodyRunOnce                 = true;
    private bool                TwoSWaitRunOnce             = true;

    private bool                ResetBeforeMove             = true;
    private bool                ResetBeforeStop             = true;

    private bool                Posing                      = false;

    private bool                LegAdjust                   = false;
    private bool                LegAdjustInit               = true;

    private float               Body_maxSpeed               = 0.03f;
    private float               Foots_maxSpeed              = 0.03f;
    private float               Hands_maxSpeed              = 0.03f;

    public float                Body_Minimum_Speed          = 0.03f;
    public float                Foot_Minimum_Speed          = 0.03f;
    public float                Hands_Minimum_Speed         = 0.03f;

    private Coroutine           checkCoroutine              = null;

    void Start()
    {
        Body_position_Last         = Armature.localPosition;
    }

    void BodyOscilation(){
        
        Vector3 Target, Start;
        Quaternion TargetR, StartR;
        float Body_Angle = 16.8f/(1f + Mathf.Exp(-1.5f*Mathf.Abs(PC.curSpeedX) + 5.4f));
        if(PC.curSpeedX < 0){
            Body_Angle = -Body_Angle;
        }
        float Body_Shift = 0.32f/(1f + Mathf.Exp(-4.5f*PC.curSpeedX + 19f));
        if(         BodyMoveStage == 0 ){
            if(Body_progress <= Body_maxSpeed){
                Body_position_Last = Armature.localPosition;
                Body_rotation_Last = Armature.localRotation;
                HandsMoveStage = 0;
                Hands_progress = 0;
            }
            Start = Body_position_Last;
            StartR = Body_rotation_Last;
            Target = Vector3.zero - Vector3.forward*Body_Shift;

            TargetR = Quaternion.Euler(-90 + Body_Angle, 0, 0);

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
            Target = Vector3.zero - Vector3.forward*Body_Shift;
            Target = Target + Vector3.up*0.06f + Vector3.right*0.02f;

            TargetR = Quaternion.Euler(-90 + Body_Angle, -15, 30);

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
            Target = Vector3.zero - Vector3.forward*Body_Shift;
            
            TargetR = Quaternion.Euler(-90 + Body_Angle, 0, 0);

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
            Target = Vector3.zero - Vector3.forward*Body_Shift;
            Target = Target  + Vector3.up*0.06f - Vector3.right*0.02f;
            
            TargetR = Quaternion.Euler(-90 + Body_Angle, -30, 15);

            Armature.localPosition = CalculatePoint(Start, Target, Body_progress);
            Armature.localRotation = CalculateRotation(StartR, TargetR, Body_progress);
            if(Body_progress >= 1){
                Body_progress = 1;
            }
        } else if(         BodyMoveStage == 4 ){
            if(Body_progress <= Body_maxSpeed){
                Body_position_Last = Armature.localPosition;
                Body_rotation_Last = Armature.localRotation;
                HandsMoveStage = 0;
                Hands_progress = 0;
            }
            Start = Body_position_Last;
            StartR = Body_rotation_Last;
            Target = Vector3.zero + Vector3.up*0.05f;
            if(PC.curSpeedX > 0){
                TargetR = Quaternion.Euler(-85, 0, 0);
            } else if(PC.curSpeedX < 0){
                TargetR = Quaternion.Euler(-95, 0, 0);
            } else {
                TargetR = Quaternion.Euler(-90, 0, 0);
            }
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

            if(PC.curSpeedX > 3){
                LTarget = new Vector3(-0.20f,  0.823f + (Mathf.Abs(PC.curSpeedX) - 1.675f)/13.3f, 0.102f);
                LTargetR = Quaternion.Euler(-21.4f - (Mathf.Abs(PC.curSpeedX) - 2.93f)/0.067f, 28.9f, -14.1f);
            } else {
                LTarget = new Vector3(-0.20f,  0.923f, 0.102f);
                LTargetR = Quaternion.Euler(-22.4f, 28.9f, -14.1f);
            }

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

            if(PC.curSpeedX > 3){
                RTarget = new Vector3(0.20f, 0.823f + (Mathf.Abs(PC.curSpeedX) - 1.675f)/13.3f, 0.102f);
                RTargetR = Quaternion.Euler(-21.4f - (Mathf.Abs(PC.curSpeedX) - 2.93f)/0.067f, -28.9f, 14.1f);
            } else {
                RTarget = new Vector3(0.20f, 0.923f, 0.102f);
                RTargetR = Quaternion.Euler(-22.4f, -28.9f, 14.1f);
            }

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
            if(PC.curSpeedY > 0){
                LTarget = new Vector3(-0.1f, 0.07f, 0) + new Vector3(-PC.curSpeedY/12, 0, PC.curSpeedX/3)*0.7f;
            } else if(PC.curSpeedY < 0) {
                LTarget = new Vector3(-0.1f, 0.07f, 0) + new Vector3(PC.curSpeedY/3, 0, PC.curSpeedX/3)*0.7f;
            } else {
                LTarget = new Vector3(-0.1f, 0.07f, 0) + new Vector3(PC.curSpeedY/12, 0, PC.curSpeedX/3)*0.7f;
            }
            if(PC.curSpeedX > 4) {
                LTarget += new Vector3(0, PC.curSpeedX/4f - 0.75f, -0.4f*PC.curSpeedX/3);
            }
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
                    
                    LeftFoot_touch_point = Lhit.point + 0.06f*Vector3.up;
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

            if(LeftFootTarget.localPosition.z <= 0 && PC.curSpeedX > 0){
                LeftFootTarget.position = LeftFoot_touch_point + Vector3.up*(1f / (7.7f + Mathf.Exp(-20f * (-LeftFootTarget.localPosition.z) + 7f)));
            } else if(LeftFootTarget.localPosition.z >= 0 && PC.curSpeedX < 0){
                LeftFootTarget.position = LeftFoot_touch_point + Vector3.up*(1f / (7.7f + Mathf.Exp(20f * (-LeftFootTarget.localPosition.z) + 7f)));
            } else {
                LeftFootTarget.position = LeftFoot_touch_point;
            }

            if(LeftFootTarget.localPosition.z <= 0 && PC.curSpeedX > 0){
                LeftFootTarget.localRotation = Quaternion.Euler((54f / (1f + Mathf.Exp(-13f * (-LeftFootTarget.localPosition.z) + 3f))) + 2.5f, 0, 0);
            } else {
                LeftFootTarget.localRotation = Quaternion.Euler(0, 0, 0);
            }

            LeftFoot_progress = 0;

            if(PC.curSpeedY > 0 && PC.curSpeedX == 0){
                if(Vector3.Distance(LeftFootTarget.position, Armature.position) >= 0.1){
                    if(RightFootMoveStage != 1 && RightFoot_progress >= 1){
                        RightFootMoveStage = 1;
                        RightFoot_progress = 0;
                    }
                }
            } else if(PC.curSpeedY < 0 && PC.curSpeedX == 0){
                if(LeftFootTarget.localPosition.x > -0.1){
                    if(RightFootMoveStage != 1 && RightFoot_progress >= 1){
                        RightFootMoveStage = 1;
                        RightFoot_progress = 0;
                    }
                }
            } else {
                if(Vector3.Distance(LeftFootTarget.position, Armature.position) >= 0.2){
                    if(RightFootMoveStage != 1 && RightFoot_progress >= 1){
                        RightFootMoveStage = 1;
                        RightFoot_progress = 0;
                    }
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
            if(PC.curSpeedY > 0){
                RTarget = new Vector3(0.1f, 0.07f, 0) + new Vector3(PC.curSpeedY/3, 0, PC.curSpeedX/3)*0.7f;
            } else if(PC.curSpeedY < 0){
                RTarget = new Vector3(0.1f, 0.07f, 0) + new Vector3(-PC.curSpeedY/12, 0, PC.curSpeedX/3)*0.7f;
            } else {
                RTarget = new Vector3(0.1f, 0.07f, 0) + new Vector3(PC.curSpeedY/12, 0, PC.curSpeedX/3)*0.7f;
            }
            if(PC.curSpeedX > 4) {
                RTarget += new Vector3(0, PC.curSpeedX/4f - 0.75f, -0.4f*PC.curSpeedX/3);
            }
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
                    RightFoot_touch_point = Rhit.point + 0.06f*Vector3.up;
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
            if(RightFootTarget.localPosition.z <= 0 && PC.curSpeedX > 0){
                RightFootTarget.position = RightFoot_touch_point + Vector3.up*(1f / (7.7f + Mathf.Exp(-20f * (-RightFootTarget.localPosition.z) + 7f)));
            } else if(RightFootTarget.localPosition.z >= 0 && PC.curSpeedX < 0){
                RightFootTarget.position = RightFoot_touch_point + Vector3.up*(1f / (7.7f + Mathf.Exp(20f * (-RightFootTarget.localPosition.z) + 7f)));
            } else {
                RightFootTarget.position = RightFoot_touch_point;
            }

            if(RightFootTarget.localPosition.z <= 0 && PC.curSpeedX > 0){
                RightFootTarget.localRotation = Quaternion.Euler((54f / (1f + Mathf.Exp(-13f * (-RightFootTarget.localPosition.z) + 3f))) + 2.5f, 0, 0);
            } else {
                RightFootTarget.localRotation = Quaternion.Euler(0, 0, 0);
            }

            RightFoot_progress = 0;
            
            if(PC.curSpeedY > 0 && PC.curSpeedX == 0){
                if(RightFootTarget.localPosition.x < 0.1){
                    if(LeftFootMoveStage != 1 && LeftFoot_progress >= 1){
                        LeftFootMoveStage = 1;
                        LeftFoot_progress = 0;
                    }
                }
            } else if(PC.curSpeedY < 0 && PC.curSpeedX == 0){
                if(Vector3.Distance(RightFootTarget.position, Armature.position) >= 0.1){
                    if(LeftFootMoveStage != 1 && LeftFoot_progress >= 1){
                        LeftFootMoveStage = 1;
                        LeftFoot_progress = 0;
                    }
                }
            } else {
                if(Vector3.Distance(RightFootTarget.position, Armature.position) >= 0.2){
                    if(LeftFootMoveStage != 1 && LeftFoot_progress >= 1){
                        LeftFootMoveStage = 1;
                        LeftFoot_progress = 0;
                    }
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

    private void LandLegs(){

        Vector3 LTarget, LStart;
        Quaternion LTargetR, LStartR;
        RaycastHit Lhit;
        Vector3 RTarget, RStart;
        Quaternion RTargetR, RStartR;
        RaycastHit Rhit;

        if(!RightFootGrounded){
            if(RightFoot_progress > 0 && RightLegRunOnce){
                RightFoot_progress = 0;
                RightLegRunOnce = false;
            }

            if(RightFoot_progress <= Foots_maxSpeed){
                RightFoot_position_Last = RightFootTarget.position;
                RightFoot_rotation_Last = RightFootTarget.localRotation;
                if (Physics.Raycast(RightFootTarget.position + 0.2f*Vector3.up, Vector3.down, out Rhit, Mathf.Infinity))
                {
                    RightFoot_touch_point = Rhit.point + 0.06f*Vector3.up;
                }
            }
                RStart = RightFoot_position_Last;
                RStartR = RightFoot_rotation_Last;
                RTarget = RightFoot_touch_point;
                RTargetR = Quaternion.Euler(0, 0, 0);
                RightFootTarget.position = CalculatePoint(RStart, RTarget, RightFoot_progress);
                RightFootTarget.localRotation = CalculateRotation(RStartR, RTargetR, RightFoot_progress);
            
            if(RightFoot_progress >= 1){
                RightFoot_progress = 1;
                RightFootGrounded = true;
                RightFoot_position_Last = RightFootTarget.position;
                RightFoot_rotation_Last = RightFootTarget.rotation;
            }

            if(RightFoot_progress <= 1 && RightFootMoveStage != 2){
                RightFoot_progress += Foots_maxSpeed;
            }

        }
        
        if(!LeftFootGrounded){
            if(LeftFoot_progress > 0 && LeftLegRunOnce){
                LeftFoot_progress = 0;
                LeftLegRunOnce = false;
            }

            if(LeftFoot_progress <= Foots_maxSpeed){
                LeftFoot_position_Last = LeftFootTarget.position;
                LeftFoot_rotation_Last = LeftFootTarget.localRotation;
                if (Physics.Raycast(LeftFootTarget.position + 0.2f*Vector3.up, Vector3.down, out Lhit, Mathf.Infinity))
                {
                    LeftFoot_touch_point = Lhit.point + 0.06f*Vector3.up;
                }
            }

            LStart = LeftFoot_position_Last;
            LStartR = LeftFoot_rotation_Last;
            LTarget = LeftFoot_touch_point;
            LTargetR = Quaternion.Euler(0, 0, 0);
            LeftFootTarget.position = CalculatePoint(LStart, LTarget, LeftFoot_progress);
            LeftFootTarget.localRotation = CalculateRotation(LStartR, LTargetR, LeftFoot_progress);

            if(LeftFoot_progress >= 1){
                LeftFoot_progress = 1;
                LeftFootGrounded = true;
                LeftFoot_position_Last = LeftFootTarget.position;
                LeftFoot_rotation_Last = RightFootTarget.rotation;
            }

            if(LeftFoot_progress <= 1 && LeftFootMoveStage != 2){
                LeftFoot_progress += Foots_maxSpeed;
            }
        }
    }

    private void LeftFoot_MoveToDefault(){
        Vector3 LTarget, LStart;
        Quaternion LTargetR, LStartR;
        RaycastHit Lhit;
        if( LeftFootMoveStage == 0){
            if(LeftFoot_progress <= Foots_maxSpeed){
                LeftFoot_position_Last = LeftFootTarget.position;
                LeftFoot_rotation_Last = LeftFootTarget.localRotation;
            }

            LeftFootGrounded = false;
            LStart = LeftFoot_position_Last;
            LStartR = LeftFoot_rotation_Last;
            LTarget = LeftFoot_position_Last + Vector3.up*0.1f;
            LTargetR = Quaternion.Euler(0, 0, 0);
            LeftFootTarget.position = CalculatePoint(LStart, LTarget, LeftFoot_progress);
            LeftFootTarget.localRotation = CalculateRotation(LStartR, LTargetR, LeftFoot_progress);

            if(LeftFoot_progress >= 1){
                LeftFoot_progress = 0;
                LeftFootMoveStage = 1;
            }

            
        } else if ( LeftFootMoveStage == 1) {
            if(LeftFoot_progress <= Foots_maxSpeed){
                LeftFoot_position_Last = LeftFootTarget.position;
                LeftFoot_rotation_Last = LeftFootTarget.localRotation;
                if (Physics.Raycast(Armature.position + 0.2f*Vector3.up - 0.15f*Armature.right, Vector3.down, out Lhit, Mathf.Infinity))
                {
                    LeftFoot_touch_point = Lhit.point + 0.06f*Vector3.up - Vector3.up*0.1f - Vector3.forward*0.05f;
                }
            }

            LeftFootGrounded = false;
            LStart = LeftFoot_position_Last;
            LStartR = LeftFoot_rotation_Last;
            LTarget =  LeftFoot_touch_point;
            LTargetR = Quaternion.Euler(0, 0, 0);
            LeftFootTarget.position = CalculatePoint(LStart, LTarget, LeftFoot_progress);
            LeftFootTarget.localRotation = CalculateRotation(LStartR, LTargetR, LeftFoot_progress);

            if(LeftFoot_progress >= 1){
                LeftFoot_progress = 1;
                LeftFootGrounded = true;
                LeftFoot_position_Last = LeftFootTarget.position;
                LeftFoot_rotation_Last = LeftFootTarget.rotation;
            }
        }

        if(LeftFoot_progress <= 1 && LeftFootMoveStage != 2){
            LeftFoot_progress += Foots_maxSpeed;
        }
    }

    private void RightFoot_MoveToDefault(){
        Vector3 RTarget, RStart;
        Quaternion RTargetR, RStartR;
        RaycastHit Rhit;
        if( RightFootMoveStage == 0){
            if(RightFoot_progress <= Foots_maxSpeed){
                RightFoot_position_Last = RightFootTarget.position;
                RightFoot_rotation_Last = RightFootTarget.localRotation;
            }
            

            RightFootGrounded = false;
            RStart = RightFoot_position_Last;
            RStartR = RightFoot_rotation_Last;
            RTarget = RightFoot_position_Last + Vector3.up*0.1f;
            RTargetR = Quaternion.Euler(0, 0, 0);
            RightFootTarget.position = CalculatePoint(RStart, RTarget, RightFoot_progress);
            RightFootTarget.localRotation = CalculateRotation(RStartR, RTargetR, RightFoot_progress);

            if(RightFoot_progress >= 1){
                RightFoot_progress = 0;
                RightFootMoveStage = 1;
            }

            
        } else if ( RightFootMoveStage == 1) {
            if(RightFoot_progress <= Foots_maxSpeed){
                RightFoot_position_Last = RightFootTarget.position;
                RightFoot_rotation_Last = RightFootTarget.localRotation;
                if (Physics.Raycast(Armature.position + 0.2f*Vector3.up + 0.15f*Armature.right, Vector3.down, out Rhit, Mathf.Infinity))
                {
                    RightFoot_touch_point = Rhit.point + 0.06f*Vector3.up - Vector3.up*0.1f - Vector3.forward*0.05f;
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
                RightFoot_progress = 1;
                RightFootGrounded = true;
                RightFoot_position_Last = RightFootTarget.position;
                RightFoot_rotation_Last = RightFootTarget.rotation;
                LegAdjust = false;
                LegAdjustInit = true;
            }
        }

        if(RightFoot_progress <= 1 && RightFootMoveStage != 2){
            RightFoot_progress += Foots_maxSpeed;
        }
    }
    
    private void Body_MoveHigher(){
        Vector3 Target, Start;
        Quaternion TargetR, StartR;
        if(Body_progress <= Body_maxSpeed){
            Body_position_Last = Armature.localPosition;
            Body_rotation_Last = Armature.localRotation;
        }
        Start = Body_position_Last;
        StartR = Body_rotation_Last;
        Target = Vector3.zero + Vector3.up*0.1f;
        TargetR = Quaternion.Euler(-90, 0, 0);
        Armature.localPosition = CalculatePoint(Start, Target, Body_progress);
        Armature.localRotation = CalculateRotation(StartR, TargetR, Body_progress);
        if(Body_progress >= 1){
            Body_progress = 1;
        }
        if(Body_progress <= 1){
            Body_progress += Body_maxSpeed;
        }
    }

    IEnumerator GoToPose()
    {
        // Wait for 2 seconds
        if(TwoSWaitRunOnce){
            yield return new WaitForSeconds(0.2f);
            TwoSWaitRunOnce = false;
        }

        if (!PC.isMoving)
        {

            if(!Posing){
                Body_progress = 0;
                LegAdjust = true;
            }
            ResetBeforeMove = true;
            Posing = true;
            Body_MoveHigher();
        }
        checkCoroutine = null;
    }

    private void CheckIfGrounded(){
        RaycastHit hit;
        if (Physics.Raycast(RightFootTarget.position, Vector3.down, out hit, 10f))
        {
            if(hit.distance > 0.065){
                RightFootGrounded = false;
            }
        }
        if (Physics.Raycast(LeftFootTarget.position, Vector3.down, out hit, 10f))
        {
            if(hit.distance > 0.065){
                LeftFootGrounded = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Body_maxSpeed = 4f*Mathf.Abs(PC.curSpeedX + PC.curSpeedY*2f)/200f;
        Hands_maxSpeed = 4f*Mathf.Abs(PC.curSpeedX + PC.curSpeedY*2f)/200f;
        Foots_maxSpeed = 3f*Mathf.Abs(PC.curSpeedX + PC.curSpeedY*2f)/200f;


        if( Foots_maxSpeed < Foot_Minimum_Speed ){
            Foots_maxSpeed = Foot_Minimum_Speed;
        }
        if( Hands_maxSpeed < Hands_Minimum_Speed ){
            Hands_maxSpeed = Hands_Minimum_Speed;
        }
        if( Body_maxSpeed < Body_Minimum_Speed ){
            Body_maxSpeed = Body_Minimum_Speed;
        }


        if (!PC.isMoving && checkCoroutine == null)
        {
            checkCoroutine = StartCoroutine(GoToPose());
        }
        
        CheckIfGrounded();

        if(!PC.isMoving && RightFootGrounded){
            RightFootTarget.position = RightFoot_position_Last;
            RightFootTarget.rotation = RightFoot_rotation_Last;
            if(Quaternion.Angle(RightFootTarget.rotation, Player.rotation) > 60){
                LegAdjust = true;
            }
        }

        if(!PC.isMoving && LeftFootGrounded){
            LeftFootTarget.position = LeftFoot_position_Last;
            LeftFootTarget.rotation = LeftFoot_rotation_Last;
            if(Quaternion.Angle(LeftFootTarget.rotation, Player.rotation) > 60){
                LegAdjust = true;
            }
        }

        if (LegAdjust){
            if(LegAdjustInit || LeftFootMoveStage == 2 || RightFootMoveStage == 2){
                LeftFoot_progress = 0;
                LeftFootMoveStage = 0;
                RightFoot_progress = 0;
                RightFootMoveStage = 0;
                LegAdjustInit = false;
            }
            LeftFoot_MoveToDefault();
            CheckIfGrounded();
            if(LeftFootGrounded){
                RightFoot_MoveToDefault();
            }
        }

        if(PC.isMoving){
            if(ResetBeforeMove){
                StartMoveCounter = 0;
                BodyMoveStage = 0;
                Body_progress = 0;
                LeftFoot_progress = 0;
                LeftFootMoveStage = 0;
                RightFoot_progress = 0;
                RightFootMoveStage = 1;
                Hands_progress = 0;
                HandsMoveStage = 0;
                RightLegRunOnce = true;
                LeftLegRunOnce = true;
                BodyRunOnce = true;
                TwoSWaitRunOnce = true;
                Posing = false;
                ResetBeforeMove = false;
                LegAdjustInit = true;
            }
            LegAdjust = false;
            ResetBeforeStop = true;
            HandsWalkingOscilation();
            BodyOscilation();
            LeftFootLogic();
            RightFootLogic();
        } else if (!Posing){
            if(ResetBeforeStop){
                BodyMoveStage = 0;
                Body_progress = 0;
                LeftFoot_progress = 0;
                LeftFootMoveStage = 0;
                RightFoot_progress = 0;
                RightFootMoveStage = 0;
                Hands_progress = 0;
                HandsMoveStage = 0;
                ResetBeforeStop = false;
                TwoSWaitRunOnce = true;
            }
            ResetBeforeMove = true;
            BodyMoveStage = 4;
            if(Body_progress > 0 && BodyRunOnce){
                Body_progress = 0;
                BodyRunOnce = false;
            }
            BodyOscilation();
            HandsWalkingOscilation();
            LandLegs();
        }
    }
}
