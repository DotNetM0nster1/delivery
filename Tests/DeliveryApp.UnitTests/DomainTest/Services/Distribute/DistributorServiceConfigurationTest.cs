using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using DeliveryApp.Core.Domain.Services.Distribute;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryApp.UnitTests.DomainTest.Services.Distribute
{
    public class DistributorServiceConfigurationTest
    {
        [Fact]
        public void CourierDistributorServiceShouldBePublic()
        {
            Assert.True(typeof(CourierDistributorService).IsPublic);
        }

        [Fact]
        public void CourierDistributorServiceShouldBeSealed()
        {
            Assert.True(typeof(CourierDistributorService).IsSealed);
        }

        [Fact]
        public void CourierDistributorServiceBeClass()
        {
            Assert.True(typeof(CourierDistributorService).IsClass);
        }

        [Fact]
        public void CourierDistributorServiceShouldBeImplementICourierDistributorService()
        {
            Assert.Contains(typeof(ICourierDistributorService), typeof(CourierDistributorService).GetInterfaces());
        }
    }
}