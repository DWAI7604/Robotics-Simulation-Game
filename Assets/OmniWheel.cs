using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmniWheel : MonoBehaviour
{
    public bool TurnLeftIsForward;

    private ConfigurableJoint WheelJoint;

    void Start()
    {
        WheelJoint = transform.GetComponent<ConfigurableJoint>();

        transform.GetComponent<ConfigurableJoint>().connectedBody = transform.parent.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        ;
        Vector3 InputAxis = new Vector3(0, -Input.GetAxisRaw("Vertical") + Input.GetAxisRaw("Rotational") * (TurnLeftIsForward ? 2 : -2) , -Input.GetAxisRaw("Horizontal"));
        WheelJoint.targetAngularVelocity = InputAxis.normalized * RobotController.Speed;      
    }
}
