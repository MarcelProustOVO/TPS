using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class health : MonoBehaviour
{
    // Start is called before the first frame update
    private int hp;
    public GameObject ef1;
    public Slider s ;
    void Start()
    {
        hp = 5;
        s.value=hp;
    }

    // Update is called once per frame
    public void damage()
    {
        hp--;
        s.value=hp;
        if(hp<=0)
        {
            GameObject e1=Instantiate(ef1,transform.position,transform.rotation);
            Destroy(e1,0.9f);
            Destroy(this.gameObject); 
        }
    }
}
