using Lab3.File.Services;
using Lab3.Interfaces;
using Lab3.Models;
using Lab3.SQL.Services;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;
using System.Data;
using System.Configuration;
using System.Collections.Specialized;
using Lab3.SQL.Repositories;
using Lab3;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

class Program
{
    private static IShopRepository shopRepository;
    private static IItemRepository itemRepository;
    private static IShopService shopService;

    private static readonly ApplicationDbContext _db = new ApplicationDbContext();

    static async Task ConnectionToSQL()
    {
        string connectionString = "Server=LAPTOP-FKQ2OA17\\SQLEXPRESS01; Encrypt=False; User=LAPTOP-FKQ2OA17\\yulia; Initial Catalog=ThirdLab; Database=ThirdLab; Integrated Security=SSPI; TrustServerCertificate=True";

        SqlConnection connection = new SqlConnection(connectionString);
        try
        {
            await connection.OpenAsync();    
        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    private static IShopService CreateShop(string TypeOfConnection)
    {
        Console.WriteLine($"Имплементация из .property: {TypeOfConnection}");
        if (TypeOfConnection == "file")
        {
            InitializeRepositories();
            InitializeServices();
            return new ShopService(shopRepository, itemRepository);
        }
        else if (TypeOfConnection == "sql")
        {          
            ConnectionToSQL();
            InitializeSQLRepositories();
            return new SQLShopService(shopRepository, itemRepository);
        }
        else
        {
            throw new ArgumentException("Недопустимый тип реализации", nameof(TypeOfConnection));
        }
    }
    public static void Main()
    {
        string typeOfConnection = System.Configuration.ConfigurationManager.AppSettings.Get("TypeOfConnection");
        shopService = CreateShop(typeOfConnection);

        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. Создать новый магазин");
            Console.WriteLine("2. Завезти партию товаров в магазин");
            Console.WriteLine("3. Просмотреть товары в магазине");
            Console.WriteLine("4. Найти магазин, в котором определенный товар самый дешевый");
            Console.WriteLine("5. Получить товары, которые можно купить в магазине на некоторую сумму");
            Console.WriteLine("6. Купить партию товаров в магазине");
            Console.WriteLine("7. Найти самый дешевый магазин для партии товаров");
            Console.WriteLine("8. Вывести список магазинов");
            Console.WriteLine("9. Выйти");

            Console.Write("Выберите действие (1-10): ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                default:
                    Console.WriteLine("Неверный ввод. Пожалуйста, выберите от 1 до 10.");
                    break;
                case "1":
                    CreateNewShop();
                    break;
                case "2":
                    ImportItemsToShop();
                    break;
                case "3":
                    ViewItemsInShop();
                    break;
                case "4":
                    FindCheapestShopForItem();
                    break;
                case "5":
                    GetAffordableItemsInShop();
                    break;
                case "6":
                    PurchaseProductsInStore();
                    break;
                case "7":
                    FindCheapestShopForItemSet();
                    break;
                case "8":
                    DisplayAllShops();
                    break;
                case "9":
                    Environment.Exit(0);
                    break;
            }

            Console.WriteLine("Нажмите Enter для продолжения...");
            Console.ReadLine();
        }
    }

    private static void InitializeRepositories()
    {
        shopRepository = new ShopRepository();
        itemRepository = new ItemRepository();
    }
    private static void InitializeServices()
    {
        shopService = new ShopService(shopRepository, itemRepository);
    }

    private static void InitializeSQLRepositories()
    {
        shopRepository = new SQLShopRepository(_db);
        itemRepository = new SQLItemRepository(_db);
    }
    private static void InitializeSQLServices()
    {
        shopService = new SQLShopService(shopRepository, itemRepository);
    }
    
    private static void CreateNewShop()
    {
        Console.Write("Введите код нового магазина: ");
        string id_shop = Console.ReadLine();

        Console.Write("Введите имя нового магазина: ");
        string Name = Console.ReadLine();

        Console.Write("Введите адрес нового магазина: ");
        string Address = Console.ReadLine();

        shopService.CreateShop(id_shop, Name, Address);

        Console.WriteLine("Магазин успешно создан!");
    }

    private static void DisplayAllShops()
    {
        Console.WriteLine("Информация о всех магазинах:");

        var shops = shopRepository.GetAllShops();

        if (shops.Count > 0)
        {
            foreach (var shop in shops)
            {
                Console.WriteLine($"Код: {shop.Id}, Название: {shop.Name}, Адрес: {shop.Address}");
            }
        }
        else
        {
            Console.WriteLine("Магазины не найдены.");
        }
    }

    private static void AddItemToShop()
    {
        Console.Write("Введите код магазина: ");
        string id_shop = Console.ReadLine();

        Console.Write("Введите имя товара: ");
        string itemName = Console.ReadLine();

        Console.Write("Введите количество товара: ");
        int count = int.Parse(Console.ReadLine());

        Console.Write("Введите стоимость товара: ");
        decimal price = decimal.Parse(Console.ReadLine());

        List<ItemInShop> items = new List<ItemInShop>
        {
            new ItemInShop { Name = itemName, Count = count, Price = price }
        };

        shopService.ItemsList(id_shop, items);

        Console.WriteLine("Товар успешно добавлен в магазин!");
    }

    private static void ViewItemsInShop()
    {
        Console.Write("Введите код магазина: ");
        string id_shop = Console.ReadLine();
  

        List<Item> items = shopService.GetItemsByShop(id_shop);

        Console.WriteLine($"Товары в магазине с кодом {id_shop}:");
        foreach (var item in items)
        {
            Console.WriteLine($"{item.Name} - {item.Count} шт. - Цена: {item.Price}");
        }
    }

    private static void ImportItemsToShop()
    {
       Console.Write("Введите код магазина: ");
        string id_shop = Console.ReadLine();

        Console.Write("Введите название товара: ");
        string itemName = Console.ReadLine();

        Console.Write("Введите количество товара: ");
        int count = int.Parse(Console.ReadLine());

        Console.Write("Введите стоимость товара: ");
        decimal price = decimal.Parse(Console.ReadLine());

        List<ItemInShop> items = new List<ItemInShop>
        {
            new ItemInShop { Name = itemName, Count = count, Price = price }
        };

        shopService.ItemsList(id_shop, items);

        Console.WriteLine("Товары успешно добавлены в магазин!");
    }

    private static void FindCheapestShopForItem()
    {
        Console.Write("Введите название товара: ");
        string itemName = Console.ReadLine();
        string cheapestShop = shopService.FindCheapestShopForItem(itemName);
        if (cheapestShop != null)
        {
            Console.WriteLine($"Самый дешевый магазин для товара '{itemName}': {cheapestShop}");
        }
        else
        {
            Console.WriteLine($"Товар '{itemName}' не найден в магазинах.");
        }
    }

    private static void GetAffordableItemsInShop()
    {
        Console.Write("Введите код магазина: ");
        string id_shop = Console.ReadLine();

        Console.Write("Введите бюджет: ");
        decimal budget = decimal.Parse(Console.ReadLine());
        List<Item> affordableItems = shopService.GetAffordableItems(id_shop, budget);
        Shop shop = shopRepository.GetShopById(id_shop);
        if (affordableItems.Count > 0)
        {
            Console.WriteLine($"Товары, которые можно купить в магазине '{shop.Name}' на бюджет {budget} рублей:");
            foreach (var item in affordableItems)
            {
                Console.WriteLine($"{item.Name} - {item.Count} шт. - Цена: {item.Price}");
            }
        }
        else
        {
            Console.WriteLine($"В магазине '{shop.Name}' нет товаров, соответствующих бюджету.");
        }
    }

    private static void PurchaseProductsInStore()
    {
        Console.Write("Введите код магазина: ");
        string id_shop = Console.ReadLine();


        Console.Write("Введите количество разных товаров в партии: ");
        int numberOfItems = int.Parse(Console.ReadLine());

        List<ItemInShop> items = new List<ItemInShop>();
        for (int i = 0; i < numberOfItems; i++)
        {
            Console.Write($"Введите название товара {i + 1}: ");
            string itemName = Console.ReadLine();

            Console.Write($"Введите количество товара {i + 1}: ");
            int count = int.Parse(Console.ReadLine());

            items.Add(new ItemInShop { Name = itemName, Count = count });
        }

        decimal totalCost = shopService.PurchaseItemsInShop(id_shop, items);

        if (totalCost >= 0)
        {
            Console.WriteLine($"Покупка успешно совершена. Общая стоимость: {totalCost} рублей");
        }
        else
        {
            Console.WriteLine($"Покупка не удалась. Недостаточно товаров в магазине.");
        }
    }

    private static void FindCheapestShopForItemSet()
    {
        Console.Write("Введите количество разных товаров в партии: ");
        int numberOfItems = int.Parse(Console.ReadLine());

        List<ItemInShop> items = new List<ItemInShop>();
        for (int i = 0; i < numberOfItems; i++)
        {
            Console.Write($"Введите название товара {i + 1}: ");
            string itemName = Console.ReadLine();

            Console.Write($"Введите количество товара {i + 1}: ");
            int count = int.Parse(Console.ReadLine());

            items.Add(new ItemInShop { Name = itemName, Count = count });
        }

        string cheapestShop = shopService.FindCheapestShopForItems(items);

        if (cheapestShop != null)
        {
            Console.WriteLine($"Самый дешевый магазин для данной партии товаров: {cheapestShop}");
        }
        else
        {
            Console.WriteLine("Для данной партии товаров магазины не найдены.");
        }
    }
}
