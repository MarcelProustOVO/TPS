using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//这个脚本要挂载到武器上面
public class itemOnWorld : MonoBehaviour
{
    
    public Item thisItem;
    public Inventory playerInventory;
    // Start is called before the first frame update
    //如果碰到了触发器
    private void OnTriggerStay(Collider other)
    {
        //如果触发到的对象是我们的玩家
        if (other.gameObject.tag == "Player")
        {
            //按F可以进行收藏，并且在场景中销毁我们的武器
            if (Input.GetKeyDown(KeyCode.F)) { 
            AddNewItem();
            Destroy(gameObject);
            }
        }
    }
    public void AddNewItem()
    {
        //检测背包中是否拥有本物品，如果未拥有
        if (!playerInventory.itemList.Contains(thisItem))
        {
            //playerInventory.itemList.Add(thisItem);
            //InventoryManager.CreateNewItem(thisItem);   
            //因为这里我们已经规定了背包的数量为12格，所以我们只需要遍历然后添加
            for(int i = 0;i< playerInventory.itemList.Count; i++)
            {
                if (playerInventory.itemList[i] == null)
                {
                    playerInventory.itemList[i] = thisItem;
                    break;
                }
            }
        }
        else
        {
            thisItem.itemHeld += 1;
        }
        //在我们的背包ui上进行更新
        InventoryManager.RefreshItem();
    }
}
