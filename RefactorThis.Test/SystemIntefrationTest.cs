using DB;
using DB.Repository;
using DB.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RefactorThis.DB.Repository;
using RefactorThis.Mapper;
using RefactorThis.Mapper.Interface;
using Xunit;
using RefactorThis.Controllers;
using System;
using Microsoft.AspNetCore.Mvc;
using RefactorThis.Models;
using RefactorThis.DB.Entity;
using System.Collections.Generic;
using System.Linq;

namespace RefactorThis.Test
{
    public class SystemIntefrationTest: IDisposable
    {
        // *************I havn't muck the dependencies as I want to do an inteferation test. 
        // *************There are some unit test as well
        // *************I should have written unit tests by mucking dependencies for each method separately, which takes time

        private readonly IServiceProvider _serviceProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductMapper _productMapper;
        private readonly IProductOptionMapper _productOptionMapper;
        private readonly IProductRepository _productRepository;
        private readonly IProductOptionRepository _productOptionRepository;
        private readonly ILogger _logger;

        private List<Guid> _productsListIds = new List<Guid>();
        private List<ProductOptionDto> _productOptionsList;
        
        public SystemIntefrationTest()
        {
            var loggerMock = new Mock<Microsoft.Extensions.Logging.ILogger>();
            ServiceCollection services = new ServiceCollection();
            services.AddTransient<IProductRepository, ProductRepository>()

                // I should have get the 
                .AddDbContext<ProductsContext>(opt => opt.UseInMemoryDatabase(databaseName: "Data Source=products.db").UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking),
               ServiceLifetime.Scoped,
               ServiceLifetime.Scoped)

            .AddTransient<IProductOptionRepository, ProductOptionRepository>()
            .AddTransient<IUnitOfWork, UnitOfWork>()
            .AddTransient<IProductMapper, ProductMapper>()
            .AddTransient<IProductOptionMapper, ProductOptionMapper>();

            _serviceProvider = services.BuildServiceProvider();

            _unitOfWork = _serviceProvider.GetService(typeof(IUnitOfWork)) as UnitOfWork;
            _productMapper = _serviceProvider.GetService(typeof(IProductMapper)) as ProductMapper;
            _productOptionMapper = _serviceProvider.GetService(typeof(IProductOptionMapper)) as ProductOptionMapper;
            _productRepository = _serviceProvider.GetService(typeof(IProductRepository)) as ProductRepository;
            _productOptionRepository = _serviceProvider.GetService(typeof(IProductOptionRepository)) as ProductOptionRepository;
            _logger = loggerMock.Object;

        }

        public void Dispose()
        {
            var context = _serviceProvider.GetService(typeof(ProductsContext)) as DbContext;
            context.Database.EnsureDeleted();
        }


        [Fact]
        public void Get_ShouldReturnsAllTheRecords()
        {

            // Assigne
            var productController = new ProductsController(_unitOfWork, _productMapper, _productOptionMapper, _productOptionRepository, _logger);

            SeedDb(productController);


            // Action
            var response = productController.Get(null);
            var result = response.Result as OkObjectResult;


            // Assert
            Assert.IsType<ProductListDto>(result.Value);
            var products = result.Value as ProductListDto;
            Assert.Equal(3, products.Items.Count);
        }

        [Fact]
        public void GetWithName_ShouldReturnsAllTheRecords_MatchName()
        {
            var productController = new ProductsController(_unitOfWork, _productMapper, _productOptionMapper, _productOptionRepository, _logger);

            SeedDb(productController);

            var response = productController.Get("Product 2");
            var result = response.Result as OkObjectResult;

            Assert.IsType<ProductListDto>(result.Value);

            var products = result.Value as ProductListDto;

            Assert.Single(products.Items);
            Assert.True(products.Items.First().Name == "Product 2");
        }

