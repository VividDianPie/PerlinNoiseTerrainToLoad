using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// 随机地图生成
public class RandomMapMaker : MonoBehaviour
{

    /// 用以柏林噪声采样的X和Z值（柏林噪声返回的是Y值）
    private float seedX, seedZ;

    /// 地图的宽度（X轴方向）
    [SerializeField]
    private int width = 50;

    /// 地图的深度（Z轴方向）
    [SerializeField]
    private int depth = 50;

    /// 地图的最大高度
    [SerializeField]
    private int maxHeight = 10;

    /// 决定了采样间隔 值越大 采样间隔越小
    [SerializeField]
    private float relief = 15.0f;

    private void Awake()
    {
        seedX = Random.value * 100f;
        seedZ = Random.value * 100f;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {

                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.localPosition = new Vector3(x, 0, z);
                cube.transform.SetParent(transform);

                SetY(cube);
            }
        }
    }

    private void OnValidate()//脚本加载 面板修改时调用
    {
        if (!Application.isPlaying)//在编辑器中，如果编辑器处于播放模式，将返回 true。
            return;

        foreach (Transform child in transform)//编辑器修改 重置 柏林噪声 y 轴
            SetY(child.gameObject);
    }

    /// 利用柏林噪声设定Y值
    /// <param name="cube"> 需要被设定Y值的物体 </param>
    private void SetY(GameObject cube)
    {
        float y = 0;

        float xSample = (cube.transform.localPosition.x + seedX) / relief;
        float zSample = (cube.transform.localPosition.z + seedZ) / relief;
        float noise = Mathf.PerlinNoise(xSample, zSample);
        y = maxHeight * noise;

        // 为了模仿我的世界的格子风 将每一次计算出来的浮点数值转换到整数值
        y = Mathf.Round(y);

        cube.transform.localPosition = new Vector3(cube.transform.localPosition.x,
                                                   y,
                                                   cube.transform.localPosition.z);

        Color color = Color.black;
        if (y > maxHeight * 0.3f)
        {
            ColorUtility.TryParseHtmlString("#019540FF", out color);
        }
        else if (y > maxHeight * 0.2f)
        {
            ColorUtility.TryParseHtmlString("#2432ADFF", out color);
        }
        else if (y > maxHeight * 0.1f)
        {
            ColorUtility.TryParseHtmlString("#D4500EFF", out color);
        }
        cube.GetComponent<MeshRenderer>().material.color = color;
    }
}
