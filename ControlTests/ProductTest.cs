
using AutoMapper;
using BusinessLogic.Interface;
using DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ControlTests
{
    [TestFixture]
    public class ProductTests
    {
        private Mock<IProductsManager> _productsManagerMock;
        private Mock<IMapper> _mapperMock;
        private ProductController _controller;

        [SetUp]
        public void SetUp()
        {
            _productsManagerMock = new Mock<IProductsManager>();
            _mapperMock = new Mock<IMapper>();
            _controller = new ProductController(_productsManagerMock.Object, _mapperMock.Object);
        }

        [Test]
        public void Index_ReturnsViewWithProducts()
        {
            // Arrange
            var products = new List<Products>
            {
                new Products { ProductID = 1, Name = "Product 1" },
                new Products { ProductID = 2, Name = "Product 2" }
            };
            _productsManagerMock.Setup(m => m.GetAllProducts()).Returns(products);
            _mapperMock.Setup(m => m.Map<List<Products>>(products)).Returns(products);

            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(products, viewResult.Model);
        }

        [Test]
        public void Create_Get_ReturnsViewForAuthenticatedManager()
        {
            // Arrange
            var controller = GetAuthenticatedController(role: "Manager");

            // Act
            var result = controller.Create();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Create_Get_RedirectsToAccessDeniedForGuest()
        {
            // Arrange
            var controller = GetAuthenticatedController(role: "Guest");

            // Act
            var result = controller.Create();

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("AccessDenied", redirectResult.ActionName);
        }

        [Test]
        public void Create_Post_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var productDto = new Products { ProductID = 1, Name = "Product 1" };
            var product = new Products { ProductID = 1, Name = "Product 1" };

            _mapperMock.Setup(m => m.Map<Products>(productDto)).Returns(product);

            // Act
            var result = _controller.Create(productDto);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectResult.ActionName);
            _productsManagerMock.Verify(m => m.AddProduct(product), Times.Once);
        }

        [Test]
        public void Create_Post_InvalidModel_ReturnsViewWithModel()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");
            var productDto = new Products();

            // Act
            var result = _controller.Create(productDto);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(productDto, viewResult.Model);
        }

        [Test]
        public void Delete_Get_ReturnsViewWithProduct()
        {
            // Arrange
            var product = new Products { ProductID = 1, Name = "Product 1" };
            _productsManagerMock.Setup(m => m.GetAllProducts()).Returns(new List<Products> { product });
            _mapperMock.Setup(m => m.Map<Products>(product)).Returns(product);

            // Act
            var result = _controller.Delete(1);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(product, viewResult.Model);
        }

        [Test]
        public void DeleteConfirmed_ValidId_RedirectsToIndex()
        {
            // Act
            var result = _controller.DeleteConfirmed(1);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectResult.ActionName);
            _productsManagerMock.Verify(m => m.DeleteProduct(1), Times.Once);
        }

        private ProductController GetAuthenticatedController(string role = "Manager")
        {
            var controller = new ProductController(_productsManagerMock.Object, _mapperMock.Object);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.NameIdentifier, "1")
            }, "TestAuthType"));
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
            return controller;
        }
    }
}
}
