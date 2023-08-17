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
        nav = GetComponent<NavMeshAgent>();//寻路组件
        target = GameObject.FindGameObjectWithTag("Player").transform;//寻找人物为目标
        nav.SetDestination(target.position);//设置追踪目标
        hp = 10;//血量初始化
        s.value = hp;//设置血条
    }

    //受伤函数
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
            //超出范围继续追踪
            nav.SetDestination(target.position);
        }
        else {
            //已进入范围内
        }
        LookAtCamera();
    }

    //将血条实时朝向玩家
    private void LookAtCamera()
    {
        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 target = new Vector3(cameraPos.x, s.transform.position.y, cameraPos.z);
        s.transform.LookAt(target);
    }

}
