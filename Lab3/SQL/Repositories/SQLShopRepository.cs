using Lab3.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab3.SQL.Repositories
{
    public class SQLShopRepository : IShopRepository
    {
        private readonly ApplicationDbContext _db;
        public SQLShopRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void CreateShop(Shop shop)
        {
            _db.Shops.Add(shop);
            _db.SaveChanges();
        }

        public List<Shop> GetAllShops()
        {
            return _db.Shops.ToList();
        }

        public List<Item> GetItemsByShopCode(string id_shop)
        {
            return _db.Items.Where(i => i.Id == id_shop).ToList();
        }
        public Shop GetShopById(string id)
        {
            return _db.Shops.FirstOrDefault(x => x.Id == id);
        }
    }
}
