using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;

//��ɫ������
//Ҫʵ������ͬ���Ľű���Ҫͬ���Ľű��̳нӿ� IPunObservable ��ʵ�֡�
public class PlayerController : MonoBehaviourPun,IPunObservable
{
   
    //���
    public Animator ani;
    public Rigidbody body;
    public Transform camTf;//����������
    //��ֵ
    public int CurHp = 10;
    public int MaxHp = 10;
    public float MoveSpeed = 5.5f;

    public float H;//ˮƽֵ
    public float V;//��ֱֵ
    public Vector3 dir;//�ƶ�����

    public Vector3 offset;//��������ɫƫ��ֵ

    public float Mouse_X;//���ƫ��ֵ
    public float Mouse_Y;
    public float scroll;//������ֵ
    public float Angle_X;//x�����ת�Ƕ�
    public float Angle_Y;

    public Quaternion camRotation;//�������ת����Ԫ��

    public Gun gun;//ǹ�Ľű�

    //����
    public AudioClip reloadClip;
    public AudioClip shootClip;

    public bool isDie = false;

    public Vector3 currentPos;
    public Quaternion currentRotation;
    //��������ģ��
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
        //�ж��Ƿ��Ǳ�����ң�ֻ�ܲ���������ɫ
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
            // �����ڼ䲻�ܴ�ǹ���
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

    //������ɫ���·�����������
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
    //������ת ͬʱ�����������λ�õ���תֵ
    public void UpdateRotation()
    {
        Mouse_X = Input.GetAxisRaw("Mouse X");
        Mouse_Y = Input.GetAxisRaw("Mouse Y");
        scroll = Input.GetAxisRaw("Mouse ScrollWheel");

        Angle_X = Angle_X - Mouse_Y;
        Angle_Y = Angle_Y + Mouse_X;
        //���ƽǶ�
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
                //������������ܿ�ǹ
                if (ani.GetCurrentAnimatorStateInfo(1).IsName("Reload")) return;

                gun.BulletCount--;
                Game.uiManager.GetUI<FightUI>("FightUI").UpdateBulletCount(gun.BulletCount);
                //���ſ��𶯻�
                ani.Play("Fire", 1, 0);
                StopAllCoroutines();
                StartCoroutine(AttackCo());
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            //����ӵ�
            AudioSource.PlayClipAtPoint(reloadClip, transform.position);//��������ӵ�����
            ani.Play("Reload");
            gun.BulletCount = 10;
            Game.uiManager.GetUI<FightUI>("FightUI").UpdateBulletCount(gun.BulletCount);
        }
    }

    //����Эͬ����,Ϊ���ӳ�0.1s�ſ�ǹ
    IEnumerator AttackCo()
    {   
        //�ӳ�0.1s�����ӵ�
        yield return new WaitForSeconds(0.1f);

        //���������Ч
        AudioSource.PlayClipAtPoint(shootClip, transform.position);

        //���߼�� ������ĵ㷢������
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, Input.mousePosition.z));
     
        //���߿��Ըĳ���ǹ��λ��Ϊ��ʼ�� �����䵽�����ɫ
        
        if (Physics.Raycast(ray, out RaycastHit hit, 10000, LayerMask.GetMask("Player")))
        {
           
            Debug.Log("�䵽��ɫ");
            hit.transform.GetComponent<PlayerController>().GetHit(damage);
        }

        if (Physics.Raycast(ray, out RaycastHit hit2, 10000, LayerMask.GetMask("Monster")))
        {

            Debug.Log("�䵽������");
            hit2.transform.GetComponent<monster>().Onhurt(damage);
        }

        //��������ĺ����ǣ�������Ҷ�ִ��һ���������
        photonView.RPC("AttackRpc", RpcTarget.All);//������Ҷ��ܿ�����������ִ��AttackRpc����

    }

   
    //���շ����������ǩ
    [PunRPC]
    public void AttackRpc()
    {
        gun.Attack();//�����ӵ�
    }

    //����
    public void GetHit(int shoulddamage)
    {
        if (isDie == true) return;

        //ͬ�����н�ɫ����
        photonView.RPC("GetHitRPC", RpcTarget.All,shoulddamage);
        
    }
    //���շ�ִ�д��д���
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
                Invoke(nameof(gameOver), 3);//�������ʾʧ��ҳ��
            }
        }
    }

    private void gameOver()
    {
        //��ʾ���
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Game.uiManager.ShowUI<LossUI>("LossUI").onClickCallBack= OnReset;
    }

    public void OnReset()
    {
        //�������
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

    //�������ܱ��˵�����
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //��������
            stream.SendNext(H);
            stream.SendNext(V);
            stream.SendNext(Angle_X);
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
      
        }
        else
        {
            //��������
            H = (float)stream.ReceiveNext();
            V = (float)stream.ReceiveNext();
            Angle_X = (float)stream.ReceiveNext();
            currentPos = (Vector3)stream.ReceiveNext();
            currentRotation = (Quaternion)stream.ReceiveNext();
           
        }
    }

}
