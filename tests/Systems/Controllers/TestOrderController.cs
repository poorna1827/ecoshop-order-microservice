using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using OrderMicroservice.Controllers;
using OrderMicroservice.DbContexts;
using OrderMicroservice.Dto;
using OrderMicroservice.Services;
using System.Net;
using tests.MockData;
using Xunit;

namespace tests.Systems.Controllers
{
    public class TestOrderController : IDisposable
    {

        private readonly OrderDbContext _context;
        public TestOrderController()
        {
            var options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            _context = new OrderDbContext(options);

            _context.Database.EnsureCreated();

        }
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();

        }


        [Fact]
        public async Task GetOrdersAsync_ShouldReturn200Status()
        {
            //Arrange


            //Identity microservice call mock 
            var ApiServiceMock = new Mock<IApiService>();
            ApiServiceMock.Setup(x => x.isAuthorized(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));
            ApiServiceMock.Setup(x => x.getUserId(It.IsAny<string>())).Returns(new Guid("20188938-5308-4B19-8E97-57E7F36A6184"));




            //database mock
            _context.Orders.AddRange(OrderMockData.GetSampleOrderItems());
            _context.SaveChanges();


            //request header token mock
            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(x => x.Headers["Authorization"]).Returns("Bearer test_token");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.Request).Returns(mockRequest.Object);


            var sut = new OrderController(_context, ApiServiceMock.Object);

