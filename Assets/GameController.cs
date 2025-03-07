using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//minor foul is 5 points, major is 15

//3 seconds of holding the wrong teams color gives a minor foul, every 5 seconds past that gives another foul.

public class GameController : MonoBehaviour
{
    public static GameController Main;


    public int Points;
    public int Penalties;

    public float StartTime;//TeleOp lasts for 2 minutes (120 seconds)
    public float ConversionTime;

    public bool GameActive = true;

    public string PlayerColor;

    public Transform Blocks;
    public Transform LowBasket;//4
    public Transform HighBasket;//8
    public Transform BasketPole;
    public Transform LowRung;//6
    public Transform HighRung;//10
    public Transform Tape;
    public Transform OtherTeamTape;
    public Transform Robot;

    public GameObject BlockPrefab;

    public Material Red;
    public Material Blue;

    private Vector2 LowRungPos;
    private Vector2 HighRungPos;

    private bool BlockInArea = false;

    private float LastConversion;

    private int HookCount;

    // Start is called before the first frame update
    void Start()
    {
        Main = this;

        StartTime = Time.time;

        foreach (MeshRenderer M in Tape.GetComponentsInChildren<MeshRenderer>())
        {
            if (M.transform.parent != Tape) { continue; }

            M.material = PlayerColor == "Red" ? Red : Blue;
        }

        foreach (MeshRenderer M in OtherTeamTape.GetComponentsInChildren<MeshRenderer>())
        {
            if (M.transform.parent != OtherTeamTape) { continue; }

            M.material = PlayerColor == "Red" ? Blue : Red;
        }

        foreach (Block block in Tape.GetComponentsInChildren<Block>())
        {
            block.SetColor(PlayerColor);
            block.transform.parent = Blocks;
        }

        foreach (Block block in OtherTeamTape.GetComponentsInChildren<Block>())
        {
            block.SetColor(PlayerColor == "Red" ? "Blue" : "Red");
            block.transform.parent = Blocks;
        }

        for (int i = 0; i < 30; i++)
        {
            for (int v = 1; v <= (i < 15 ? 3 : 1); v++)
            {
                GameObject B = Instantiate(BlockPrefab, Blocks);
                B.transform.position = new Vector3(Random.Range(-1f, 1f), Random.Range(0.5f, 1f), Random.Range(-1.8f, 1.8f));
                B.transform.rotation = Quaternion.Euler(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));

                B.GetComponent<Block>().SetColor(v == 1 ? "Yellow" : v == 2 ? "Red" : "Blue");
            }   
        }

        LowRungPos = new Vector2(LowRung.position.y - 0.4f, LowRung.position.z);
        HighRungPos = new Vector2(HighRung.position.y - 0.4f, HighRung.position.z);

        LowRung.GetComponent<MeshRenderer>().material = PlayerColor == "Red" ? Red : Blue;
        HighRung.GetComponent<MeshRenderer>().material = PlayerColor == "Red" ? Red : Blue;
        BasketPole.GetComponent<MeshRenderer>().material = PlayerColor == "Red" ? Red : Blue;
    }

    private void Update()
    {
        if (!GameActive) { return; }

        if (Time.time - StartTime >= 120)
        {
            GameActive = false;
            Debug.Log("TeleOp ended, you scored " + Points + " points.");
        }

        int Temp = 0;

        foreach (Transform block in Blocks)
        {
            Block BlockInstance = block.GetComponent<Block>();

            if (BlockInstance.Color != "Yellow" && (PlayerColor == "Blue" && BlockInstance.Color == "Red" || PlayerColor == "Red" && BlockInstance.Color == "Blue"))
            {
                continue;
            }

            if (block.position.y >= LowBasket.position.y && block.position.y < HighBasket.position.y)
            {
                if (Vector3.Distance(block.position, LowBasket.position) <= 2)
                {
                    Temp += 4;
                }
            }
            else if (block.position.y >= HighBasket.position.y)
            {
                if (Vector3.Distance(block.position, HighBasket.position) <= 2)
                {
                    Temp += 8;
                }
            }

            if (BlockInstance.Color == "Yellow")
            {
                continue;
            }

            Vector2 RungSpace = new Vector2(block.position.y, block.position.z);

            if (RungSpace.x >= LowRungPos.x && Mathf.Abs(RungSpace.y - LowRungPos.y) <= 0.4f && RungSpace.x < HighRungPos.x)
            {
                Temp += 6;
            }
            else if (RungSpace.x >= HighRungPos.x && Mathf.Abs(RungSpace.y - HighRungPos.y) <= 0.4f)
            {
                Temp += 10;
            }

            if (block.GetComponent<Block>().HasHook) { continue; }

            if (Vector3.Distance(new Vector3(4.25f, 0, -7.2f), block.position) <= 3)
            {
                if (!BlockInArea)
                {
                    BlockInArea = true;
                    LastConversion = Time.time;
                }
                else if (Time.time - LastConversion >= ConversionTime)
                {
                    LastConversion = Time.time;
                    BlockInArea = false;
                    block.GetComponent<Block>().Convert(HookCount);
                    HookCount++;
                }
            }
        }

        if (Vector3.Distance(new Vector3(4.25f, 0, -7.2f), Robot.position) <= 3)
        {
            Points += 3;
        }

        Points = Temp;
    }
}
