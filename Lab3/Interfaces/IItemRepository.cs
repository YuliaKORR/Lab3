using Lab3.Models;

namespace Lab3.Interfaces
{
    public interface IItemRepository
    {
        void CreateItem(Item item);
        List<Item> GetItemsByShopCode(string id_shop);
        void UpdateItem(Item item);
        void UpdateItems(string id_shop, List<Item> items);
        List<Item> GetItemsByItemName(string itemName);
        Item GetItemInShopByName(string itemName, string id_shop);
    }
}
