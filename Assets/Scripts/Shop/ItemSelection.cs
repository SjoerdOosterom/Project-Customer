using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSelection : MonoBehaviour
{
    private int currentItem = 0;

    // Store the prices of each item
    public List<float> itemPrices;

    // Store the names of each item
    public List<string> itemNames;

    // Keep track of which items have been bought
    private bool[] itemBought;

    // Reference to the price tag UI (assign in the inspector)
    public TMP_Text priceTagText;

    // Reference to the item name UI (assign in the inspector)
    public TMP_Text itemNameText;

    // Reference to the player's money (assign initial value)
    public float playerMoney = 100f;

    // Reference to the player's money display
    public TMP_Text moneyText;

    // Reference to the Buy Button
    public Button buyButton;

    private void Start()
    {
        // Initialize the bought status for each item
        itemBought = new bool[itemPrices.Count]; // Initialize based on itemPrices list length

        // Initialize all items as not bought
        for (int i = 0; i < itemBought.Length; i++)
        {
            itemBought[i] = false;
        }

        // Initialize by displaying the first item, price, and name
        SelectItem(currentItem);

        // Update the player's money display
        UpdateMoneyText();
    }

    // Method to select and display an item
    private void SelectItem(int _index)
    {
        // Loop through the child items and activate/deactivate them based on the index
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == _index);
        }

        // Update the price tag, item name, and buy button state
        UpdatePriceTag(_index);
        UpdateItemName(_index);
        UpdateBuyButtonState();
    }

    // Method to change the item
    public void ChangeItem(int _change)
    {
        currentItem += _change;

        // Ensure the item index is within bounds
        if (currentItem < 0)
        {
            currentItem = transform.childCount - 1; // Wrap around to the last item
        }
        else if (currentItem >= transform.childCount)
        {
            currentItem = 0; // Wrap around to the first item
        }

        // Select the item and update the price tag, item name, and buy button
        SelectItem(currentItem);
    }

    // Method to update the price tag based on the selected item
    private void UpdatePriceTag(int itemIndex)
    {
        if (itemPrices != null && itemIndex >= 0 && itemIndex < itemPrices.Count)
        {
            priceTagText.text = "$" + itemPrices[itemIndex].ToString("F2");
        }
        else
        {
            priceTagText.text = "Price not available";
        }
    }

    // Method to update the item name based on the selected item
    private void UpdateItemName(int itemIndex)
    {
        if (itemNames != null && itemIndex >= 0 && itemIndex < itemNames.Count)
        {
            itemNameText.text = itemNames[itemIndex];
        }
        else
        {
            itemNameText.text = "Item name not available";
        }
    }

    // Method to update the Buy Button's state
    private void UpdateBuyButtonState()
    {
        // Disable the buy button if the item is already bought or if the player doesn't have enough money
        if (itemBought[currentItem] || playerMoney < itemPrices[currentItem])
        {
            buyButton.interactable = false;
        }
        else
        {
            buyButton.interactable = true;
        }
    }

    // Method to update the player's money display
    private void UpdateMoneyText()
    {
        moneyText.text = "Money: $" + playerMoney.ToString("F2");
    }

    // Method to handle buying an item
    public void BuyItem()
    {
        // Check if the player has enough money to buy the item
        if (playerMoney >= itemPrices[currentItem])
        {
            // Deduct the item's price from the player's money
            playerMoney -= itemPrices[currentItem];

            // Mark the item as bought
            itemBought[currentItem] = true;

            // Update the player's money display
            UpdateMoneyText();

            // Disable the Buy Button for this item
            UpdateBuyButtonState();
        }
    }
}
