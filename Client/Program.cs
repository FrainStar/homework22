using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace Client
{

    [Serializable]
    public class Product
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string name { get; set; }

        [Range(0.01, 10000)] 
        public double price { get; set; }

        public Product(string nname, double nprice)
        {
            name = nname;
            price = nprice;
        }

        public Product(string nname)
        {
            name = nname;
        }

        public Product() {}
    }

    public class RemoveProductClass
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string name { get; set; }

        public RemoveProductClass(string nname)
        {
            name = nname;
        }

    }

    public class Program
    {
        private const string BaseUrl = "http://localhost";
        private const string Port = "5109";
        private const string AddProductMethod = "/store/add";
        private const string ShowProductsMethod = "/store/show";
        private const string RemoveProductMethod = "/store/remove";
        private static readonly HttpClient Client = new HttpClient();

        private static void DisplayProducts()
        {
            var url = $"{BaseUrl}:{Port}{ShowProductsMethod}";

            var response = Client.GetAsync(url).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result; 
            var products = JsonSerializer.Deserialize<List<Product>>(responseContent);

            Console.WriteLine("-----------------------------------------------------------------");
            Console.WriteLine("| Название продукта | Цена |");
            Console.WriteLine("-----------------------------------------------------------------");

            foreach (var prod in products)
            {
                Console.WriteLine($"| {prod.name, -18} | {prod.price, -5}"); 
            }

            Console.WriteLine("-----------------------------------------------------------------");
        }

        private static void SendProduct()
        {

            var url = $"{BaseUrl}:{Port}{AddProductMethod}";

            Console.WriteLine("Введите название продукта:");
            var name = Console.ReadLine();

            Console.WriteLine("Введите цену продукта:");
            var price = double.Parse(Console.ReadLine());

            var product = new Product(name,price); 

            var json = JsonSerializer.Serialize(product);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = Client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(responseContent);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }

        private static void DeleteProduct()
        {
            var url = $"{BaseUrl}:{Port}{RemoveProductMethod}";

            Console.WriteLine("Введите название продукта:");
            var name = Console.ReadLine();

            var product = new RemoveProductClass(name); 

            var json = JsonSerializer.Serialize(product);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = Client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(responseContent);
            }
            else
            {
                Console.WriteLine($"Error: {response}");
            }
        }

        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            while (true)
            {
                Console.WriteLine("Выберите опцию:");
                Console.WriteLine("1. Вывести список кофе");
                Console.WriteLine("2. Отправить кофе");
                Console.WriteLine("3. Удалить кофе");
                Console.WriteLine("4. Выйти");
                Console.Write("Введите ваш выбор: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DisplayProducts();
                        break;
                    case "2":
                        SendProduct();
                        break;
                    case "3":
                        DeleteProduct();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }
    }
}
