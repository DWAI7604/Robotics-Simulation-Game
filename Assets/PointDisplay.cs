using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointDisplay : MonoBehaviour
{
    TextMeshProUGUI Display;

    // Start is called before the first frame update
    void Start()
    {
        Display = transform.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        Display.text = "Points: " + GameController.Main.Points.ToString();
    }
}
