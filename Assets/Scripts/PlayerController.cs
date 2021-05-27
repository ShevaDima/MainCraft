using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float gravityScale = 9.8f,
                        speedScale = 5f,
                        jumpForce = 8f,
                        turnSpeed = 90f;

    private float verticalSpeed = 0f,
                  mouseX = 0f,
                  mouseY = 0f,
                  currentAngleX = 0f;
    
    private CharacterController controller;
    [SerializeField]
    Camera goCamera;

    [SerializeField]
    GameObject particleObject;
    private GameObject currentEquipedItem;
    [SerializeField]
    private GameObject[] equiptableItems;
    private const float hitScaleSpeed = 15f;
    private float hitLastTime = 0f;

    private InventoryManager inventoryManager;
    public List<ItemData> inventoryItems, currentChestItems;
    private Transform itemParent;
    bool canMove = true;

    private RaycastHit hit;
    public const string equipNotSelected = "EquipNotSelected";
    [HideInInspector]
    public string itemToEquip = equipNotSelected;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();

        itemParent = GameObject.Find("InventoryContent").transform;
        inventoryManager.CreateItem(1, inventoryItems);
    }

    void Start()
    {
        EquipItem("Pickaxe");
    }

    void Update()
    {
        if (canMove)
        {
            RotateCharacter();
            MoveCharacter();
            
            if (Physics.Raycast(goCamera.transform.position, goCamera.transform.forward, out hit, 6f))
            {
                if (Input.GetMouseButton(0))
                {
                    ObjectInteraction(hit.transform.gameObject);
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    ItemAbility();
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.E) && !inventoryManager.inventoryPanel.activeSelf)
        {
            OpenInventory();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseInventoryPanels();
        }
        else if (Input.GetKeyDown(KeyCode.E) && inventoryManager.inventoryPanel.activeSelf && itemToEquip != equipNotSelected)
        {
            EquipItem(itemToEquip);
        }
    }

    void RotateCharacter()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        transform.Rotate(new Vector3(0f, mouseX * turnSpeed * Time.deltaTime, 0f));
        currentAngleX += mouseY * turnSpeed * Time.deltaTime * -1f;
        currentAngleX = Mathf.Clamp(currentAngleX, -90f, 90f);
        goCamera.transform.localEulerAngles = new Vector3(currentAngleX, 0f, 0f);
    }
        
    void MoveCharacter()
    {
        Vector3 velocity = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        velocity = transform.TransformDirection(velocity) * speedScale;

        if (controller.isGrounded)
        {
            verticalSpeed = 0f;
            if (Input.GetButton("Jump"))
            {
                verticalSpeed = jumpForce;
            }
        }

        verticalSpeed -= gravityScale * Time.deltaTime;
        velocity.y = verticalSpeed;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Dig(Block block)
    {
        if (Time.time - hitLastTime > 1 / hitScaleSpeed)
        {
            currentEquipedItem.GetComponent<Animator>().SetTrigger("attack");
            hitLastTime = Time.time;
            Tool currentToolInfo;
            if (currentEquipedItem.TryGetComponent<Tool>(out currentToolInfo))
            {
                block.health -= currentToolInfo.damageToBlock;
            }
            else
            {
                block.health -= 1;
            }
            GameObject go = Instantiate(particleObject, block.gameObject.transform.position, Quaternion.identity);
            go.GetComponent<ParticleSystemRenderer>().material = block.gameObject.GetComponent<MeshRenderer>().material;
            if (block.health <= 0)
            {
                block.DestroyBehaviour();
            }
        }
    }

    private void ObjectInteraction(GameObject tempObject)
    {
        switch(tempObject.tag)
        {
            case "Block":
                Dig(tempObject.GetComponent<Block>());
                break;
            case "Enemy":
               
                break;
            case "Chest":
                currentChestItems = tempObject.GetComponent<Chest>().chestItems;
                OpenChest();
                break;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name.StartsWith("mini"))
        {
            inventoryManager.CreateItem(2, inventoryItems);
            Destroy(col.gameObject);
        }
    }

    private void OpenInventory()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        canMove = false;

        inventoryManager.inventoryPanel.SetActive(true);
        if(inventoryItems.Count > 0)
        {
            for(int i = 0; i < inventoryItems.Count; i++)
            {
                inventoryManager.InstantiatingItem(inventoryItems[i], itemParent, inventoryManager.inventorySlots);
            }
        }
    }

    private void OpenChest()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        canMove = false;

        if(!inventoryManager.chestPanel.activeSelf)
        {
            inventoryManager.chestPanel.SetActive(true);
            Transform itemParent = GameObject.Find("ChestContent").transform;
            for(int i = 0; i < currentChestItems.Count; i++)
            {
                inventoryManager.InstantiatingItem(currentChestItems[i], itemParent, inventoryManager.currentChestSlots);
            }
        }
    }

    private void CloseInventoryPanels()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        canMove = true;

        foreach (GameObject slot in inventoryManager.currentChestSlots)
        {
            Destroy(slot);
        }
        foreach (GameObject slot in inventoryManager.inventorySlots)
        {
            Destroy(slot);
        }
        inventoryManager.currentChestSlots.Clear();
        inventoryManager.inventorySlots.Clear();
        inventoryManager.inventoryPanel.SetActive(false);
        inventoryManager.chestPanel.SetActive(false);
    }

    private void EquipItem(string toolName)
    {
        foreach (GameObject tool in equiptableItems)
        {
            if (tool.name == toolName)
            {
                tool.SetActive(true);
                currentEquipedItem = tool;
                toolName = equipNotSelected;
            }
            else
            {
                tool.SetActive(false);
            }
        }

        
    }

    private void ItemAbility()
    {
        switch(currentEquipedItem.name)
        {
            case "Ground":
                CreateBlock();
                break;
            case "Meat":
                break;
            default:
                break;
        }
    }

    private void CreateBlock()
    {
        GameObject blockPref = Resources.Load<GameObject>("Ground");
        Vector3 tempPos = hit.transform.gameObject.transform.position;
        Vector3 newBlockPos = Vector3.zero;
        if (hit.transform.gameObject.tag == "Block")
        {
            GameObject currentBlock = Instantiate(blockPref);

            if (hit.point.y == tempPos.y + 0.5f)
            {
                newBlockPos = new Vector3(tempPos.x, tempPos.y + 1, tempPos.z);
            }
            else if (hit.point.y == tempPos.y - 0.5f)
            {
                newBlockPos = new Vector3(tempPos.x, tempPos.y - 1, tempPos.z);
            }
            if (hit.point.z == tempPos.z + 0.5f)
            {
                newBlockPos = new Vector3(tempPos.x, tempPos.y, tempPos.z + 1);
            }
            else if (hit.point.z == tempPos.z - 0.5f)
            {
                newBlockPos = new Vector3(tempPos.x, tempPos.y , tempPos.z - 1);
            }
            if (hit.point.x == tempPos.x + 0.5f)
            {
                newBlockPos = new Vector3(tempPos.x + 1, tempPos.y, tempPos.z);
            }
            else if (hit.point.x == tempPos.x - 0.5f)
            {
                newBlockPos = new Vector3(tempPos.x - 1, tempPos.y, tempPos.z);
            }

            currentBlock.transform.position = newBlockPos;
            ModifyItemCount("Ground");
        }
    }

    private void ModifyItemCount(string itemName)
    {
        foreach (ItemData item in inventoryItems)
        {
            if (item.name == itemName)
            {
                item.count--;
                if (item.count <= 0)
                {
                    inventoryItems.Remove(item);
                    EquipItem(inventoryItems[0].name);
                }
                break;
            }
        }
    }
}
