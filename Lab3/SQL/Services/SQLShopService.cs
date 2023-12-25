using Lab3.Interfaces;
using Lab3.Models;

namespace Lab3.SQL.Services
{
    public class SQLShopService : IShopService
    {
        private readonly IShopRepository _shopRepository;
        private readonly IItemRepository _itemRepository;

        public SQLShopService(IShopRepository shopRepository, IItemRepository itemRepository)
        {
            _shopRepository = shopRepository;
            _itemRepository = itemRepository;
        }

        public List<Item> GetItemsByShop(string id_shop)
        {
            return _itemRepository.GetItemsByShopCode(id_shop);
        }
        public void CreateShop(string id_shop, string shopName, string address)
        {
            Shop newShop = new Shop		

            {
                Id = id_shop,
                Name = shopName,
                Address = address
            };

            _shopRepository.CreateShop(newShop);
        }
        public List<Item> GetAffordableItems(string id_shop, decimal budget)
        {
            List<Item> allItems = _itemRepository.GetItemsByShopCode(id_shop);
            List<Item> affordableItems = new List<Item>();

            foreach (var item in allItems)
            {
                decimal unitPrice = item.Price;

                if (unitPrice > 0)
                {
                    int affordableCount = (int)Math.Floor(budget / unitPrice);

                    if (affordableCount > 0)
                    {
                        affordableItems.Add(new Item
                        {
                            Name = item.Name,
                            Count = affordableCount,
                            Price = unitPrice
                        });
                    }
                }
            }

            return affordableItems;
        }
        public string FindCheapestShopForItem(string itemName)
        {
            Dictionary<string, decimal> shopTotalCosts = new Dictionary<string, decimal>();
            List<Item> availableItems = _itemRepository.GetItemsByItemName(itemName);

            if (!shopTotalCosts.Any())
            {
                Console.WriteLine($"Товар '{itemName}' не найден в магазинах.");
                return null;
            }

            foreach (var availableItem in availableItems)
            {
                decimal totalCost = availableItem.Count * availableItem.Price;

                if (!shopTotalCosts.ContainsKey(availableItem.Id))
                {
                    shopTotalCosts[availableItem.Id] = totalCost;
                }
                else
                {
                    shopTotalCosts[availableItem.Id] += totalCost;
                }
            }

            if (shopTotalCosts.Count > 0)
            {
                string cheapestShop = shopTotalCosts.OrderBy(kv => kv.Value).First().Key;
                Shop shop = _shopRepository.GetShopById(cheapestShop);
                if (shop != null)
                {
                    return shop.Name;
                }
                return null;
            }

            return null;
        }
        public string FindCheapestShopForItems(List<ItemInShop> items)
        {
            Dictionary<string, decimal> shopTotalCosts = new Dictionary<string, decimal>();

            foreach (var item in items)
            {
                List<Item> availableItems = _itemRepository.GetItemsByItemName(item.Name);

                if (!availableItems.Any())
                {
                    Console.WriteLine($"Товар '{item.Name}' не найден в магазинах.");
                    return null;
                }

                foreach (var availableItem in availableItems)
                {
                    decimal totalCost = item.Count * availableItem.Price;

                    if (!shopTotalCosts.ContainsKey(availableItem.Id))
                    {
                        shopTotalCosts[availableItem.Id] = totalCost;
                    }
                    else
                    {
                        shopTotalCosts[availableItem.Id] += totalCost;
                    }
                }
            }

            if (shopTotalCosts.Count > 0)
            {
                string cheapestShop = shopTotalCosts.OrderBy(kv => kv.Value).First().Key;
                Shop shop = _shopRepository.GetShopById(cheapestShop);
                if (shop != null)
                {
                    return shop.Name;
                }
                return null;
            }

            return null;
        }
        public decimal PurchaseItemsInShop(string id_shop, List<ItemInShop> items)
        {
            List<Item> availableItems = _itemRepository.GetItemsByShopCode(id_shop);
            decimal totalCost = 0.0m;

            foreach (var item in items)
            {
                Item availableItem = availableItems.FirstOrDefault(i => i.Name == item.Name);

                if (availableItem == null || availableItem.Count < item.Count)
                {
                    return -1;
                }
                totalCost += item.Count * availableItem.Price;
                availableItem.Count -= item.Count;
            }
            _itemRepository.UpdateItems(id_shop, availableItems);

            return totalCost;
        }
        public void ItemsList(string id_shop, List<ItemInShop> items)
        {
            foreach (var item in items)
            {
                // Проверка существующего товара
                Item existingItem = _itemRepository.GetItemInShopByName(item.Name, id_shop);

                if (existingItem != null)
                {
                    // Товар уже существует, увеличиваем количество
                    existingItem.Count += item.Count;
                    existingItem.Price = item.Price;
                    _itemRepository.UpdateItem(existingItem);
                }
                else
                {
                    // Товар не существует, создаем новый
                    Item newItem = new Item
                    {
                        Name = item.Name,
                        Id = id_shop,
                        Count = item.Count,
                        Price = item.Price
                    };

                    _itemRepository.CreateItem(newItem);
                }
            }
        }
    }
}
