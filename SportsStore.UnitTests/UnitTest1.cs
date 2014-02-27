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
            IEnumerable<Product> result = (IEnumerable<Product>)controller.List(2).Model; 

            // Assert
            Product[] productArray = result.ToArray();
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
    }
}