        [Fact]
        public void GetOption_ShouldReturnsAllTheRecords_MatchProductId()
        {
            // I havn't muck the dependencies as I want to do an inteferation test. 
            // There are some unit test as well
            var productController = new ProductsController(_unitOfWork, _productMapper, _productOptionMapper, _productOptionRepository, _logger);

            SeedDb(productController);

            var response = productController.GetOptions(_productsListIds[0]);
            var result = response.Result as OkObjectResult;

            Assert.IsType<ProductOptionListDto>(result.Value);

            var productsOptions = result.Value as ProductOptionListDto;

            Assert.Equal(2, productsOptions.Items.Count);
        }

        [Fact]
        public void GetOption_ShouldReturnsOneRecord_MatchProductId_And_MatchOptionId()
        {
            // I havn't muck the dependencies as I want to do an integeration test. 
            // There are some unit test as well
            var productController = new ProductsController(_unitOfWork, _productMapper, _productOptionMapper, _productOptionRepository, _logger);

            SeedDb(productController);

            var productOptions = GetProductOptionById(productController, _productsListIds[0]);

            var responseById = productController.GetOptions(_productsListIds[0], productOptions[0].Id);
            var resultById = responseById.Result as OkObjectResult;
            Assert.IsType<ProductOptionDto>(resultById.Value);

            var productOption = resultById.Value as ProductOptionDto;

            Assert.True(productOption.Id == productOptions[0].Id);
        }

        private void SeedDb(ProductsController productController)
        {
            // This inserts 3 products and then call seedProductOptionDb to add some in to productOtion table

            var p1 = new Models.ProductDto
            {
                Id = Guid.Empty,
                DeliveryPrice = 20.2M,
                Description = "Description for product 1",
                Name = "Product 1",
                Price = 22.2M

            };

            var productIdResponse1 = productController.Post(p1);
            _productsListIds.Add(GetIdFromPostActionResult(productIdResponse1));

            var p2 = new Models.ProductDto
            {
                Id = Guid.Empty,
                DeliveryPrice = 20.2M,
                Description = "Description for product 2",
                Name = "Product 2",
                Price = 22.2M

            };

            var productIdResponse2 = productController.Post(p2);
            _productsListIds.Add(GetIdFromPostActionResult(productIdResponse2));

            var p3 = new Models.ProductDto
            {
                Id = Guid.Empty,
                DeliveryPrice = 20.2M,
                Description = "Description for product 3",
                Name = "Product 3",
                Price = 22.2M

            };

            var productIdResponse3 = productController.Post(p3);
            _productsListIds.Add(GetIdFromPostActionResult(productIdResponse3));

            var response = productController.Get(null);
            var result = response.Result as OkObjectResult;

            SeedProductOptionsTable(productController, _productsListIds);
        }


        private void SeedProductOptionsTable(ProductsController productController, List<Guid> pproductsListIds)
        {
            // This inserts 2 options for the first product and one option for the third product

            var postResult1 = productController.Post(pproductsListIds[0], new ProductOptionDto
            {
                Id = Guid.Empty,
                Description = "Desctiprion for option 1",
            });

            var postResult2 = productController.Post(pproductsListIds[0], new ProductOptionDto
            {
                Id = Guid.Empty,
                Description = "Desctiprion for option 1",
            });

            var postResult3 = productController.Post(pproductsListIds[2], new ProductOptionDto
            {
                Id = Guid.Empty,
                Description = "Desctiprion for option 1",
            });
        }

        private List<ProductOptionDto> GetProductOptionById(ProductsController productController, Guid productId)
        {
            var response = productController.GetOptions(productId);
            var result = response.Result as OkObjectResult;

            var productsOptionsList = (result.Value as ProductOptionListDto).Items;

            return productsOptionsList;
        }

        private Guid GetIdFromPostActionResult(ActionResult<Guid> actionResult)
        {
            var resultById = actionResult.Result as OkObjectResult;
            Assert.IsType<Guid>(resultById.Value);

            var result = resultById.Value as Guid?;

            return result == null ? Guid.Empty : (Guid)result;                   
        }
    }
}
