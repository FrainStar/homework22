namespace homework22;

using System.Data.SQLite; 
using System.Collections.Generic; 


public class SQLiteProductRepository : IProductRepository
{
    private readonly string _connectionString;

    private List<Product> _products = new List<Product>();

    private const string CreateTableQuery = @"
    CREATE TABLE IF NOT EXISTS Products (
        Id INTEGER PRIMARY KEY,
        Name TEXT NOT NULL,
        Price REAL NOT NULL)";

    public SQLiteProductRepository(string connection)
    {
        _connectionString = connection;
        InitializeDatabase();
        ReadDataFromDatabase();
    }

    private void InitializeDatabase()
    {
        SQLiteConnection connection = new SQLiteConnection(_connectionString); 
        Console.WriteLine("База данных :  " + _connectionString + " создана");
        connection.Open();
        SQLiteCommand command = new SQLiteCommand(CreateTableQuery, connection);
        command.ExecuteNonQuery();
    }

    private void ReadDataFromDatabase()
    {
        _products = GetAllProducts();
    }

    public List<Product> GetAllProducts()
    {
        List<Product> products = new List<Product>();
        using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Products";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product product = new Product(reader["Name"].ToString(), Convert.ToDouble(reader["Price"])); 
                        products.Add(product);
                    }
                }
            }
        }
        return products;
    }

    public void AddProduct(Product product)
    {
        using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            string query = "INSERT INTO Products (Name, Price) VALUES (@Name, @Price)";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.ExecuteNonQuery();
            }
        }
    }

    public void RemoveProduct(RemoveProductClass product)
    {
        using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            string query = "DELETE FROM Products WHERE Name = @Name";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", product.Name);
                command.ExecuteNonQuery();
            }
        }
    }

    public Product GetProductByName(string name)
    {
        using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Products WHERE Name = @Name";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {

                        Product product = new Product(reader["Name"].ToString().ToUpper(), Convert.ToDouble(reader["Price"]));
                        return product;
                    }
                    return null;
                }
            }
        }
    }
}