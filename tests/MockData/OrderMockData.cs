using OrderMicroservice.Dto;
using OrderMicroservice.Models;

namespace tests.MockData
{
    internal class OrderMockData
    {

        public static List<Order> GetSampleOrderItems()
        {
            return new List<Order>
            {
                new Order
                {
                OrderId = new Guid("10188938-5308-4B19-8E97-57E7F36A6184"),
                CId = new Guid("20188938-5308-4B19-8E97-57E7F36A6184"),
                PId = new Guid("30188938-5308-4B19-8E97-57E7F36A6184"),
                ProductName = "sample product 1",
                quantity = 1,
                OrderAmount = 100,
                OrderDate = DateTime.Now,
                Address = "sample address 1"
                },

               new Order
                {
                OrderId = new Guid("10288938-5308-4B19-8E97-57E7F36A6184"),
                CId = new Guid("20288938-5308-4B19-8E97-57E7F36A6184"),
                PId = new Guid("30288938-5308-4B19-8E97-57E7F36A6184"),
                ProductName = "sample product 2",
                quantity = 2,
                OrderAmount = 200,
                OrderDate = DateTime.Now,
                Address = "sample address 2"
                },

               new Order
                {
                OrderId = new Guid("10388938-5308-4B19-8E97-57E7F36A6184"),
                CId = new Guid("20388938-5308-4B19-8E97-57E7F36A6184"),
                PId = new Guid("30388938-5308-4B19-8E97-57E7F36A6184"),
                ProductName = "sample product 3",
                quantity = 3,
                OrderAmount = 300,
                OrderDate = DateTime.Now,
                Address = "sample address 3"
                }

            };

        }
        public static OrderDto GetSampleInfoToOrder()
        {
           

            List<ProductsDto> productItemList = new List<ProductsDto>()
            {
              new ProductsDto()
                {
                pId = new Guid("9bbc907c-ca0f-5d26-a0ec-303e80158f88"),
                quantity = 1,
                price = 100,
                name = "random name"

                },
            };

            PaymentDto paymentinfo = new PaymentDto()
            {
                creditCardNumber = "123456789123",
                cvv = "123",
                expiryYear = "05/26"

            };

            var data = new OrderDto()
            {
                Address = "random address",
                Orders = productItemList,
                Payment = paymentinfo


            };

            return data;

        }

    }
}