            sut.ControllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object
            };


            //Act
            var result = await sut.GetOrders();



            //Assert
            result.GetType().Should().Be(typeof(OkObjectResult));
        }

        [Fact]
        public async Task GetOrdersAsync_ShouldReturn401Status()
        {
            //Arrange


            //Identity microservice call mock 
            var ApiServiceMock = new Mock<IApiService>();
            ApiServiceMock.Setup(x => x.isAuthorized(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Unauthorized));
            ApiServiceMock.Setup(x => x.getUserId(It.IsAny<string>())).Returns(new Guid("20188938-5308-4B19-8E97-57E7F36A6184"));




            //database mock
            _context.Orders.AddRange(OrderMockData.GetSampleOrderItems());
            _context.SaveChanges();


            //request header token mock
            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(x => x.Headers["Authorization"]).Returns("Bearer test_token");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.Request).Returns(mockRequest.Object);


            var sut = new OrderController(_context, ApiServiceMock.Object);

            sut.ControllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object
            };


            //Act
            var result = await sut.GetOrders();



            //Assert
            result.GetType().Should().Be(typeof(UnauthorizedResult));
        }

        [Fact]
        public async Task PlaceOrderAsync_ShouldReturn200Status()
        {
            //Arrange


            //Identity microservice call mock 
            var ApiServiceMock = new Mock<IApiService>();
            ApiServiceMock.Setup(x => x.isAuthorized(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));
            ApiServiceMock.Setup(x => x.getUserId(It.IsAny<string>())).Returns(new Guid("20188938-5308-4B19-8E97-57E7F36A6184"));


            //Product microservice mock
            ApiServiceMock.Setup(x => x.areValidProducts(It.IsAny<List<Guid>>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));


            //Payment microservice mock 
            ApiServiceMock.Setup(x => x.processPayment(It.IsAny<PaymentDto>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));



            //database mock
            _context.Orders.AddRange(OrderMockData.GetSampleOrderItems());
            _context.SaveChanges();


            //request header token mock
            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(x => x.Headers["Authorization"]).Returns("Bearer test_token");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.Request).Returns(mockRequest.Object);


            var sut = new OrderController(_context, ApiServiceMock.Object);

            sut.ControllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object
            };


            OrderDto data = OrderMockData.GetSampleInfoToOrder();
            //Act
            var result = await sut.PlaceOrder(data);



            //Assert
            result.GetType().Should().Be(typeof(OkResult));
        }

        [Fact]
        public async Task PlaceOrderAsync_Invalid_Payment()
        {
            //Arrange


            //Identity microservice call mock 
            var ApiServiceMock = new Mock<IApiService>();
            ApiServiceMock.Setup(x => x.isAuthorized(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));
            ApiServiceMock.Setup(x => x.getUserId(It.IsAny<string>())).Returns(new Guid("20188938-5308-4B19-8E97-57E7F36A6184"));


            //Product microservice mock
            ApiServiceMock.Setup(x => x.areValidProducts(It.IsAny<List<Guid>>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));


            //Payment microservice mock 
            ApiServiceMock.Setup(x => x.processPayment(It.IsAny<PaymentDto>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest));



            //database mock
            _context.Orders.AddRange(OrderMockData.GetSampleOrderItems());
            _context.SaveChanges();


            //request header token mock
            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(x => x.Headers["Authorization"]).Returns("Bearer test_token");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.Request).Returns(mockRequest.Object);


            var sut = new OrderController(_context, ApiServiceMock.Object);

            sut.ControllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object
            };


            OrderDto data = OrderMockData.GetSampleInfoToOrder();
            //Act
            var result = await sut.PlaceOrder(data);



            //Assert
            result.GetType().Should().Be(typeof(BadRequestObjectResult));
        }

        [Fact]
        public async Task PlaceOrderAsync_Invalid_Products()
        {
            //Arrange


            //Identity microservice call mock 
            var ApiServiceMock = new Mock<IApiService>();
            ApiServiceMock.Setup(x => x.isAuthorized(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));
            ApiServiceMock.Setup(x => x.getUserId(It.IsAny<string>())).Returns(new Guid("20188938-5308-4B19-8E97-57E7F36A6184"));


            //Product microservice mock
            ApiServiceMock.Setup(x => x.areValidProducts(It.IsAny<List<Guid>>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NotFound));


            //Payment microservice mock 
            ApiServiceMock.Setup(x => x.processPayment(It.IsAny<PaymentDto>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));



            //database mock
            _context.Orders.AddRange(OrderMockData.GetSampleOrderItems());
            _context.SaveChanges();


            //request header token mock
            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(x => x.Headers["Authorization"]).Returns("Bearer test_token");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.Request).Returns(mockRequest.Object);


            var sut = new OrderController(_context, ApiServiceMock.Object);

            sut.ControllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object
            };


            OrderDto data = OrderMockData.GetSampleInfoToOrder();
            //Act
            var result = await sut.PlaceOrder(data);



            //Assert
            result.GetType().Should().Be(typeof(BadRequestObjectResult));
        }

        [Fact]
        public async Task PlaceOrderAsync__ShouldReturn401Status()
        {
            //Arrange


            //Identity microservice call mock 
            var ApiServiceMock = new Mock<IApiService>();
            ApiServiceMock.Setup(x => x.isAuthorized(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Unauthorized));
            ApiServiceMock.Setup(x => x.getUserId(It.IsAny<string>())).Returns(new Guid("20188938-5308-4B19-8E97-57E7F36A6184"));


            //Product microservice mock
            ApiServiceMock.Setup(x => x.areValidProducts(It.IsAny<List<Guid>>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));


            //Payment microservice mock 
            ApiServiceMock.Setup(x => x.processPayment(It.IsAny<PaymentDto>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));



            //database mock
            _context.Orders.AddRange(OrderMockData.GetSampleOrderItems());
            _context.SaveChanges();


            //request header token mock
            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(x => x.Headers["Authorization"]).Returns("Bearer test_token");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.Request).Returns(mockRequest.Object);


            var sut = new OrderController(_context, ApiServiceMock.Object);

            sut.ControllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object
            };


            OrderDto data = OrderMockData.GetSampleInfoToOrder();
            //Act
            var result = await sut.PlaceOrder(data);



            //Assert
            result.GetType().Should().Be(typeof(UnauthorizedResult));
        }
    }
}
