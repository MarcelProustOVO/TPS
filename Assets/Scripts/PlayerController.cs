using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;

//角色控制器
//要实现数据同步的脚本，要同步的脚本继承接口 IPunObservable 并实现。
public class PlayerController : MonoBehaviourPun,IPunObservable
{
   
    //组件
    public Animator ani;
    public Rigidbody body;
    public Transform camTf;//跟随的摄像机
    //数值
    public int CurHp = 10;
    public int MaxHp = 10;
    public float MoveSpeed = 5.5f;

    public float H;//水平值
    public float V;//垂直值
    public Vector3 dir;//移动方向

    public Vector3 offset;//摄像机与角色偏移值

    public float Mouse_X;//鼠标偏移值
    public float Mouse_Y;
    public float scroll;//鼠标滚轮值
    public float Angle_X;//x轴的旋转角度
    public float Angle_Y;

    public Quaternion camRotation;//摄像机旋转的四元数

    public Gun gun;//枪的脚本

    //声音
    public AudioClip reloadClip;
    public AudioClip shootClip;

    public bool isDie = false;

    public Vector3 currentPos;
    public Quaternion currentRotation;
    //武器管理模块
    public GameObject TheBagUI;
    bool  TheBagUIactive = true;
    public int damage=1;
    public int defence = 0;
    public int Damage
    {
        set
        {
            damage = value;
        }
    }
    public int Defence
    {
        set
        {
            defence = value;
        }
    }

    void Start()
    {
      
        TheBagUI = GameObject.Find("Game/Canvas/BagUI");
        TheBagUI.transform.Find("CloseBtn").GetComponent<Button>().onClick.AddListener(TheBagUIClose);
        TheBagUI.SetActive(false);

        Angle_X = transform.eulerAngles.x;
        Angle_Y = transform.eulerAngles.y;

        ani = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
        gun = GetComponentInChildren<Gun>();

        camTf = Camera.main.transform;

        currentPos = transform.position;
        currentRotation = transform.rotation;

        if (photonView.IsMine)
        {
            Game.uiManager.GetUI<FightUI>("FightUI").UpdateHp(CurHp, MaxHp);
        }
    }

