using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//����ű�Ҫ���ص���������
public class itemOnWorld : MonoBehaviour
{
    
    public Item thisItem;
    public Inventory playerInventory;
    // Start is called before the first frame update
    //��������˴�����
    private void OnTriggerStay(Collider other)
    {
        //����������Ķ��������ǵ����
        if (other.gameObject.tag == "Player")
        {
            //��F���Խ����ղأ������ڳ������������ǵ�����
            if (Input.GetKeyDown(KeyCode.F)) { 
            AddNewItem();
            Destroy(gameObject);
            }
        }
    }
    public void AddNewItem()
    {
        //��ⱳ�����Ƿ�ӵ�б���Ʒ�����δӵ��
        if (!playerInventory.itemList.Contains(thisItem))
        {
            //playerInventory.itemList.Add(thisItem);
            //InventoryManager.CreateNewItem(thisItem);   
            //��Ϊ���������Ѿ��涨�˱���������Ϊ12����������ֻ��Ҫ����Ȼ�����
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
        //�����ǵı���ui�Ͻ��и���
        InventoryManager.RefreshItem();
    }
}
