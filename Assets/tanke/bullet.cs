using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public GameObject ef;
    private Rigidbody rig;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject,1);
        rig=transform.GetComponent<Rigidbody>();
        rig.AddForce(transform.forward *800);
    }

    // Update is called once per frame
    public void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
        GameObject e=Instantiate(ef,transform.position,transform.rotation);
        Destroy(e,1.5f);
        if(other.tag=="tank")
        {
            other.SendMessage("damage");
        }
    }
}
