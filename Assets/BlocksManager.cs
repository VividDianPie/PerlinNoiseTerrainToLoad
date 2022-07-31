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
        // ��ά����ϵ ������ ���� 
        curidx = Mathf.FloorToInt(transform.position.x / PerlinMapBlock.Blocksize + 0.5f);
        curidy = Mathf.CeilToInt(transform.position.z / PerlinMapBlock.Blocksize + 0.5f) - 1;
        //������chunk������
        ManagerBlock();
    }
    //���췽����
    //��׼�̿��鼶 �ο�������ģ�� ���ݵ�ǰ���� ������� �����ܰ˷���� �������� ���ǵ�ǰ��������������� �����ܰ˷���� �������� ������
    string[] MakeBlockNames()
    {
        string[] blocknames = new string[9];
        for (int y = 0; y < 3; ++y)
        {
            for (int x = 0; x < 3; ++x)
            {// negative ���� ��д   positive ���� ��д
                blocknames[y * 3 + x] = "Blocks_"
                    + ((curidx + xoffset[x] < 0) ? "n" : "p") + Mathf.Abs(curidx + xoffset[x])
                    + ((curidy + yoffset[y] < 0) ? "n" : "p") + Mathf.Abs(curidy + yoffset[y]);
                Debug.Log(blocknames[y * 3 + x]);
            }
        }
        return blocknames;
    }
    //�Ƿ񱣴淽��           �̿��鼶chunk������ ��ǰchunk������
    bool CanIKeep(string[] names, string name)
    {
        foreach (string item in names)
        {//�˳���chunk�� �� �̿��鼶chunk���Զ�Ӧ����
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
        // scene ���Ѵ��� ����chunk ���� 
        PerlinMapBlock[] pmbs = GameObject.FindObjectsOfType<PerlinMapBlock>();
        //��׼ģ��̿�����
        string[] wantblock = MakeBlockNames();

        foreach (PerlinMapBlock block in pmbs)
        {
            // ��ǰ����chunk���Աȱ�׼ģ��̿��鼶chunk���Ƿ���һ���ĳ���
            if (!CanIKeep(wantblock, block.gameObject.name))
            {
                // �˵�ǰ������chunk�� �� ��׼ģ�� chunk�� ƥ�䲻�� ��ζ �� �����´�chunk ����
                block.DeleteBlock();
            }
            else
            {
                // ����chunk �����Ĭ
                block.KeepBlock();
            }
        }

        for (int y = 0; y < 3; ++y)
        {
            for (int x = 0; x < 3; ++x)
            {
                if (WellBirth(pmbs, // ������ǰ ȫ���Ÿ� chunk 
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

        if (x != curidx || y != curidy) //����ɫ����߳� ���� һ���� ���� ��ʱ 
        {
            curidx = x;
            curidy = y;
            //��ζ��Ҫ������ͼ��
            ManagerBlock();
        }
    }
}
