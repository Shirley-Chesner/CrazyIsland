using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class playerInventory : MonoBehaviour
{
    public static playerInventory instance;
    public GameObject inventory;
    PlayerMovement moveScript;
    public List<item> items = new List<item>();
    public Transform itemContent;
    public GameObject inventoryItem;

    public ItemController[] ItemsController;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        inventory.SetActive(false);
        moveScript = gameObject.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventory.SetActive(!inventory.activeSelf);
            moveScript.changeAllowMovement(!inventory.activeSelf);

            if (inventory.activeSelf)
            {
                listItems();
            }
        }
         else if (!inventory.activeSelf)
        {
            moveScript.changeAllowMovement(true);
        }
    }


    public void add(item item)
    {
        items.Add(item);
    }

    public void remove(item item)
    {
        items.Remove(item);
    }

    public void usePotion(int heal)
    {
        PlayerHealth health = gameObject.GetComponent<PlayerHealth>();
        health.heal(heal);
    }

    public void listItems()
    {
        // Clean inventory before each open
        foreach (Transform item in itemContent)
        {
            Destroy(item.gameObject);
        }

        // Add items to inventory
        foreach (var item in items)
        {
            GameObject obj = Instantiate(inventoryItem, itemContent);
            var itemName = obj.transform.Find("itemName").GetComponent<TMP_Text>();
            var itemIcon = obj.transform.Find("itemIcon").GetComponent<Image>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
        }

        setInvItems();
    }


    public void setInvItems()
    {
        ItemsController = itemContent.GetComponentsInChildren<ItemController>();
        for (int i =0; i < items.Count; i++)
        {
            ItemsController[i].addItem(items[i]);
        }
    }

}
