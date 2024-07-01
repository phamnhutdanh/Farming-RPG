using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_UI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Player player;
    public List<Slot_UI> slots = new List<Slot_UI>();

    [SerializeField]
    private Canvas canvas;

    private Slot_UI draggedSlot;
    private Image draggedIcon;
    private bool dragSingle;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
    }

    void Start()
    {
        inventoryPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
            // Refresh();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            dragSingle = true;
        } 
        else
        {
            dragSingle = false;
        }
    }

    public void ToggleInventory()
    {
        if (!inventoryPanel.activeSelf)
        {
            inventoryPanel.SetActive(true);
            Refresh();
        } else
        {
            inventoryPanel.SetActive(false);
        }
    }

    public void Refresh()
    {
        if (slots.Count == player.inventory.slots.Count)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (player.inventory.slots[i].itemName != "")
                {
                    slots[i].SetItem(player.inventory.slots[i]);
                }
                else
                {
                    slots[i].SetEmpty();
                }
            }
        }
    }

    public void Remove()
    {
        Item itemToFrop = GameManager.instance.itemManager.GetItemByName(player.inventory.slots[draggedSlot.slotID].itemName);
        if (itemToFrop != null)
        {
            if (dragSingle)
            {
                player.DropItem(itemToFrop);
                player.inventory.Remove(draggedSlot.slotID);
            } else
            {
                player.DropItem(itemToFrop, player.inventory.slots[draggedSlot.slotID].count);
                player.inventory.Remove(draggedSlot.slotID, player.inventory.slots[draggedSlot.slotID].count);
            }
         
            Refresh();
        }
        draggedSlot = null;
    }

    public void SlotBeginDrag(Slot_UI slot)
    {
        draggedSlot = slot;
        draggedIcon = Instantiate(draggedSlot.itemIcon);
        draggedIcon.transform.SetParent(canvas.transform);
        draggedIcon.raycastTarget = false;
        draggedIcon.rectTransform.sizeDelta = new Vector2(50, 50);

        MoveToMousePosition(draggedIcon.gameObject);
    }

    public void SlotDrag()
    {
        MoveToMousePosition(draggedIcon.gameObject);
    }

    public void SlotEndDrag()
    {
        Destroy(draggedIcon.gameObject);
        draggedIcon = null;
    }

    public void SlotDrop(Slot_UI slot)
    {
         
    }

    private void MoveToMousePosition(GameObject toMove)
    {
        if (canvas != null)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                Input.mousePosition, null, out position
                );
            toMove.transform.position = canvas.transform.TransformPoint(position);
        }
    }
}
