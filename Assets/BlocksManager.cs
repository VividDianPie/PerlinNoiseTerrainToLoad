using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksManager : MonoBehaviour
{
    private int curidx, curidy;
    private int[] xoffset = { -1, 0, 1 };
    private int[] yoffset = { 1, 0, -1 };


    void Start()
    {
        // 二维坐标系 四象限 坐标 
        curidx = Mathf.FloorToInt(transform.position.x / PerlinMapBlock.Blocksize + 0.5f);
        curidy = Mathf.CeilToInt(transform.position.z / PerlinMapBlock.Blocksize + 0.5f) - 1;
        //决定了chunk的生死
        ManagerBlock();
    }
    //制造方块名
    //标准教科书级 参考区块名模板 根据当前坐标 计算出了 其四周八方向的 方块坐标 考虑当前坐标的理论无限性 其四周八方向的 方块坐标 亦无限
    string[] MakeBlockNames()
    {
        string[] blocknames = new string[9];
        for (int y = 0; y < 3; ++y)
        {
            for (int x = 0; x < 3; ++x)
            {// negative 负数 缩写   positive 正数 缩写
                blocknames[y * 3 + x] = "Blocks_"
                    + ((curidx + xoffset[x] < 0) ? "n" : "p") + Mathf.Abs(curidx + xoffset[x])
                    + ((curidy + yoffset[y] < 0) ? "n" : "p") + Mathf.Abs(curidy + yoffset[y]);
                Debug.Log(blocknames[y * 3 + x]);
            }
        }
        return blocknames;
    }
    //是否保存方块           教科书级chunk名数组 当前chunk场景名
    bool CanIKeep(string[] names, string name)
    {
        foreach (string item in names)
        {//此场景chunk名 与 教科书级chunk名皆对应不上
            if (name.Equals(item))
            {
                return true;
            }
        }
        return false;
    }


    bool WellBirth(PerlinMapBlock[] pm, string name)
    {
        foreach (PerlinMapBlock p in pm)
        {

            if (p.gameObject.name.Equals(name))
                return false;
        }
        return true;
    }


    void ManagerBlock()
    {
        // scene 中已存在 地形chunk 类型 
        PerlinMapBlock[] pmbs = GameObject.FindObjectsOfType<PerlinMapBlock>();
        //标准模板教科书名
        string[] wantblock = MakeBlockNames();

        foreach (PerlinMapBlock block in pmbs)
        {
            // 当前场景chunk名对比标准模板教科书级chunk名是否有一定的出入
            if (!CanIKeep(wantblock, block.gameObject.name))
            {
                // 此当前场景此chunk名 与 标准模板 chunk名 匹配不上 意味 此 场景下此chunk 将死
                block.DeleteBlock();
            }
            else
            {
                // 其他chunk 将会缄默
                block.KeepBlock();
            }
        }

        for (int y = 0; y < 3; ++y)
        {
            for (int x = 0; x < 3; ++x)
            {
                if (WellBirth(pmbs, // 场景当前 全部九个 chunk 
                    "Blocks_" + ((curidx + xoffset[x] < 0) ? "n" : "p") + Mathf.Abs(curidx + xoffset[x]) + ((curidy + yoffset[y] < 0) ? "n" : "p") + Mathf.Abs(curidy + yoffset[y])))
                {
                    GameObject obj = new GameObject();
                    PerlinMapBlock ppp = obj.AddComponent<PerlinMapBlock>();
                    ppp.Blockidx = curidx + xoffset[x];
                    ppp.Blockidy = curidy + yoffset[y];

                    ppp.Init();
                }
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        int x, y;
        //if (transform.position.x >= 0)
        //{
        //    x = ((int)transform.position.x) / PerlinMapBlock.Blocksize;
        //}
        //else
        //{
        //    x = ((int)(transform.position.x - PerlinMapBlock.Blocksize)) / PerlinMapBlock.Blocksize;
        //}
        x = Mathf.FloorToInt(transform.position.x / PerlinMapBlock.Blocksize + 0.5f);
        y = Mathf.CeilToInt(transform.position.z / PerlinMapBlock.Blocksize + 0.5f) - 1;

        if (x != curidx || y != curidy) //当角色相机走出 区块 一整个 长宽（ ）时 
        {
            curidx = x;
            curidy = y;
            //意味着要更换地图了
            ManagerBlock();
        }
    }
}
