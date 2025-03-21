using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    public static float Speed;
    public float _Speed;
    public float IntakeExtendDistance;
    public float VSlideExtendDistance;
    public float IntakeUpAngle;
    public float IntakeGrabDistance;
    public float ClawGrabDistance;

    public Vector3 ArmHighAngleX;
    public Vector3 ArmHighAngleZ;
    public Vector3 ArmWallAngleX;
    public Vector3 ArmWallAngleZ;
    public Vector3 ArmPlaceAngleX;
    public Vector3 ArmPlaceAngleZ;

    public ConfigurableJoint IntakeJoint;
    public ConfigurableJoint IntakeRotationJoint;
    public ConfigurableJoint IntakeGrabJoint;
    public ConfigurableJoint VSlideJoint;
    public ConfigurableJoint ArmJointX;
    public ConfigurableJoint ArmJointZ;
    public ConfigurableJoint ClawGrabJoint;

    public Transform Blocks;
    public Transform Specimens;

    public Block StarterBlock;


    private Transform IntakeGrabbed = null;
    private Transform ClawGrabbed = null;

    private int IntakeGrabState = 0; //-1 is eject, 0 is off, 1 is grab

    void Start()
    {
        Speed = _Speed;

        print(StarterBlock);
        print(GameController.Main);

        StarterBlock.SetColor(GameController.Main.PlayerColor);

        ClawGrabbed = StarterBlock.transform;
        ClawGrabJoint.connectedBody = ClawGrabbed.transform.GetComponent<Rigidbody>();

        ClawGrabJoint.xMotion = ConfigurableJointMotion.Locked;
        ClawGrabJoint.yMotion = ConfigurableJointMotion.Locked;
        ClawGrabJoint.zMotion = ConfigurableJointMotion.Locked;

        ClawGrabJoint.angularXMotion = ConfigurableJointMotion.Locked;
        ClawGrabJoint.angularYMotion = ConfigurableJointMotion.Locked;
        ClawGrabJoint.angularZMotion = ConfigurableJointMotion.Locked;
    }

    void FixedUpdate()
    {
        if (!GameController.Main.GameActive) { return; }

        Speed = _Speed;

        if (Input.GetKey(KeyCode.Joystick1Button5))
        {
            IntakeJoint.targetPosition = new Vector3(0, IntakeExtendDistance, 0);

        }//extend intake
        else if (Input.GetKey(KeyCode.Joystick1Button4))
        {
            IntakeJoint.targetPosition = Vector3.zero;
        }//retract intake

        if (Input.GetKey(KeyCode.Joystick2Button5))//extend vslide
        {
            VSlideJoint.targetPosition = new Vector3(0, -VSlideExtendDistance, 0);

        }
        else if (Input.GetKey(KeyCode.Joystick2Button4))//retract vslide
        {
            VSlideJoint.targetPosition = new Vector3(0, 0, 0);
        }

        if (Input.GetKey(KeyCode.Joystick1Button1) || Input.GetKey(KeyCode.Joystick1Button2))//rotate intake up
        {
            IntakeRotationJoint.targetRotation = Quaternion.Euler(IntakeUpAngle, 0, 0);
        }
        else if (Input.GetKey(KeyCode.Joystick1Button3))//Rotate intake down;
        {
            IntakeRotationJoint.targetRotation = Quaternion.Euler(0, 0, 0);
        }

        if (Input.GetKey(KeyCode.Joystick1Button1))
        {
            IntakeGrabState = -1;
        }
        else if (Input.GetKey(KeyCode.Joystick1Button2))
        {
            IntakeGrabState = 0;
        }
        else if (Input.GetKey(KeyCode.Joystick1Button3))
        {
            IntakeGrabState = 1;
        }


        if (IntakeGrabState == 1 && !IntakeGrabbed && !ClawGrabbed)
        {
            Transform Closest = null;
            float ClosestDist = float.MaxValue;

            for (int i = 0; i < Blocks.childCount; i++)
            {
                Transform Child = Blocks.GetChild(i);
                float Dist = Vector3.Distance(IntakeJoint.transform.position, Child.position);

                if (Dist < ClosestDist)
                {
                    ClosestDist = Dist;
                    Closest = Child;
                }
            }

            if (ClosestDist < IntakeGrabDistance && Closest)
            {
                IntakeGrabJoint.connectedBody = Closest.GetComponent<Rigidbody>();
                IntakeGrabJoint.xMotion = ConfigurableJointMotion.Locked;
                IntakeGrabJoint.yMotion = ConfigurableJointMotion.Locked;
                IntakeGrabJoint.zMotion = ConfigurableJointMotion.Locked;

                IntakeGrabJoint.angularXMotion = ConfigurableJointMotion.Locked;
                IntakeGrabJoint.angularYMotion = ConfigurableJointMotion.Locked;
                IntakeGrabJoint.angularZMotion = ConfigurableJointMotion.Locked;
                IntakeGrabbed = Closest;
                IntakeGrabbed.parent = transform;
                IntakeGrabState = 0;
            }
        }
        else if(IntakeGrabState == -1 && IntakeGrabbed)
        {
            IntakeGrabJoint.xMotion = ConfigurableJointMotion.Free;
            IntakeGrabJoint.yMotion = ConfigurableJointMotion.Free;
            IntakeGrabJoint.zMotion = ConfigurableJointMotion.Free;

            IntakeGrabJoint.angularXMotion = ConfigurableJointMotion.Free;
            IntakeGrabJoint.angularYMotion = ConfigurableJointMotion.Free;
            IntakeGrabJoint.angularZMotion = ConfigurableJointMotion.Free;
            IntakeGrabJoint.connectedBody = null;
            IntakeGrabbed.parent = Blocks;
            IntakeGrabbed = null;
            IntakeGrabState = 0;
        }

        if (Input.GetAxis("VDPad") < -0.5f)
        {
            ArmJointX.targetRotation = Quaternion.Euler(Vector3.zero);
            ArmJointZ.targetRotation = Quaternion.Euler(Vector3.zero);
        }

        else if(Input.GetAxis("HDPad") > 0.5f)
        {
            ArmJointX.targetRotation = Quaternion.Euler(ArmPlaceAngleX);
            ArmJointZ.targetRotation = Quaternion.Euler(ArmPlaceAngleZ);
        }
        else if (Input.GetAxis("HDPad") < -0.5f)
        {
            ArmJointX.targetRotation = Quaternion.Euler(ArmWallAngleX);
            ArmJointZ.targetRotation = Quaternion.Euler(ArmWallAngleZ);
        }

        if (Input.GetKey(KeyCode.Joystick2Button0))
        {
            ArmJointX.targetRotation = Quaternion.Euler(ArmHighAngleX);
            ArmJointZ.targetRotation = Quaternion.Euler(ArmHighAngleZ);

            if (!ClawGrabbed)
            {
                Transform Closest = null;
                float ClosestDist = float.MaxValue;

                for (int i = 0; i < Blocks.childCount; i++)
                {
                    Transform Child = Blocks.GetChild(i);
                    float Dist = Vector3.Distance(ClawGrabJoint.transform.position, Child.position);

                    if (Dist < ClosestDist)
                    {
                        ClosestDist = Dist;
                        Closest = Child;
                    }
                }

                if (ClosestDist < ClawGrabDistance)
                {
                    ClawGrabbed = Closest;
                    ClawGrabbed.parent = transform;
                    ClawGrabJoint.connectedBody = ClawGrabbed.GetComponent<Rigidbody>();

                    ClawGrabJoint.xMotion = ConfigurableJointMotion.Locked;
                    ClawGrabJoint.yMotion = ConfigurableJointMotion.Locked;
                    ClawGrabJoint.zMotion = ConfigurableJointMotion.Locked;

                    ClawGrabJoint.angularXMotion = ConfigurableJointMotion.Locked;
                    ClawGrabJoint.angularYMotion = ConfigurableJointMotion.Locked;
                    ClawGrabJoint.angularZMotion = ConfigurableJointMotion.Locked;
                }
            }
        }//close claw and arm up (grab block)
        else if (Input.GetKey(KeyCode.Joystick2Button2) && ClawGrabbed)//open claw (drop block)
        {
            ClawGrabbed.parent = Blocks;
            ClawGrabbed = null;
            ClawGrabJoint.connectedBody = null;

            ClawGrabJoint.xMotion = ConfigurableJointMotion.Free;
            ClawGrabJoint.yMotion = ConfigurableJointMotion.Free;
            ClawGrabJoint.zMotion = ConfigurableJointMotion.Free;

            ClawGrabJoint.angularXMotion = ConfigurableJointMotion.Free;
            ClawGrabJoint.angularYMotion = ConfigurableJointMotion.Free;
            ClawGrabJoint.angularZMotion = ConfigurableJointMotion.Free;
        }
    }

}