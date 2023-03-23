using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AbandonMine.Inventory
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField]
        private InventoryItemData[] itemDataList;

        public class InventoryItem
        {
            public string displayName;
            public Sprite icon;
        }

        public static PlayerInventory Instance { get; private set; }

        public delegate void PlayerInventoryHandler();
        public static event PlayerInventoryHandler OnInventoryListUpdatedEvent;

        private List<InventoryItem> itemList = new List<InventoryItem>();

        public void UpdateItemList()
        {
            itemList.Clear();
            PlayFabManager.Instance.GetInventoryItemList(
                playFabItemList =>
                {
                    foreach (var playFabItem in playFabItemList)
                    {
                        var itemData = itemDataList.FirstOrDefault(item => { return string.Equals(item.ItemClass, playFabItem.itemClass); });

                        itemList.Add(new InventoryItem
                        {
                            displayName = playFabItem.itemDisplayName,
                            icon = itemData != null ? itemData.InventoryIcon : null
                        });
                    };

                    OnInventoryListUpdatedEvent?.Invoke();
                });
        }

        public List<InventoryItem> GetItemList()
        {
            return itemList;
        }

        private void Awake()
        {
            if (PlayerInventory.Instance == null)
            {
                PlayerInventory.Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private InventoryItemData GetInventoryItemDataForItemClass(string itemClass)
        {
            return itemDataList.FirstOrDefault(item => { return item.ItemClass == itemClass; });
        }
    }
}
