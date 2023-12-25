using Lab3.Interfaces;
using Lab3.Models;

namespace Lab3.SQL.Repositories
{
    public class SQLItemRepository : IItemRepository
    {
        private readonly ApplicationDbContext _db;
        public SQLItemRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async void CreateItem(Item item)
        {
            _db.Items.Add(item);
            await _db.SaveChangesAsync();
        }

        public Item GetItemInShopByName(string itemName, string id_shop)
        {
            return _db.Items.FirstOrDefault(i => i.Name == itemName && i.Id == id_shop);
        }
        public List<Item> GetItemsByShopCode(string id_shop)
        {
            return _db.Items.Where(i => i.Id == id_shop).ToList();
        }

        public void UpdateItem(Item item)
        {
            Item existingItem = _db.Items.FirstOrDefault(i => i.Name == item.Name && i.Id == item.Id);

            if (existingItem != null)
            {
                existingItem.Count = item.Count;
                existingItem.Price = item.Price;
            }

            _db.Items.Update(existingItem);
            _db.SaveChanges();
        }

        public List<Item> GetItemsByItemName(string itemName)
        {
            return _db.Items.Where(i => i.Name.Contains(itemName)).ToList();
        }

        public void UpdateItems(string id_shop, List<Item> items)
        {
            foreach (var updatedItem in items)
            {
                Item existingItem = _db.Items.FirstOrDefault(i => i.Name == updatedItem.Name && i.Id == updatedItem.Id);

                if (existingItem != null)
                {
                    existingItem.Count = updatedItem.Count;
                    existingItem.Price = updatedItem.Price;
                    _db.Items.Update(existingItem);
                    _db.SaveChanges();
                }
            }
        }
    }
}
