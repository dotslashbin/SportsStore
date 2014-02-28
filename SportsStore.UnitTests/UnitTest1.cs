using System;
using System.Web.Mvc; 
using System.Collections.Generic;
using System.Linq; 
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;
using SportsStore.WebUI.HtmlHelpers; 

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { ProductID = 1, Name = "p1"}, 
                new Product { ProductID = 2, Name = "p2"}, 
                new Product { ProductID = 3, Name = "p3"}, 
                new Product { ProductID = 4, Name = "p4"}, 
                new Product { ProductID = 5, Name = "p5"}
                //new Product { ProductID = 6, Name = "p6"}, 
                //new Product { ProductID = 7, Name = "p7"}, 
                //new Product { ProductID = 8, Name = "p8"}, 
            });

            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            // Act

            // First Implementation
            // IEnumerable<Product> result = (IEnumerable<Product>)controller.List(2).Model; 
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model; 

            // Assert
            Product[] productArray = result.Products.ToArray(); 
            Assert.IsTrue(productArray.Length == 2);
            Assert.AreEqual(productArray[0].Name, "p4");
            Assert.AreEqual(productArray[1].Name, "p5"); 
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper htmlHelper = null;

            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            }; 

            Func<int, string> pageUrlDelegate = i => "Page" + i;

            MvcHtmlString result = htmlHelper.PageLinks(pagingInfo, pageUrlDelegate); 

            // Assert 
            //Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1<a/>" + @"<a class=""btn btn-default"" hnref=""Page2"">2</a>" + @"<a class=""btn btn-default"" hnref=""Page3"">3</a>", result.ToString()); 
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { ProductID = 1, Name = "p1"}, 
                new Product { ProductID = 2, Name = "p2"}, 
                new Product { ProductID = 3, Name = "p3"}, 
                new Product { ProductID = 4, Name = "p4"}, 
                new Product { ProductID = 5, Name = "p5"}
            });

            // Arrange
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3; 

            // Act
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model; 

            // Assert 
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2); 
        }

        [TestMethod]
        public void Can_filter_Products()
        {
            // Arrange - Create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { ProductID = 1, Name = "P1", Category = "Cat1" },
                new Product { ProductID = 2, Name = "P2", Category = "Cat2" },
                new Product { ProductID = 3, Name = "P3", Category = "Cat1" },
                new Product { ProductID = 4, Name = "P4", Category = "Cat2" },
                new Product { ProductID = 5, Name = "P5", Category = "Cat3" },
            });

            // Arrange - Create controller and make the page size 3 items
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3; 

            // Action
            Product[] result = ((ProductsListViewModel)controller.List("Cat2", 1).Model).Products.ToArray(); 

            //Assert
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "P4" && result[1].Category == "Cat2"); 
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            // Arrange - create mock respository
            Mock<IProductRepository> mock = new Mock<IProductRepository>(); 
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { ProductID = 1, Name = "P1", Category = "Cat1" },
                new Product { ProductID = 2, Name = "P2", Category = "Cat2" },
                new Product { ProductID = 3, Name = "P3", Category = "Cat1" },
                new Product { ProductID = 4, Name = "P4", Category = "Cat2" },
                new Product { ProductID = 5, Name = "P5", Category = "Cat3" },
            }); 

            // Arrange - Create the controller
            NavController target = new NavController(mock.Object); 

            // Act - get the result of categories
            string[] results = ((IEnumerable<string>)target.Menu().Model).ToArray(); 

            // Assert
            Assert.AreEqual(results.Length, 3); 
            Assert.AreEqual(results[0], "Cat1"); 
            Assert.AreEqual(results[1], "Cat2"); 
            Assert.AreEqual(results[2], "Cat3"); 
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            // Arrange - create mockup repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { ProductID = 1, Name = "P1", Category = "Apples"}, 
                new Product { ProductID = 2, Name = "P2", Category = "Bananas"}, 
            }); 

            // Arrange - create controller
            NavController target = new NavController(mock.Object); 
            
            // Arrange -define the category to selected
            string categoryToSelect = "Apples"; 

            // Action
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory; 

            // Assert 
            Assert.AreEqual(categoryToSelect, result); 
           
        }

        [TestMethod]
        public void Generate_Cateogry_Specific_Product_Count()
        {
            // Arrange - Create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { ProductID = 1, Name = "p1", Category = "Cat1" },
                new Product { ProductID = 2, Name = "p2", Category = "Cat2" },
                new Product { ProductID = 3, Name = "p3", Category = "Cat1" },
                new Product { ProductID = 4, Name = "p4", Category = "Cat2" },
                new Product { ProductID = 5, Name = "p5", Category = "Cat2" }
            });

            // Arrange - define controller 
            ProductController target = new ProductController(mock.Object);

            int result1 = ((ProductsListViewModel)target.List("Cat1").Model).PagingInfo.TotalItems;
            int result2 = ((ProductsListViewModel)target.List("Cat2").Model).PagingInfo.TotalItems;
            int result3 = ((ProductsListViewModel)target.List(null).Model).PagingInfo.TotalItems; 

            // Assert 
            Assert.AreEqual(result1, 2);
            Assert.AreEqual(result2, 3);
            Assert.AreEqual(result3, 5); 

        }
    }
}
