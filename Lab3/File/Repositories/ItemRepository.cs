using Lab3.Interfaces;
using Lab3.Models;

public class ItemRepository : IItemRepository
{
    private const string ItemsFilePath = "items.csv";
    private List<Item> _items; 

    public ItemRepository()
    {
        _items = ReadItemsFromFile();
    }

    public void CreateItem(Item item)
    {
        _items.Add(item);
        WriteItemsToFile(_items);
    }

    public Item GetItemInShopByName(string itemName, string id_shop)
    {
        return _items.FirstOrDefault(i => i.Name == itemName && i.Id == id_shop);
    }
    public List<Item> GetItemsByShopCode(string id_shop)
    {
        return _items.Where(i => i.Id == id_shop).ToList();
    }

    public void UpdateItem(Item item)
    {
        Item existingItem = _items.FirstOrDefault(i =>
            i.Name == item.Name && i.Id == item.Id);

        if (existingItem != null)
        {
            existingItem.Count = item.Count;
            existingItem.Price = item.Price;
        }

        WriteItemsToFile(_items);
    }

    public List<Item> GetItemsByItemName(string itemName)
    {
        return _items.Where(i => i.Name.Contains(itemName)).ToList();
    }

    public void UpdateItems(string id_shop, List<Item> items)
    {
        foreach (var updatedItem in items)
        {
            Item existingItem = _items.FirstOrDefault(i =>
                i.Name == updatedItem.Name && i.Id == id_shop);

            if (existingItem != null)
            {
                existingItem.Count = updatedItem.Count;
                existingItem.Price = updatedItem.Price;
            }
        }

        WriteItemsToFile(_items);
    }
    
    //Чтение, запись файла
    private List<Item> ReadItemsFromFile()
    {
        if (File.Exists(ItemsFilePath))
        {
            var lines = File.ReadAllLines(ItemsFilePath).Skip(1);
            return lines.Select(line => line.Split(','))
                        .Select(parts => new Item
                        {
                            Name = parts[0],
                            Id = parts[1],
                            Count = int.Parse(parts[2]),
                            Price = parts.Length > 3 ? decimal.Parse(parts[3]) : 0.0m
                        })
                        .ToList();
        }
        else
        {
            return new List<Item>();
        }
    }

    private void WriteItemsToFile(List<Item> items)
    {
        var csvLines = new List<string> { "Name,Id,Count,Price" };

        csvLines.AddRange(items.Select(item =>
            $"{item.Name},{item.Id},{item.Count},{item.Price}"));

        File.WriteAllLines(ItemsFilePath, csvLines);
    }
}
