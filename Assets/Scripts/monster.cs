using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class monster : MonoBehaviour
{
    private NavMeshAgent nav;
    public Transform target;
    private int hp;
    public Slider s;
    public int maxDis = 2;
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();//Ѱ·���
        target = GameObject.FindGameObjectWithTag("Player").transform;//Ѱ������ΪĿ��
        nav.SetDestination(target.position);//����׷��Ŀ��
        hp = 10;//Ѫ����ʼ��
        s.value = hp;//����Ѫ��
    }

    //���˺���
    public void Onhurt(int hurt)
    {
        hp = hp - hurt;
        s.value = hp;
        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    void Update()
    {
        float dis = Vector3.Distance(target.position, this.transform.position);
        if (dis > maxDis)
        {
            //������Χ����׷��
            nav.SetDestination(target.position);
        }
        else {
            //�ѽ��뷶Χ��
        }
        LookAtCamera();
    }

    //��Ѫ��ʵʱ�������
    private void LookAtCamera()
    {
        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 target = new Vector3(cameraPos.x, s.transform.position.y, cameraPos.z);
        s.transform.LookAt(target);
    }

}
