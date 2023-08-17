using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class npc : MonoBehaviour
{
    private float time;
    public GameObject b;
    public GameObject p;
    public GameObject player;
    private Vector3 target;
    public NavMeshAgent nav;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        //player = GameObject.Find("Tank");
        target = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        //nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        float d = Vector3.Distance(transform.position, player.transform.position);
        if (d > 10)
        {
            //巡逻行为
            //transform.LookAt(target);
            if (Vector3.Distance(transform.position, target) < 1) 
            {
                target=new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            }
             transform.position += (target - transform.position).normalized * Time.deltaTime;
            //nav.SetDestination(target);
        }
        else
        {
            if (d < 5)
            {
                //攻击行为
                nav.speed = 0;
               transform.LookAt(player.transform);
                time += Time.deltaTime;
                if (time >= 1)
                {
                    Instantiate(b, p.transform.position, transform.rotation);
                    time = 0;
                }
            }
            else
            {
                //追击行为
                //transform.LookAt(player.transform);
                 transform.position += (player.transform.position - transform.position).normalized*Time.deltaTime;
                //nav.SetDestination(player.transform.position);
            }
        }
        




    }
}
