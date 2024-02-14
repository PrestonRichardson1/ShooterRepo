using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    [Header("Ui")]
    public GameObject inventory;
    private List<Slot> allInventorySlots = new List<Slot>();
    public List<Slot> InventorySlots = new List<Slot>();
    public List<Slot> hotbarSlots = new List<Slot>();
    public Image crosshair;
    public TMP_Text itemHoverText;

    [Header("Raycast")]
    public float raycastDistance = 5f;
    public LayerMask itemLayer;
    public Transform dropLocation; // The location items will be dropped from.

    [Header("Drag and Drog")]
    public Image dragIconImage;
    private Item currentDraggedItem;
    private int currentDragSlotIndex = -1;

    [Header("Equippable Items")]
    public List<GameObject> equippableItems = new List<GameObject>();
    public Transform selectedItemImage;



    public void Start()
    {
        Cursor.visible = false;
        toggleInventory(false);

        foreach(Slot uiSlot in InventorySlots)
        {
            uiSlot.initializeSlot();
        }
    }

    public void Update()
    {
        itemRaycast(Input.GetKeyDown(KeyCode.E));

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            toggleInventory(!inventory.activeInHierarchy);
        }

        if(inventory.activeInHierarchy && Input.GetMouseButtonDown(0)) 
        {
            Debug.Log("DragInventory");
            dragInventoryIcon();
        }
        else if(currentDragSlotIndex != -1 && Input.GetMouseButtonUp(0) || currentDragSlotIndex != -1 && !inventory.activeInHierarchy) // If hovered, and inventory closes. put it back 
        {
            Debug.Log("DropInventory");
            dropInventoryIcon();
        }

        dragIconImage.transform.position = Input.mousePosition;

    }

    public void itemRaycast(bool hasClicked = false)
    {
        itemHoverText.text = "";
        Ray ray = Camera.main.ScreenPointToRay(crosshair.transform.position);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, raycastDistance, itemLayer))
        {
            if (hit.collider != null)
            {
                if (hasClicked) // Pick upo
                {
                    Item newItem = hit.collider.GetComponent<Item>();
                    if (newItem)
                    {
                        addItemToInventory(newItem);
                    }

                }
                else// Get the name
                {
                    Item newItem = hit.collider.GetComponent<Item>();

                    if (newItem)
                    {
                        itemHoverText.text = newItem.name;
                    }
                }

            }
        }
    }

    private void addItemToInventory(Item itemToAdd)
    {
        int leftoverQuantity = itemToAdd.currentQuantity;
        Slot openSlot = null;
        for(int i = 0; i < InventorySlots.Count; i++)
        {
            Item heldItem = InventorySlots[i].getItem();

            if (heldItem != null && itemToAdd.name == heldItem.name)
            {
                int freeSpaceInSlot = heldItem.maxQuantity - heldItem.currentQuantity;

                if(freeSpaceInSlot >= leftoverQuantity)
                {
                    heldItem.currentQuantity += leftoverQuantity;
                    Destroy(itemToAdd.gameObject);
                    InventorySlots[i].updateData();
                    return;
                }
                else // add as much as we can addd to the current slot
                {
                    heldItem.currentQuantity = heldItem.maxQuantity;
                    leftoverQuantity -= freeSpaceInSlot;
                }
            }
            else if(heldItem == null)
            {
                if (!openSlot)
                {
                    openSlot = InventorySlots[i];
                }
            }
            InventorySlots[i].updateData();
        }
        if(leftoverQuantity > 0 && openSlot)
        {
            openSlot.setItem(itemToAdd);
            itemToAdd.currentQuantity = leftoverQuantity;
            itemToAdd.gameObject.SetActive(false);
        }
        else
        {
            itemToAdd.currentQuantity = leftoverQuantity;

        }
    }


    private void toggleInventory(bool enable)
    {
        inventory.SetActive(enable);

        Cursor.lockState = enable ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = enable;

        //disable the rotation of the camera
        // basically turn sensitivity and roration to 0.
    }

    private void dragInventoryIcon()
    {
        for (int i = 0; i < InventorySlots.Count; i++)
        {
            Debug.Log("Dragged1");
            Slot curSlot = InventorySlots[i];
            if (curSlot.hovered && curSlot.hasItem())
            {
                Debug.Log("Dragged2");
                currentDragSlotIndex = i; // update the current drag slot index variable.

                currentDraggedItem = curSlot.getItem(); // get the item from the current inventory slot
                dragIconImage.sprite = currentDraggedItem.icon;
                dragIconImage.color = new Color(1,1,1,1); // makes the follow mouse incon opaque

                curSlot.setItem(null); // removes item from current slot.
            }
        }
    }

    private void dropInventoryIcon()
    {
        dragIconImage.sprite = null;
        dragIconImage.color = new Color(1,1,1,0);

        for (int i = 0; i < InventorySlots.Count; i++)
        {
            Slot curSlot = InventorySlots[i];
            if (curSlot.hovered) // swap two pos of items.
            {
                Item itemToSwap = curSlot.getItem();

                curSlot.setItem(currentDraggedItem);

                InventorySlots[currentDragSlotIndex].setItem(itemToSwap);

                resetDragVariables();
                return;
            }
            else // place the item with no swapping
            {
                curSlot.setItem(currentDraggedItem);
                resetDragVariables();
                return;
            }
        }

        // if we get to his point we dropped the item in an invalid location or closed inv.
        InventorySlots[currentDragSlotIndex].setItem(currentDraggedItem);
        resetDragVariables();

    }

    private void resetDragVariables()
    {
        currentDraggedItem = null;
        currentDragSlotIndex = -1;
    }


}
