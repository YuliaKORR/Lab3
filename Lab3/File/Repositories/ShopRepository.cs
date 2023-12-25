using Lab3.Models;

public class ShopRepository : IShopRepository
{
    private const string ShopsFilePath = "shops.csv";

    public void CreateShop(Shop shop)
    {
        var shops = ReadShopsFromFile();

        shops.Add(shop);

        WriteShopsToFile(shops);
    }

    public List<Shop> GetAllShops()
    {
        return ReadShopsFromFile();
    }
    public Shop GetShopById(string id)
    {
        return ReadShopsFromFile().FirstOrDefault(x=> x.Id == id);
    }

    public List<Item> GetItemsByShopCode(string id_shop)
    {
        var lines = File.ReadAllLines(ShopsFilePath).Skip(1);
        var items = lines.Where(line => line.Split(',')[0] == id_shop)
                            .Select(line => line.Split(','))
                            .Select(parts => new Item
                            {
                                Name = parts[1],
                                Count = int.Parse(parts[3]),
                                Price = decimal.Parse(parts[4])
                            })
                            .ToList();
        return items;
    }

    //Запись, чтение файла
    private List<Shop> ReadShopsFromFile()
    {
        if (File.Exists(ShopsFilePath))
        {
            var lines = File.ReadAllLines(ShopsFilePath).Skip(1);
            return lines.Select(line => line.Split(','))
                        .Select(parts => new Shop
                        {
                            Id = parts[0],
                            Name = parts[1],
                            Address = parts[2]
                        })
                        .ToList();
        }
        else
        {
            return new List<Shop>();
        }
    }

    private void WriteShopsToFile(List<Shop> shops)
    {
        var csvLines = new List<string> { "Id,Name,Address" };

        csvLines.AddRange(shops.Select(shop =>
            $"{shop.Id},{shop.Name},{shop.Address}"));

        File.WriteAllLines(ShopsFilePath, csvLines);
    }
}
