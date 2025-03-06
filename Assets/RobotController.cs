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

    public Vector3 ArmHighAngle;
    public Vector3 ArmWallAngle;
    public Vector3 ArmPlaceAngle;

    public ConfigurableJoint IntakeJoint;
    public ConfigurableJoint IntakeGrabJoint;
    public ConfigurableJoint VSlideJoint;
    public ConfigurableJoint ArmJoint;
    public ConfigurableJoint ClawGrabJoint;

    public Transform Blocks;
    public Transform Specimens;


    private Transform IntakeGrabbed = null;
    private Transform ClawGrabbed = null;

    private int GrabState = 0; //-1 is eject, 0 is off, 1 is intake

    private void Start()
    {
        Speed = _Speed;
    }

    private void FixedUpdate()
    {
        Speed = _Speed;

        if (Input.GetKey(KeyCode.Joystick1Button5))
        {
            IntakeJoint.targetPosition = new Vector3(0, 0, -IntakeExtendDistance);

        }
        else if (Input.GetKey(KeyCode.Joystick1Button4))
        {
            IntakeJoint.targetPosition = Vector3.zero;
        }

        if (Input.GetKey(KeyCode.Joystick2Button5))
        {
            VSlideJoint.targetPosition = new Vector3(0, -VSlideExtendDistance, 0);

        }
        else if (Input.GetKey(KeyCode.Joystick2Button4))
        {
            VSlideJoint.targetPosition = new Vector3(0, 0, 0);
        }

        if (Input.GetKey(KeyCode.Joystick1Button0) || Input.GetKey(KeyCode.Joystick1Button1) || Input.GetKey(KeyCode.Joystick1Button2))
        {
            IntakeJoint.targetRotation = Quaternion.Euler(IntakeUpAngle, 0, 0);
        }
        else if (Input.GetKey(KeyCode.Joystick1Button3))
        {
            IntakeJoint.targetRotation = Quaternion.Euler(0, 0, 0);
        }

        if (Input.GetKey(KeyCode.Joystick1Button1))
        {
            GrabState = 0;
        }
        else if (Input.GetKey(KeyCode.Joystick1Button2) || Input.GetKey(KeyCode.Joystick1Button3))
        {
            GrabState = 1;
        }
        else if (Input.GetKey(KeyCode.Joystick1Button0))
        {
            GrabState = -1;
        }


        if (GrabState == 1 && !IntakeGrabbed)
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
                IntakeGrabbed = Closest;
                GrabState = 0;
            }
        }
        else if(GrabState == -1 && IntakeGrabbed)
        {
            IntakeGrabJoint.xMotion = ConfigurableJointMotion.Free;
            IntakeGrabJoint.yMotion = ConfigurableJointMotion.Free;
            IntakeGrabJoint.zMotion = ConfigurableJointMotion.Free;
            IntakeGrabJoint.connectedBody = null;
            IntakeGrabbed = null;
            GrabState = 0;
        }

        if (Input.GetAxis("VDPad") < -0.5f)
        {
            ArmJoint.targetRotation = Quaternion.Euler(Vector3.zero);
        }

        else if(Input.GetAxis("HDPad") > 0.5f)
        {
            ArmJoint.targetRotation = Quaternion.Euler(ArmPlaceAngle);
        }
        else if (Input.GetAxis("HDPad") < -0.5f)
        {
            ArmJoint.targetRotation = Quaternion.Euler(ArmWallAngle);
        }

        if (Input.GetKey(KeyCode.Joystick2Button2))
        {
            ArmJoint.targetRotation = Quaternion.Euler(ArmHighAngle);

            if (!ClawGrabbed)
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

                if (ClosestDist < IntakeGrabDistance)
                {
                    IntakeGrabbed = Closest;
                    ClawGrabJoint.connectedBody = IntakeGrabbed.GetComponent<Rigidbody>();

                    ClawGrabJoint.xMotion = ConfigurableJointMotion.Locked;
                    ClawGrabJoint.yMotion = ConfigurableJointMotion.Locked;
                    ClawGrabJoint.zMotion = ConfigurableJointMotion.Locked;

                    ClawGrabJoint.angularXMotion = ConfigurableJointMotion.Locked;
                    ClawGrabJoint.angularYMotion = ConfigurableJointMotion.Locked;
                    ClawGrabJoint.angularZMotion = ConfigurableJointMotion.Locked;
                }
            }
        }
        else if (Input.GetKey(KeyCode.Joystick2Button1))
        {
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