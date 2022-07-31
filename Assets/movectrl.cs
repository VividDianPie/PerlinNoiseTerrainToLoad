using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movectrl : MonoBehaviour
{
    void Start()
    {
        //设置相机初始坐标与旋转方向
        transform.position = new Vector3(transform.position.x, 15, transform.position.z);
        Quaternion q = new Quaternion();
        q.eulerAngles = new Vector3(30, 0, 0);
        transform.rotation = q;

    }


    // 控制相机移动与转向部分
    void Update()
    {



        if (Input.GetKey(KeyCode.W))
        {
            Vector3 dir = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
            transform.Translate(dir * Time.deltaTime * 3, Space.World);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Vector3 dir = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
            transform.Translate(-dir * Time.deltaTime * 3, Space.World);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.up, Time.deltaTime * 30.0f, Space.World);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up, Time.deltaTime * 30.0f, Space.World);
        }

    }
}
