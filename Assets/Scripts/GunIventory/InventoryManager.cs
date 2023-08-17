using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    //����ģʽ���Գ����������ͻ����
    // Start is called before the first frame update
    static InventoryManager instance;//����ģʽ

    public Inventory maBag;//���ǵı���
    public GameObject slotGrid;//�������е���Ʒ����Ҫд�����������
    //public slot slotPrefab;
    public GameObject emptySlot;//��������������ʾ��Ʒ���Ǹ�Ԥ����
    public Text itemInfomation;//ÿ����Ʒ�Ľ���
    public List<GameObject>slots= new List<GameObject>();   //��Ʒ���б�
    private void Awake()
    {
        //��֤����ģʽ
        if (instance != null)
        {
            Destroy(this); 
        }
        instance = this; 
    }
    private void Start()
    {
        //�����ǵ�װ������и���
        GameObject.Find("Game/Canvas/BagUI/weapon").gameObject.GetComponent<slot>().slotID = -1;
        GameObject.Find("Game/Canvas/BagUI/weapon").gameObject.GetComponent<slot>().setupslot(null);
        GameObject.Find("Game/Canvas/BagUI/defence").gameObject.GetComponent<slot>().slotID = -1;
        GameObject.Find("Game/Canvas/BagUI/defence").gameObject.GetComponent<slot>().setupslot(null);
        GameObject.Find("Game/Canvas/BagUI/defence/Item").gameObject.SetActive(true);
        GameObject.Find("Game/Canvas/BagUI/weapon/Item").gameObject.SetActive(true);
    }
    private void OnEnable()
    {
        //һ��ʼ�͸���һ��
        RefreshItem();  
    }
    //�����Ʒ��ʱ�򣬾ͻ����һ���������
    public static void updateItemInfo(string ItemDescription)
    {
        instance.itemInfomation.text = "";
        instance.itemInfomation.text += ItemDescription;
    }
    //�����Ϊ�˴����������������Բ��ÿ�����Ϊû��
    //public static void CreateNewItem(Item item)
    //{
    //    //��ʼ��Ԥ���壬λ�ã���ת��
    //    slot newItem = Instantiate(instance.slotPrefab,instance.slotGrid.transform.position,Quaternion.identity);
    //    //���׽ڵ������ǵĸ���
    //    newItem.gameObject.transform.SetParent(instance.slotGrid.transform);
    //    //Ϊ����ʵ�����Ķ���ֵ
    //    newItem.slotItem=item;
    //    newItem.slotImage.sprite = item.itemImage;
    //    newItem.slotNum.text=item.itemHeld.ToString();
    //}
    //���������е��б��е�����ȫ����ʵ��һ��
    
    
    
    public static void RefreshItem()   //�����������Ǳ����ĺ���
    {
        //һ��ʼ�϶���0��һ��ʼ�ǲ����õģ����Ǻ���ÿ�θ��£��Ͷ���12�ˣ��Ժ�ÿ�ζ�Ҫ��
        for (int i=0;i<instance.slotGrid.transform.childCount;i++)
        {
            if (instance.slotGrid.transform.childCount == 0) break;
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);      
            instance.slots.Clear();
        }

     for(int i=0;i<instance.maBag.itemList.Count;i++)
        {
            //����ʵ�廯��Ȼ��ŵ�List�б���
            GameObject newItem = Instantiate(instance.emptySlot, instance.slotGrid.transform.position, Quaternion.identity);
            //������ʵ������slotԤ�������Grid���棬Grid layout Group������Զ�Ϊ��������
            newItem.transform.SetParent(instance.slotGrid.transform);
            //�������Ԥ���廹���������ǻ���������ǵ�
            instance.slots.Add(newItem);
            instance.slots[i].GetComponent<slot>().slotID= i;
            instance.slots[i].GetComponent<slot>().setupslot(instance.maBag.itemList[i]);//������Ϣ
        }
    }
    private void OnApplicationQuit()
    {
        for (int i = 0; i < instance.maBag.itemList.Count; i++)
        {
            instance.maBag.itemList[i]=null;
        }
           
    }

}
