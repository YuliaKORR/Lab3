using Lab3.Models;

public interface IShopRepository
{
    void CreateShop(Shop shop);
    List<Item> GetItemsByShopCode(string id_shop);
    List<Shop> GetAllShops();
    Shop GetShopById(string id);
}
