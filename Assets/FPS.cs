using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPS : MonoBehaviour
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
        Display.text = ((int)(1 / Time.unscaledDeltaTime)).ToString();
    }
}
