namespace homework22;

public interface IProductRepository
{
    List<Product> GetAllProducts();
    Product GetProductByName(string name);
    void AddProduct(Product product);
    void RemoveProduct(RemoveProductClass product);
}