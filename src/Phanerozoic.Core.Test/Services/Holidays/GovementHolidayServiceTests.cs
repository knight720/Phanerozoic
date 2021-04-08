using System;
using System.Net.Http;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Phanerozoic.Core.Services.Holidays.Tests
{
    public class GovementHolidayServiceTests
    {
        [Fact()]
        public void 二零二一年一月一日為假日()
        {
            //// Arrange
            var serviceCollection = new ServiceCollection().AddHttpClient();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var httpClientFatory = serviceProvider.GetRequiredService<IHttpClientFactory>();

            var date = new DateTime(2021, 1, 1);
            var target = new GovementHolidayService(httpClientFatory);

            //// Act
            var actual = target.IsHoliday(date);

            //// Assert
            actual.Should().BeTrue();
        }

        [Fact()]
        public void 二零二一年一月四日為非假日()
        {
            //// Arrange
            var serviceCollection = new ServiceCollection().AddHttpClient();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var httpClientFatory = serviceProvider.GetRequiredService<IHttpClientFactory>();

            var date = new DateTime(2021, 1, 4);
            var target = new GovementHolidayService(httpClientFatory);

            //// Act
            var actual = target.IsHoliday(date);

            //// Assert
            actual.Should().BeFalse();
        }
    }
}