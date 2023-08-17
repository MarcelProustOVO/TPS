using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
 private Rigidbody rig;
 public GameObject bullet;
 public GameObject pos;
 // Start is called before the first frame update
void Start()
{
 rig =transform.GetComponent<Rigidbody>();
 }
// Update is called once per frame
 void Update()
 {
   //方向键控制运动
  //transform.position += transforn.forward*Time.deltaTime;
  //Debug. Log(transform.forward);
  //if(Input.GetKey(KeyCode.UpArrow))
  //{
  // transform. position +=transform. forward * Time.deltaTime;
 // }
 //else if(Input.GetKey(KeyCode. DownArrow))
 //{
   //transform. position +=(-1)* transform. forward * Time.deltaTime;
//}
//else if(Input.GetKey(KeyCode.LeftArrow))
//{
   //transform. Rotate((-1) * transform . up*Time. deltaTime*30);
//}
//else if(Input.GetKey(KeyCode. RightArrow))
//{
   //transform. Rotate(transform. up * Time.deltaTime * 30);
//}
  //}
  //通过方向键和wsad控制方向运动
  float v = Input.GetAxis("Vertical");
  float h = Input.GetAxis("Horizontal");
 // transform. position += v * transform.forward * Time.deltaTime;
 //transform. Rotate(h* transform. up * Time. deltaTime * 20);
  //通过物理组件控制运动
  rig.velocity = v * transform.forward;
  rig.angularVelocity =h * transform .up * 10;
  //攻击
  if(Input.GetMouseButtonDown(0))
  {
   Instantiate(bullet,pos.transform.position,transform.rotation);
  }
}
}
