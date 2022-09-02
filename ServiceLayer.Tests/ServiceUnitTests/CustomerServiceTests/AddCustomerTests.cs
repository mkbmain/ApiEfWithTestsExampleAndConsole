using System;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using Microsoft.Extensions.Logging;
using Moq;
using ServiceLayer.Models;
using Shouldly;
using Xunit;

namespace ServiceLayer.Tests.ServiceUnitTests.CustomerServiceTests;

public class AddCustomerTests
{
    [Fact]
    public async Task Ensure_we_can_addCustomer()
    {
        var request = new CreateCustomerRequest
        {
            City = "cityTe",
            PostCode = "15236526",
            Email = "grwh@hrwh.com",
            BuildingName = "name",
            BuildingNumber = "4yt34",
            Dob = DateTime.Now.AddDays(-555).Date,
            Name = "mikeb",
            Street = "streetName"
        };
        const int expected = 225;
        var (sut, mocklogger, mockrepo) = CustomerServiceFactory.Generate();
        mockrepo.Setup(e => e.Add(It.Is<Customer>(t => t.Email == request.Email &&
                                                       t.Name == request.Name &&
                                                       t.CustomerAddresses.Count == 1 &&
                                                       t.CustomerAddresses.First().City == request.City &&
                                                       t.CustomerAddresses.First().Street == request.Street &&
                                                       t.CustomerAddresses.First().BuildingName ==
                                                       request.BuildingName &&
                                                       t.CustomerAddresses.First().BuildingNumber ==
                                                       request.BuildingNumber &&
                                                       t.CustomerAddresses.First().PostCode == request.PostCode &&
                                                       t.Email == request.Email &&
                                                       t.DateOfBirth == request.Dob)))
            .Callback<Customer>(e => { e.Id = expected; });

        var result = await sut.AddCustomer(request);
        result.Status.ShouldBe(ServiceStatus.Success);
        result.Data.ShouldBe(expected);
        mockrepo.Verify(e => e.Add(It.Is<Customer>(t => t.Email == request.Email &&
                                                        t.Name == request.Name &&
                                                        t.CustomerAddresses.Count == 1 &&
                                                        t.CustomerAddresses.First().City == request.City &&
                                                        t.CustomerAddresses.First().Street == request.Street &&
                                                        t.CustomerAddresses.First().BuildingName ==
                                                        request.BuildingName &&
                                                        t.CustomerAddresses.First().BuildingNumber ==
                                                        request.BuildingNumber &&
                                                        t.CustomerAddresses.First().PostCode == request.PostCode &&
                                                        t.Email == request.Email &&
                                                        t.DateOfBirth == request.Dob)), Times.Once());
    }

    [Fact]
    public async Task Ensure_on_error_we_report()
    {
        var request = new CreateCustomerRequest
        {
            City = "cityTe",
            PostCode = "15236526",
            Email = "grwh@hrwh.com",
            BuildingName = "name",
            BuildingNumber = "4yt34",
            Dob = DateTime.Now.AddDays(-555).Date,
            Name = "mikeb",
            Street = "streetName"
        };

        var (sut, mocklogger, mockrepo) = CustomerServiceFactory.Generate();
        var exception = new Exception(Guid.NewGuid().ToString());
        mockrepo.Setup(e => e.Add(It.IsAny<Customer>()))
            .ThrowsAsync(exception);


        var result = await sut.AddCustomer(request);
        result.Status.ShouldBe(ServiceStatus.Error);
        result.Message.ShouldBe(exception.Message);

        mocklogger.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), 
                It.IsAny<It.IsAnyType>(), exception, 
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), 
            Times.Once);
    }
}