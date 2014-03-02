using System;
using System.Linq; 
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // Arrange - Create some cart test products
            Product product1 = new Product{ ProductID = 1, Name = "Product1" };
            Product product2 = new Product { ProductID = 2, Name = "Product2" };

            // Arrange - create a new cart
            Cart target = new Cart();

            target.AddItem(product1, 1);
            target.AddItem(product2, 1);

            CartLine[] results = target.Lines.Lines.ToArray(); 


            // Assert
            //Assert.AreEqual(results.Length, 2);
            //Assert.AreEqual(results[0].Product, product1);
            //Assert.AreEqual(results[1].Product, product2);
            
        }
    }
}
