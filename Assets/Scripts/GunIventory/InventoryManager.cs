using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    //单例模式，自程序启动，就会存在
    // Start is called before the first frame update
    static InventoryManager instance;//单例模式

    public Inventory maBag;//我们的背包
    public GameObject slotGrid;//我们所有的物品，都要写在这个格下面
    //public slot slotPrefab;
    public GameObject emptySlot;//这是我们用来显示物品的那个预制体
    public Text itemInfomation;//每个物品的介绍
    public List<GameObject>slots= new List<GameObject>();   //物品的列表
    private void Awake()
    {
        //保证单例模式
        if (instance != null)
        {
            Destroy(this); 
        }
        instance = this; 
    }
    private void Start()
    {
        //对我们的装备格进行更新
        GameObject.Find("Game/Canvas/BagUI/weapon").gameObject.GetComponent<slot>().slotID = -1;
        GameObject.Find("Game/Canvas/BagUI/weapon").gameObject.GetComponent<slot>().setupslot(null);
        GameObject.Find("Game/Canvas/BagUI/defence").gameObject.GetComponent<slot>().slotID = -1;
        GameObject.Find("Game/Canvas/BagUI/defence").gameObject.GetComponent<slot>().setupslot(null);
        GameObject.Find("Game/Canvas/BagUI/defence/Item").gameObject.SetActive(true);
        GameObject.Find("Game/Canvas/BagUI/weapon/Item").gameObject.SetActive(true);
    }
    private void OnEnable()
    {
        //一开始就更新一下
        RefreshItem();  
    }
    //点击物品的时候，就会调用一次这个函数
    public static void updateItemInfo(string ItemDescription)
    {
        instance.itemInfomation.text = "";
        instance.itemInfomation.text += ItemDescription;
    }
    //起初是为了创建背包数量，可以不用看，因为没用
    //public static void CreateNewItem(Item item)
    //{
    //    //初始化预制体，位置，和转向
    //    slot newItem = Instantiate(instance.slotPrefab,instance.slotGrid.transform.position,Quaternion.identity);
    //    //父亲节点是我们的格子
    //    newItem.gameObject.transform.SetParent(instance.slotGrid.transform);
    //    //为我们实例化的对象赋值
    //    newItem.slotItem=item;
    //    newItem.slotImage.sprite = item.itemImage;
    //    newItem.slotNum.text=item.itemHeld.ToString();
    //}
    //把我们已有的列表中的武器全部都实现一遍
    
    
    
    public static void RefreshItem()   //用来更新我们背包的函数
    {
        //一开始肯定是0，一开始是不调用的，但是后面每次更新，就都是12了，以后每次都要用
        for (int i=0;i<instance.slotGrid.transform.childCount;i++)
        {
            if (instance.slotGrid.transform.childCount == 0) break;
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);      
            instance.slots.Clear();
        }

     for(int i=0;i<instance.maBag.itemList.Count;i++)
        {
            //挨个实体化，然后放到List列表中
            GameObject newItem = Instantiate(instance.emptySlot, instance.slotGrid.transform.position, Quaternion.identity);
            //把我们实例化的slot预制体放在Grid下面，Grid layout Group组件会自动为我们排列
            newItem.transform.SetParent(instance.slotGrid.transform);
            //但是添加预制体还不够，我们还必须把我们的
            instance.slots.Add(newItem);
            instance.slots[i].GetComponent<slot>().slotID= i;
            instance.slots[i].GetComponent<slot>().setupslot(instance.maBag.itemList[i]);//更新信息
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
