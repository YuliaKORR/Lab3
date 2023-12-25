using Lab3.Models;

namespace Lab3.Interfaces
{
    public interface IShopService
    {
        void CreateShop(string id_shop, string shopName, string address);
        void ItemsList(string id_shop, List<ItemInShop> items);
        List<Item> GetItemsByShop(string id_shop);
        List<Item> GetAffordableItems(string id_shop, decimal budget);
        decimal PurchaseItemsInShop(string id_shop, List<ItemInShop> items);
        string FindCheapestShopForItem(string itemName);
        string FindCheapestShopForItems(List<ItemInShop> items);
    }
}
