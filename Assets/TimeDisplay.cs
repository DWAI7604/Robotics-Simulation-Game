using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeDisplay : MonoBehaviour
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
        Display.text = (Mathf.Round(Mathf.Clamp(120 - Time.time - GameController.Main.StartTime, 0, 120) * 10) / 10).ToString();
    }
}
