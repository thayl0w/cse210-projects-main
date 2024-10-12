using System;
using System.Collections.Generic;

// Product class
public class Product
{
    private string _name;
    private string _productId;
    private decimal _price;
    private int _quantity;

    public Product(string name, string productId, decimal price, int quantity)
    {
        _name = name;
        _productId = productId;
        _price = price;
        _quantity = quantity;
    }

    public decimal GetTotalCost()
    {
        return _price * _quantity;
    }

    public string GetName()
    {
        return _name;
    }

    public string GetProductId()
    {
        return _productId;
    }
}

// Address class
public class Address
{
    private string _street;
    private string _city;
    private string _state;
    private string _country;

    public Address(string street, string city, string state, string country)
    {
        _street = street;
        _city = city;
        _state = state;
        _country = country;
    }

    public bool IsInUSA()
    {
        return _country.ToLower() == "usa";
    }

    public string GetFullAddress()
    {
        return $"{_street}\n{_city}, {_state}\n{_country}";
    }
}

// Customer class
public class Customer
{
    private string _name;
    private Address _address;

    public Customer(string name, Address address)
    {
        _name = name;
        _address = address;
    }

    public bool IsInUSA()
    {
        return _address.IsInUSA();
    }

    public string GetName()
    {
        return _name;
    }

    public string GetAddress()
    {
        return _address.GetFullAddress();
    }
}

// Order class
public class Order
{
    private List<Product> _products;
    private Customer _customer;

    public Order(Customer customer, List<Product> products)
    {
        _customer = customer;
        _products = products;
    }

    public decimal GetTotalCost()
    {
        decimal totalCost = 0;
        foreach (Product product in _products)
        {
            totalCost += product.GetTotalCost();
        }
        decimal shippingCost = _customer.IsInUSA() ? 5 : 35;
        return totalCost + shippingCost;
    }

    public string GetPackingLabel()
    {
        string label = "Packing Label:\n";
        foreach (Product product in _products)
        {
            label += $"{product.GetName()} (ID: {product.GetProductId()})\n";
        }
        return label;
    }

    public string GetShippingLabel()
    {
        return $"Shipping Label:\n{_customer.GetName()}\n{_customer.GetAddress()}";
    }
}

// Main program
class Program
{
    static void Main(string[] args)
    {
        // Create two address instances
        Address address1 = new Address("123 Knockout St", "Manila", "NCR", "Philippines");
        Address address2 = new Address("456 Champion Ave", "Los Angeles", "CA", "USA");

        // Create two customer instances
        Customer customer1 = new Customer("Manny Pacquiao", address1);
        Customer customer2 = new Customer("Jinkee Pacquiao", address2);

        // Create product instances
        Product product1 = new Product("Boxing Gloves", "A001", 99.99m, 2);
        Product product2 = new Product("Speed Bag", "B002", 59.50m, 1);
        Product product3 = new Product("Jump Rope", "C003", 15.75m, 3);

        // Create two orders
        Order order1 = new Order(customer1, new List<Product> { product1, product3 });
        Order order2 = new Order(customer2, new List<Product> { product2, product3 });

        // Display packing and shipping labels, and total cost
        Console.WriteLine(order1.GetPackingLabel());
        Console.WriteLine(order1.GetShippingLabel());
        Console.WriteLine($"Total Cost: ${order1.GetTotalCost():0.00}\n");

        Console.WriteLine(order2.GetPackingLabel());
        Console.WriteLine(order2.GetShippingLabel());
        Console.WriteLine($"Total Cost: ${order2.GetTotalCost():0.00}");
    }
}
