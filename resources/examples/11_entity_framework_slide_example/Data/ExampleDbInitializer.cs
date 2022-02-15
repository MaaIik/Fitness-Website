using System.Collections.Generic;
using System.Linq;

using Example.Models;

namespace Example.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ExampleDbContext context)
        {
            // Delete the database before we initialize it.
            // This is common to do during development.
            context.Database.EnsureDeleted();

            // Make sure the database and tables exist
            context.Database.EnsureCreated();

            // Add test customers
            context.Customers.AddRange(new List<Customer>{
                new Customer("Knut", "Olsen", "knut@olsen.no"),
                new Customer("Lise", "Larsen", "lise@larsen.no")
            });

            // Add test products
            context.Products.AddRange(new List<Product>{
                new Product("Chocolate", 14.99m),
                new Product("Ice cream", 39.99m)
            });

            // Save now so that the customers and products are written to the database
            context.SaveChanges();

            // Get the data we just saved
            var customers = context.Customers.ToList();
            var products = context.Products.ToList();

            // Create test order for Knut
            var order1 = new Order();
            
            order1.Customer = customers[0];
            order1.OrderLines.Add(new OrderLine(products[0], 5));
            order1.OrderLines.Add(new OrderLine(products[1], 1));

            // Create test order for Lise
            var order2 = new Order();

            order2.Customer = customers[1];
            order2.OrderLines.Add(new OrderLine(products[0], 3));
            order2.OrderLines.Add(new OrderLine(products[1], 2));

            // Add test orders
            context.Orders.Add(order1);
            context.Orders.Add(order2);

            // Finally save the orders to the database
            context.SaveChanges();
        }
    }
}
