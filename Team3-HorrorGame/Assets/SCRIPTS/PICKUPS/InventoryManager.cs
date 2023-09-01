using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    private List<string> inventory = new List<string>();
    InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
    

    // Start is called before the first frame update
    void Start()
    {
        // adding key to inventory list
        inventoryManager.AddKey("Key");
    }

    // Update is called once per frame
    void Update()
    {
        //check if the player has a key -- could be changed to if player has item
        bool hasSilverKey = inventoryManager.HasKey("Key");
        if (hasSilverKey)
        {
            Debug.Log("You have the Key!");
        }
        else
        {
            Debug.Log("You don't have the Key.");
        }
    }

    public void AddKey(string keyName)
    {
        inventory.Add(keyName);

    }

    public bool HasKey(string keyName)
    {
        return inventory.Contains(keyName);
    }
}
