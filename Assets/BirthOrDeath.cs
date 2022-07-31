using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirthOrDeath : MonoBehaviour
{
    public Vector3 posTarget;//��������ʵ�ο�����
    public bool needMove = true;
    private bool isAlive = true;
    public Vector3 deadpos;//��������

    public delegate void ImDead();

    public ImDead GoDead;

    public bool IsAlive
    {
        get => isAlive;
        set
        {
            deadpos = posTarget - new Vector3(0.0f, 100.0f + Random.value * 100.0f, 0.0f);//����ʵģ��������� �漴�³� ��Ϊ ��������
            needMove = true;
            isAlive = value;
        }
    }

    void Start()
    {

    }

    void Update()
    {
        if (IsAlive)
        {
            if (needMove)
            {
                if ((posTarget - transform.position).magnitude < 0.01f)
                {
                    transform.position = new Vector3(posTarget.x, posTarget.y, posTarget.z);
                    needMove = false;
                }
                else
                {
                    transform.position = Vector3.Lerp(transform.position, posTarget, 0.1f);//���� ��0.1���ٶ� �� �����׼�ο����� ��£   
                }
            }
        }
        else// ������
        {
            transform.position = Vector3.Lerp(transform.position, deadpos, 0.1f);
            if ((deadpos - transform.position).magnitude < 0.3f)
            {
                GoDead();
                /*
                  public void BlockDead()
                  {
                       subblockcount--;
                       if(subblockcount==0)
                       {
                         Destroy(gameObject);
                         }
                       }
                 */
                Destroy(gameObject);
            }
        }
    }
}
