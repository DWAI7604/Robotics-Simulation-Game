using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Material Yellow;
    public Material Red;
    public Material Blue;

    public GameObject Hook;

    public bool HasHook = false;

    public string Color = "Yellow";

    public void SetColor(string NewColor)
    {
        transform.GetComponent<MeshRenderer>().material = (NewColor == "Yellow" ? Yellow : NewColor == "Red" ? Red : Blue);
        Color = NewColor;
    }

    public void Convert(int HookCount)
    {
        HasHook = true;
        Hook.SetActive(true);

        transform.rotation = Quaternion.Euler(0, -90, 90);
        transform.position = new Vector3(4.5f - HookCount * 0.15f, 0.5f, -4.9f);
    }
}
