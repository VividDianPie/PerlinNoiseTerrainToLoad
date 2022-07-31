using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinMapBlock : MonoBehaviour
{
    /// 用以柏林噪声采样的X和Z值的随机种子（柏林噪声返回的是Y值）
    private static float seedX, seedZ;
    /// 决定了采样间隔 值越大 采样间隔越小
    [SerializeField]
    private static float relief = 15.0f;
    /// 一个地图块横竖21个格子
    private static int blocksize = 21;//单个区块的宽高
    /// 地图的最大高度
    [SerializeField]
    private static int maxHeight = 10;
    //块id
    private int blockidx;
    private int blockidy;
    private string blockname;
    private Vector2 center;
    private float leftx;
    private float lefty;
    private int subblockcount = 0;


    //当此 chunk 之下的 方块全部 
    public void BlockDead()
    {
        subblockcount--;
        if (subblockcount == 0)
        {
            Destroy(gameObject);
        }
    }



    public Vector2 Center
    {
        get
        {
            return center = new Vector2(Leftx + Blocksize / 2.0f, Lefty - Blocksize / 2.0f);
        }
    }



    public int Blockidx { get => blockidx; set => blockidx = value; }//九宫格真方块Id
    public int Blockidy { get => blockidy; set => blockidy = value; }
    public string Blockname
    {
        get
        {
            blockname = "Blocks_" + ((blockidx < 0) ? "n" : "p") + Mathf.Abs(blockidx) + ((blockidy < 0) ? "n" : "p") + Mathf.Abs(blockidy);
            return blockname;
        }
    }

    //区块左 
    public float Leftx
    {
        get
        {
            leftx = Blockidx * Blocksize - Blocksize / 2.0f;
            return leftx;
        }
    }
    public float Lefty
    {
        get
        {
            lefty = Blockidy * Blocksize + Blocksize / 2.0f;
            return lefty;
        }
    }
    public static int Blocksize { get => blocksize; set => blocksize = value; }//单个区块的宽高

    static PerlinMapBlock()
    {
        System.Random r = new System.Random();
        seedX = r.Next(0, 100);
        seedZ = r.Next(0, 100);
    }

    void Start()
    {

    }
    public void Init()
    {
        gameObject.name = Blockname;//九宫格实时方块代号名
        transform.position = new Vector3(Center.x, 0.0f, Center.y);//货真价实的区块中心坐标

        for (int y = 0; y < Blocksize; ++y)//真区块横竖 size
        {
            for (int x = 0; x < Blocksize; ++x)
            {
                newBlock(x, y);
            }
        }
    }




    private void newBlock(int x, int y)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.SetParent(transform);

        float xSample = (Leftx + x + seedX) / relief;
        float zSample = (Lefty - y + seedZ) / relief;
        float noise = Mathf.PerlinNoise(xSample, zSample);
        float posy = maxHeight * noise;
        posy = Mathf.Round(posy);

        cube.transform.position = new Vector3(
            Leftx + x,
            (Random.value > 0.5) ? (posy - 100.0f - Random.value * 100.0f) : (posy + 100.0f - Random.value * 100.0f),
            Lefty - y);

        Color color = Color.black;
        if (posy > maxHeight * 0.3f)
        {
            ColorUtility.TryParseHtmlString("#019540FF", out color);
        }
        else if (posy > maxHeight * 0.2f)
        {
            ColorUtility.TryParseHtmlString("#2432ADFF", out color);
        }
        else if (posy > maxHeight * 0.1f)
        {
            ColorUtility.TryParseHtmlString("#D4500EFF", out color);
        }
        cube.GetComponent<MeshRenderer>().material.color = color;

        BirthOrDeath sc = cube.AddComponent<BirthOrDeath>();
        sc.posTarget = new Vector3(Leftx + x, posy, Lefty - y);//生死方块真真实坐标
        sc.needMove = true;
        sc.IsAlive = true;
        sc.GoDead = BlockDead;
    }





    //方块将删
    public void DeleteBlock()
    {
        subblockcount = 0;
        // 获取 chunk区块之下所有方块  21 * 21 = 441  
        BirthOrDeath[] sc = gameObject.GetComponentsInChildren<BirthOrDeath>();
        foreach (BirthOrDeath item in sc)
        {
            subblockcount++;
            item.IsAlive = false;
        }
    }


    //方块不动
    public void KeepBlock()
    {
        subblockcount = 0;
        BirthOrDeath[] sc = gameObject.GetComponentsInChildren<BirthOrDeath>();
        foreach (BirthOrDeath item in sc)
        {
            subblockcount++;
            item.IsAlive = true;
        }
    }
}
