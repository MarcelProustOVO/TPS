using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ǹе
public class Gun : MonoBehaviour
{
    public int BulletCount = 10;

    public GameObject bulletPrefab;
    public GameObject casingPreafab;

    public Transform bulletTf;
    public Transform casingTf;
    void Start()
    {
        
    }
    
    public void Attack()
    {
        GameObject bulletObj = Instantiate(bulletPrefab);
        bulletObj.transform.position = bulletTf.transform.position;
        bulletObj.GetComponent<Rigidbody>().AddForce(transform.forward * 500, ForceMode.Impulse);//子弹飞快些 让中心点跟枪口位置可自行调整摄像机的偏移值

        GameObject casingObj = Instantiate(casingPreafab);
        casingObj.transform.position = casingTf.transform.position;
    }
}
