using SportsStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.WebUI.Models; 

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int pageSize = 4; 

        public ProductController(IProductRepository productRepository)
        {
            this.repository = productRepository; 
        }

        public ViewResult List(string category, int page = 1)
        {
            ProductsListViewModel model = new ProductsListViewModel {
                Products = repository.Products.Where(p => p.Category == null || p.Category == category).OrderBy(p => p.ProductID).Skip((page - 1) * pageSize).Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize, 
                    TotalItems = repository.Products.Count()
                }, 
                CurrentCategory = category
            };

            return View(model); 

            // First implementation
            //return View(repository.Products.OrderBy(p => p.ProductID).Skip((page - 1) * pageSize).Take(pageSize)); 
        }

        // TODO: continue implementing pagination
	}
}