    private void TheBagUIClose()
    {
        TheBagUI.SetActive(TheBagUIactive);
        if (TheBagUIactive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        TheBagUIactive = !TheBagUIactive;

    }

    void Update()
    {
       // Debug.Log(ShouleDamage);
        //判断是否是本机玩家，只能操作本机角色
        if (photonView.IsMine)
        {
            if (isDie == true)
            {
                return;
            }
            if (Input.GetKey(KeyCode.LeftShift)) { MoveSpeed = 5f; }
            else { MoveSpeed = 2f; }
            OpenMyBag();
            UpdateRotation();
            UpdatePosition();
            // 背包期间不能打枪射击
            if (TheBagUIactive)
            {
                InputCt1();
            }
        }
        else
        {
            UpdateLogic();
        }
    }

    private void OpenMyBag()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            TheBagUI.SetActive(TheBagUIactive);
            if (TheBagUIactive)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                TheBagUIactive = !TheBagUIactive;
               
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                TheBagUIactive = !TheBagUIactive;
            }
        }
    }

    //其他角色更新发送来的数据
    public void UpdateLogic()
    {
        transform.SetPositionAndRotation(Vector3.Lerp(transform.position, currentPos, Time.deltaTime * MoveSpeed * 10), Quaternion.Slerp(transform.rotation, currentRotation, Time.deltaTime * 500));
    }

    private void LateUpdate()
    {
        ani.SetFloat("Horizontal", H);
        ani.SetFloat("Vertical", V);
        ani.SetBool("isDie", isDie);

    }
    public void UpdatePosition()
    {
        H = Input.GetAxisRaw("Horizontal");
        V = Input.GetAxisRaw("Vertical");
        dir = camTf.forward * V + camTf.right * H;
        body.MovePosition(transform.position + dir * Time.deltaTime * MoveSpeed);

    }
    //更新旋转 同时设置摄像机的位置的旋转值
    public void UpdateRotation()
    {
        Mouse_X = Input.GetAxisRaw("Mouse X");
        Mouse_Y = Input.GetAxisRaw("Mouse Y");
        scroll = Input.GetAxisRaw("Mouse ScrollWheel");

        Angle_X = Angle_X - Mouse_Y;
        Angle_Y = Angle_Y + Mouse_X;
        //限制角度
        Angle_X = ClampAngle(Angle_X, -60, 60);
        Angle_Y = ClampAngle(Angle_Y, -360, 360);

        camRotation = Quaternion.Euler(Angle_X, Angle_Y, 0);
        camTf.rotation = camRotation;

        offset.z += scroll;

        //camTf.position = transform.position + camTf.rotation*offset;
        camTf.position = transform.position + camTf.rotation * new Vector3(offset.x*0.4f, offset.y*0.4f, offset.z*0.4f) ;
        transform.eulerAngles = new Vector3(0, camTf.eulerAngles.y, 0);
    }
    public float ClampAngle(float val, float min, float max)
    {
        if (val > 360) val -= 360;
        if (val < -360) val += 360;
        return Mathf.Clamp(val, min, max);
    }

    public void InputCt1()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (gun.BulletCount > 0)
            {
                //若正在填充则不能开枪
                if (ani.GetCurrentAnimatorStateInfo(1).IsName("Reload")) return;

                gun.BulletCount--;
                Game.uiManager.GetUI<FightUI>("FightUI").UpdateBulletCount(gun.BulletCount);
                //播放开火动画
                ani.Play("Fire", 1, 0);
                StopAllCoroutines();
                StartCoroutine(AttackCo());
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            //填充子弹
            AudioSource.PlayClipAtPoint(reloadClip, transform.position);//播放填充子弹声音
            ani.Play("Reload");
            gun.BulletCount = 10;
            Game.uiManager.GetUI<FightUI>("FightUI").UpdateBulletCount(gun.BulletCount);
        }
    }

    //攻击协同程序,为了延迟0.1s才开枪
    IEnumerator AttackCo()
    {   
        //延迟0.1s发射子弹
        yield return new WaitForSeconds(0.1f);

        //播放射击音效
        AudioSource.PlayClipAtPoint(shootClip, transform.position);

        //射线检测 鼠标中心点发送射线
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, Input.mousePosition.z));
     
        //射线可以改成在枪口位置为起始点 避免射到自身角色
        
        if (Physics.Raycast(ray, out RaycastHit hit, 10000, LayerMask.GetMask("Player")))
        {
           
            Debug.Log("射到角色");
            hit.transform.GetComponent<PlayerController>().GetHit(damage);
        }

        if (Physics.Raycast(ray, out RaycastHit hit2, 10000, LayerMask.GetMask("Monster")))
        {

            Debug.Log("射到敌人了");
            hit2.transform.GetComponent<monster>().Onhurt(damage);
        }

        //这个函数的含义是，所有玩家都执行一次这个函数
        photonView.RPC("AttackRpc", RpcTarget.All);//所有玩家都能看见，所有人执行AttackRpc函数

    }

   
    //接收方带上这个标签
    [PunRPC]
    public void AttackRpc()
    {
        gun.Attack();//发射子弹
    }

    //受伤
    public void GetHit(int shoulddamage)
    {
        if (isDie == true) return;

        //同步所有角色受伤
        photonView.RPC("GetHitRPC", RpcTarget.All,shoulddamage);
        
    }
    //接收方执行此行代码
    [PunRPC]
    public void GetHitRPC(int shoulddamage)
    {
        CurHp = CurHp- shoulddamage + defence;
        if (CurHp <= 0)
        {
            CurHp = 0;
            isDie = true;
        }
        if (photonView.IsMine)
        {
            Game.uiManager.GetUI<FightUI>("FightUI").UpdateHp(CurHp, MaxHp);
            Game.uiManager.GetUI<FightUI>("FightUI").UpdateBlood();
            if (CurHp==0)
            {
                Invoke(nameof(gameOver), 3);//三秒后显示失败页面
            }
        }
    }

    private void gameOver()
    {
        //显示鼠标
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Game.uiManager.ShowUI<LossUI>("LossUI").onClickCallBack= OnReset;
    }

    public void OnReset()
    {
        //隐藏鼠标
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        photonView.RPC("OnResetRPC", RpcTarget.All);
    }

    [PunRPC]
    public void OnResetRPC()
    {
        isDie = false;
        CurHp = MaxHp;
        if (photonView.IsMine)
        {
            Game.uiManager.GetUI<FightUI>("FightUI").UpdateHp(CurHp, MaxHp);
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (ani != null)
        {
            Vector3 angle = ani.GetBoneTransform(HumanBodyBones.Chest).localEulerAngles;
            angle.x = Angle_X;
            ani.SetBoneLocalRotation(HumanBodyBones.Chest, Quaternion.Euler(angle));
        }
    }

    //用来接受别人的数据
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //发送数据
            stream.SendNext(H);
            stream.SendNext(V);
            stream.SendNext(Angle_X);
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
      
        }
        else
        {
            //接收数据
            H = (float)stream.ReceiveNext();
            V = (float)stream.ReceiveNext();
            Angle_X = (float)stream.ReceiveNext();
            currentPos = (Vector3)stream.ReceiveNext();
            currentRotation = (Quaternion)stream.ReceiveNext();
           
        }
    }

}
