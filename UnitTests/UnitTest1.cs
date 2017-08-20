using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Web.Mvc;
using WebUI.Models;
using System;
using WebUI.HtmlHelpers;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // przygotowanie
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"}
                });
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // działanie
            ProductsListViewModel result =
            (ProductsListViewModel)controller.List(2).Model;

            // asercje
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper html = null;
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            Func<int, string> pageUrlDelegate = i => "Strona" + i;

            MvcHtmlString result = html.PageLinks(pagingInfo, pageUrlDelegate);

            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Strona1"">1</a>"
            + @"<a class=""btn btn-default btn-primary selected"" href=""Strona2"">2</a>"
            + @"<a class=""btn btn-default"" href=""Strona3"">3</a>", result.ToString());

        }
    }
}