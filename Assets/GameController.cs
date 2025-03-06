using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform Blocks;

    public GameObject RedBlock;
    public GameObject BlueBlock;
    public GameObject YellowBlock;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject R = Instantiate(RedBlock, Blocks);
            GameObject B = Instantiate(BlueBlock, Blocks);
            GameObject Y = Instantiate(YellowBlock, Blocks);

            R.transform.position = new Vector3(Random.Range(-1.2f, 1.2f), Random.Range(0.5f, 1.5f), Random.Range(-2.2f, 2.2f));
            B.transform.position = new Vector3(Random.Range(-1.2f, 1.2f), Random.Range(0.5f, 1.5f), Random.Range(-2.2f, 2.2f));
            Y.transform.position = new Vector3(Random.Range(-1.2f, 1.2f), Random.Range(0.5f, 1.5f), Random.Range(-2.2f, 2.2f));

            R.transform.Rotate(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            B.transform.Rotate(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            Y.transform.Rotate(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        }

        for (int i = 0; i < 20; i++)
        {
            GameObject Y = Instantiate(YellowBlock, Blocks);

            Y.transform.position = new Vector3(Random.Range(-1.1f, 1.1f), Random.Range(0.0f, 1.0f), Random.Range(-2.0f, 2.0f));

            Y.transform.Rotate(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        }
    }
}
