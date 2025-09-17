using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using DeliveryApp.Core.Domain.Services.Distribute;
using Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryApp.UnitTests.DomainTest.Services.Distribute
{
    public class CourierDistributorServiceTest
    {
        [Fact]
        public void WhenGetingMostSituableCourierForOrder_AndOrderIsNull_ThenMethodShouldBeReturnError()
        {
            //Arrange
            var xFirstCourier = 2;
            var yFirstCourier = 7;

            var xSecondCourier = 6;
            var ySecondCourier = 5;

            var xThirdCourier = 4;
            var yThirdCourier = 9;

            var firstCourierSpeed = 2;
            var secondCourierSpeed = 3;
            var thirdCourierSpeed = 7;

            var firstCourierName = "Maxim Netgov";
            var secondCourierName = "Denis Codov";
            var thirdCourierName = "Kirill Zubov";

            CourierDistributorService courierDistributor = new();

            List<Courier> couriers =
            [
                Courier.Create(firstCourierSpeed, firstCourierName, Location.Create(xFirstCourier, yFirstCourier).Value).Value,
                Courier.Create(secondCourierSpeed, secondCourierName, Location.Create(xSecondCourier, ySecondCourier).Value).Value,
                Courier.Create(thirdCourierSpeed, thirdCourierName, Location.Create(xThirdCourier, yThirdCourier).Value).Value,
            ];

            //Act
            var courierDistributionResult = courierDistributor.DistributeOrderOnCouriers(null, couriers);

            //Assert
            Assert.True(courierDistributionResult.IsFailure);
            Assert.False(courierDistributionResult.IsSuccess);
            Assert.True(courierDistributionResult.Error.Code == "value.is.required");
            Assert.Throws<CSharpFunctionalExtensions.ResultFailureException<Error>>(() => courierDistributionResult.Value);
        }

        [Fact]
        public void WhenGetingMostSituableCourierForOrder_AndListOfCouriersNull_ThenMethodShouldBeReturnError()
        {
            //Arrange
            CourierDistributorService courierDistributor = new();

            var xOrder = 4;
            var yOrder = 2;
            var orderVolume = 5;
            var basketId = Guid.NewGuid();
            var orderLocation = Location.Create(xOrder, yOrder).Value;
            var order = Order.Create(basketId, orderLocation, orderVolume).Value;

            //Act
            var courierDistributionResult = courierDistributor.DistributeOrderOnCouriers(order, null);

            //Assert
            Assert.True(courierDistributionResult.IsFailure);
            Assert.False(courierDistributionResult.IsSuccess);
            Assert.True(courierDistributionResult.Error.Code == "value.is.required");
            Assert.Throws<CSharpFunctionalExtensions.ResultFailureException<Error>>(() => courierDistributionResult.Value);
        }

        [Fact]
        public void WhenGetingMostSituableCourierForOrder_AndListOfCouriersIsEmpty_ThenMethodShouldBeReturnError()
        {
            //Arrange
            CourierDistributorService courierDistributor = new();

            var xOrder = 2;
            var yOrder = 8;
            var orderVolume = 6;
            var basketId = Guid.NewGuid();
            var orderLocation = Location.Create(xOrder, yOrder).Value;
            var order = Order.Create(basketId, orderLocation, orderVolume).Value;

            //Act
            var courierDistributionResult = courierDistributor.DistributeOrderOnCouriers(order, []);

            //Assert
            Assert.True(courierDistributionResult.IsFailure);
            Assert.False(courierDistributionResult.IsSuccess);
            Assert.True(courierDistributionResult.Error.Code == "value.is.required");
            Assert.Throws<CSharpFunctionalExtensions.ResultFailureException<Error>>(() => courierDistributionResult.Value);
        }

        [Fact]
        public void WhenGetingMostSituableCourierForOrder_AndNotFoundTheMostMatchingCourierForOrder_ThenMethodShouldBeReturnError()
        {
            //Arrange
            var xFirstCourier = 2;
            var yFirstCourier = 7;

            var xSecondCourier = 6;
            var ySecondCourier = 5;

            var xThirdCourier = 4;
            var yThirdCourier = 9;

            var firstCourierSpeed = 2;
            var secondCourierSpeed = 3;
            var thirdCourierSpeed = 7;

            var firstCourierName = "Maxim Netgov";
            var secondCourierName = "Denis Codov";
            var thirdCourierName = "Kirill Zubov";

            CourierDistributorService courierDistributor = new();

            List<Courier> couriers =
            [
                Courier.Create(firstCourierSpeed, firstCourierName, Location.Create(xFirstCourier, yFirstCourier).Value).Value,
                Courier.Create(secondCourierSpeed, secondCourierName, Location.Create(xSecondCourier, ySecondCourier).Value).Value,
                Courier.Create(thirdCourierSpeed, thirdCourierName, Location.Create(xThirdCourier, yThirdCourier).Value).Value,
            ];

            var xOrder = 2;
            var yOrder = 8;
            var orderVolume = 20;
            var basketId = Guid.NewGuid();
            var orderLocation = Location.Create(xOrder, yOrder).Value;
            var order = Order.Create(basketId, orderLocation, orderVolume).Value;

            //Act
            var courierDistributionResult = courierDistributor.DistributeOrderOnCouriers(order, couriers);

            //Assert
            Assert.True(courierDistributionResult.IsFailure);
            Assert.False(courierDistributionResult.IsSuccess);
            Assert.True(courierDistributionResult.Error.Code == "order.cannot.be.distributed");
            Assert.True(courierDistributionResult.Error.InnerError.Code == "not.found.matching.courier.for.order");
            Assert.Throws<CSharpFunctionalExtensions.ResultFailureException<Error>>(() => courierDistributionResult.Value);
        }

        [Theory]
        [InlineData(
         "Alexey Gudinov", 2, 1, 1,
         "Mihail Zakitskiy", 2, 2, 1,
         "Valeriy Samsonov", 2, 3, 1, 
         5, 2, 5,
         "Mihail Zakitskiy", 2, 2, 1)]
        [InlineData(
         "Ivan Petrov", 1, 1, 1,
         "Nikolay Smornov", 2, 3, 1,
         "Sergey Ivanov", 2, 4, 1,
         4, 2, 10,
         "Nikolay Smornov", 2, 3, 1)]
        [InlineData(
         "Dmitry Smirnov", 9, 10, 1,
         "Alexey Volkov", 6, 8, 1,
         "Nikolay Sokolov", 8, 1, 1,
         8, 5, 6,
         "Nikolay Sokolov", 8, 1, 1)]
        [InlineData(
         "Pavel Lebedev", 7, 1, 1,
         "Andrey Kozlov", 1, 10, 1,
         "Vladimir Morozov", 3, 5, 1,
         10, 10, 1,
         "Andrey Kozlov", 1, 10, 1)]
        [InlineData(
         "Oleg Novikov", 3, 3, 1,
         "Mikhail Egorov", 3, 7, 1,
         "Anton Popov", 9, 10, 1,
         2, 9, 5,
         "Mikhail Egorov", 3, 7, 1)]
        [InlineData(
         "Roman Orlov", 2, 4, 1,
         "Igor Kuznetsov", 6, 6, 1,
         "Yuri Stepanov", 3, 8, 1,
         4, 2, 9,
         "Roman Orlov", 2, 4, 1)]
        [InlineData(
         "Maxim Nikolaev", 6, 7, 1,
         "Kirill Pavlov", 7, 6, 1,
         "Denis Fedorov", 8, 5, 1,
         4, 3, 7,
         "Denis Fedorov", 8, 5, 1)]
        [InlineData(
         "Vyacheslav Mikhailov", 1, 10, 1,
         "Artem Alexandrov", 10, 1, 1,
         "Boris Andreev", 1, 1, 1,
         10, 10, 5,
         "Artem Alexandrov", 10, 1, 1)]
        [InlineData(
         "Konstantin Vasiliev", 3, 3, 1,
         "Stanislav Sergeev", 5, 5, 1,
         "Leonid Romanov", 3, 8, 1,
         2, 6, 3,
         "Leonid Romanov", 3, 8, 1)]
        [InlineData(
         "Victor Bogdanov", 3, 4, 1,
         "Evgeny Belov", 8, 6, 1,
         "Anatoly Vinogradov", 6, 9, 1,
         6, 6, 6,
         "Evgeny Belov", 8, 6, 1)]
        [InlineData(
         "Stepan Tarasov", 4, 5, 2,
         "Gennady Filippov", 5, 8, 2,
         "Valery Markov", 8, 10, 2,
         7, 10, 2,
         "Valery Markov", 8, 10, 2)]
        [InlineData(
         "Stepan Tarasov", 4, 4, 2,
         "Gennady Filippov", 2, 10, 2,
         "Valery Markov", 8, 5, 2,
         2, 1, 4,
         "Stepan Tarasov", 4, 4, 2)]
        [InlineData(
         "Eduard Borisov", 6, 6, 2,
         "Timofey Zaitsev", 5, 8, 2,
         "Matvey Denisov", 3, 8, 2,
         8, 10, 7,
         "Timofey Zaitsev", 5, 8, 2)]
        [InlineData(
         "Egor Mironov", 6, 3, 2,
         "Fedor Antonov", 7, 3, 2,
         "German Gavrilov", 8, 3, 2,
         5, 8, 8,
         "Egor Mironov", 6, 3, 2)]
        [InlineData(
         "Orest Dmitriev", 5, 2, 2,
         "Platon Ignatov", 3, 4, 2,
         "Semyon Kirillov", 9, 3, 2,
         9, 9, 1,
         "Semyon Kirillov", 9, 3, 2)]
        [InlineData(
         "Arkady Afanasyev", 5, 2, 2,
         "Valentin Leonov", 3, 4, 2,
         "Vsevolod Gusev", 9, 3, 2,
         9, 9, 7,
         "Vsevolod Gusev", 9, 3, 2)]
        [InlineData(
         "Marat Nikitin", 1, 4, 2,
         "Rashid Karpov", 2, 4, 2,
         "Ruslan Savin", 8, 2, 2,
         3, 6, 2,
         "Rashid Karpov", 2, 4, 2)]
        [InlineData(
         "Vadim Loginov", 3, 10, 2,
         "Albert Solovyov", 4, 9, 2,
         "Rinat Krylov", 5, 10, 2,
         4, 7, 3,
         "Albert Solovyov", 4, 9, 2)]
        [InlineData(
         "Ilya Zhuravlev", 6, 5, 2,
         "Anton Bykov", 8, 5, 2,
         "Stepan Gromov", 5, 8, 2,
         2, 7, 1,
         "Stepan Gromov", 5, 8, 2)]
        [InlineData(
         "Kirill Grigoryev", 3, 6, 2,
         "Vladimir Zakharov", 4, 8, 2,
         "Stepan Gromov", 10, 3, 2,
         3, 1, 10,
         "Kirill Grigoryev", 3, 6, 2)]
        [InlineData(
         "Ilya Zhuravlev", 9, 5, 3,
         "Viktor Frolov", 2, 9, 3,
         "Stepan Gromov", 4, 10, 3,
         3, 2, 10,
         "Viktor Frolov", 2, 9, 3)]
        [InlineData(
         "Timur Belyaev", 1, 7, 3,
         "Arseniy Polyakov", 3, 8, 3,
         "Denis Matveev", 9, 1, 3,
         8, 6, 7,
         "Denis Matveev", 9, 1, 3)]
        [InlineData(
         "Viktor Trofimov", 3, 4, 3,
         "Maxim Kiselev", 4, 8, 3,
         "Pavel Alekseev", 6, 8, 3,
         10, 8, 4,
         "Pavel Alekseev", 6, 8, 3)]
        [InlineData(
         "Oleg Ignatov", 3, 7, 3,
         "Ilya Efimov", 3, 2, 3,
         "Egor Melnikov", 5, 1, 3,
         2, 10, 1,
         "Oleg Ignatov", 3, 7, 3)]
        [InlineData(
         "Anatoly Panov", 5, 3, 3,
         "Stanislav Voronov", 2, 5, 3,
         "Leonid Fedoseev", 8, 4, 3,
         5, 7, 3,
         "Anatoly Panov", 5, 3, 3)]
        [InlineData(
         "Boris Rudenko", 2, 5, 3,
         "Artem Spiridonov", 5, 2, 3,
         "Stepan Lapin", 9, 2, 3,
         8, 6, 2,
         "Stepan Lapin", 9, 2, 3)]
        [InlineData(
         "Mark Kudryavtsev", 4, 1, 3,
         "Vladislav Sharapov", 7, 1, 3,
         "Nikita Chistyakov", 8, 10, 3,
         1, 6, 2,
         "Mark Kudryavtsev", 4, 1, 3)]
        [InlineData(
         "Denis Safronov", 4, 3, 3,
         "Roman Pankratov", 10, 3, 3,
         "Sergey Belov", 3, 9, 3,
         7, 6, 9,
         "Roman Pankratov", 10, 3, 3)]
        [InlineData(
         "Dmitry Loginov", 3, 5, 3,
         "Igor Orlov", 10, 5, 3,
         "Alexey Nazarov", 4, 7, 3,
         6, 3, 7,
         "Dmitry Loginov", 3, 5, 3)]
        [InlineData(
         "Nikolay Krylov", 4, 2, 3,
         "Mikhail Danilov", 8, 4, 3,
         "Kirill Chernov", 6, 6, 3,
         3, 8, 2,
         "Kirill Chernov", 6, 6, 3)]
        [InlineData(
         "Evgeny Gavrilov", 2, 3, 4,
         "Oleg Ermakov", 7, 2, 4,
         "Ivan Ignatiev", 2, 9, 4,
         8, 8, 5,
         "Ivan Ignatiev", 2, 9, 4)]
        [InlineData(
         "Vladimir Titov", 10, 4, 4,
         "Andrey Filatov", 1, 8, 4,
         "Pavel Korolev", 6, 10, 4,
         8, 8, 3,
         "Pavel Korolev", 6, 10, 4)]
        [InlineData(
         "Maxim Petukhov", 9, 10, 4,
         "Roman Yashin", 10, 9, 4,
         "Anton Makarov", 10, 10, 4,
         6, 4, 2,
         "Roman Yashin", 10, 9, 4)]
        [InlineData(
         "Sergey Kuzin", 2, 5, 4,
         "Denis Vinogradov", 6, 3, 4,
         "Artem Solovyov", 9, 7, 4,
         4, 6, 1,
         "Sergey Kuzin", 2, 5, 4)]
        [InlineData(
         "Fedor Spiridonov", 3, 4, 4,
         "Yuri Gavrilov", 6, 5, 4,
         "Mikhail Danilov", 10, 2, 4,
         8, 10, 1,
         "Yuri Gavrilov", 6, 5, 4)]
        [InlineData(
         "Stanislav Trofimov", 4, 2, 4,
         "Kirill Egorov", 7, 5, 4,
         "Alexey Popov", 5, 9, 4,
         2, 5, 3,
         "Kirill Egorov", 7, 5, 4)]
        [InlineData(
         "Vladislav Lebedev", 2, 3, 4,
         "Roman Makarov", 9, 1, 4,
         "Ivan Fedorov", 8, 8, 4,
         3, 10, 3,
         "Ivan Fedorov", 8, 8, 4)]
        [InlineData(
         "Andrey Solovyov", 3, 4, 4,
         "Sergey Gusev", 8, 5, 4,
         "Oleg Petrov", 6, 10, 4,
         6, 1, 3,
         "Sergey Gusev", 8, 5, 4)]
        [InlineData(
         "Pavel Belov", 2, 5, 4,
         "Anton Egorov", 9, 4, 4,
         "Dmitry Mironov", 8, 9, 4,
         6, 1, 3,
         "Anton Egorov", 9, 4, 4)]
        [InlineData(
         "Pavel Belov", 4, 3, 4,
         "Anton Egorov", 9, 5, 4,
         "Dmitry Mironov", 3, 8, 4,
          7, 1, 2,
          "Pavel Belov", 4, 3, 4)]
        [InlineData(
         "Maxim Pavlov", 10, 3, 5,
         "Kirill Kuznetsov", 10, 5, 5,
         "Nikolay Antonov", 10, 7, 5,
         3, 3, 2,
         "Maxim Pavlov", 10, 3, 5)]
        [InlineData(
         "Vladimir Morozov", 2, 4, 5,
         "Artem Vinogradov", 9, 4, 5,
         "Stanislav Ivanov", 6, 8, 5,
         5, 5, 5,
         "Stanislav Ivanov", 6, 8, 5)]
        [InlineData(
         "Mikhail Smirnov", 5, 5, 5,
         "Ivan Sokolov", 9, 4, 5,
         "Alexey Volkov", 8, 9, 5,
         1, 1, 4,
         "Mikhail Smirnov", 5, 5, 5)]
        [InlineData(
         "Roman Orlov", 3, 4, 5,
         "Andrey Novikov", 1, 9, 5,
         "Denis Stepanov", 7, 9, 5,
         10, 1, 2,
         "Roman Orlov", 3, 4, 5)]
        [InlineData(
         "Sergey Nikolaev", 10, 5, 5,
         "Pavel Mikhailov", 8, 8, 5,
         "Igor Andreev", 5, 10, 5,
         5, 2, 2,
         "Igor Andreev", 5, 10, 5)]
        [InlineData(
         "Anton Vasiliev", 3, 3, 5,
         "Oleg Romanov", 6, 6, 5,
         "Maxim Bogdanov", 10, 10, 5,
         7, 2, 1,
         "Oleg Romanov", 6, 6, 5)]
        [InlineData(
         "Kirill Belov", 3, 3, 5,
         "Dmitry Tarasov", 6, 6, 5,
         "Ivan Filippov", 10, 10, 5,
         10, 1, 1,
         "Ivan Filippov", 10, 10, 5)]
        [InlineData(
         "Mikhail Markov", 10, 2, 5,
         "Sergey Borisov", 5, 10, 5,
         "Pavel Zaitsev", 10, 8, 5,
         1, 4, 1,
         "Sergey Borisov", 5, 10, 5)]
        [InlineData(
         "Alexey Denisov", 4, 3, 5,
         "Andrey Mironov", 7, 5, 5,
         "Roman Antonov", 3, 9, 5,
         10, 5, 1,
         "Andrey Mironov", 7, 5, 5)]
        [InlineData(
         "Anton Gavrilov", 5, 3, 5,
         "Vladimir Dmitriev", 8, 6, 5,
         "Kirill Ignatov", 9, 2, 5,
         2, 8, 1,
         "Vladimir Dmitriev", 8, 6, 5)]
        [InlineData(
         "Denis Kirillov", 3, 4, 6,
         "Nikolay Afanasyev", 6, 7, 6,
         "Maxim Leonov", 3, 9, 6,
         10, 4, 1,
         "Nikolay Afanasyev", 6, 7, 6)]
        [InlineData(
         "Ivan Gusev", 3, 4, 6,
         "Sergey Nikitin", 9, 4, 6,
         "Pavel Karpov", 6, 9, 6,
         6, 1, 3,
         "Sergey Nikitin", 9, 4, 6)]
        [InlineData(
         "Oleg Savin", 4, 5, 6,
         "Alexey Loginov", 7, 9, 6,
         "Andrey Solovyov", 8, 3, 6,
         2, 1, 2,
         "Oleg Savin", 4, 5, 6)]
        [InlineData(
         "Roman Krylov", 8, 2, 6,
         "Anton Zhuravlev", 8, 6, 6,
         "Mikhail Frolov", 2, 10, 6,
         3, 4, 2,
         "Mikhail Frolov", 2, 10, 6)]
        [InlineData(
         "Kirill Gromov", 2, 5, 6,
         "Dmitry Yashin", 9, 4, 6,
         "Ivan Korolev", 6, 10, 6,
         4, 1, 2,
         "Kirill Gromov", 2, 5, 6)]
        [InlineData(
         "Sergey Yermakov", 5, 2, 6,
         "Pavel Belousov", 8, 6, 6,
         "Oleg Prokhorov", 3, 8, 6,
         1, 5, 3,
         "Oleg Prokhorov", 3, 8, 6)]
        [InlineData(
         "Andrey Rogov", 1, 5, 6,
         "Anton Sorokin", 7, 10, 6,
         "Roman Maltsev", 10, 8, 6,
         8, 1, 1,
         "Roman Maltsev", 10, 8, 6)]
        [InlineData(
         "Alexey Elizarov", 2, 1, 6,
         "Nikolay Dronov", 7, 1, 6,
         "Vladimir Chernov", 10, 7, 6,
         5, 10, 1,
         "Vladimir Chernov", 10, 7, 6)]
        [InlineData(
         "Mikhail Samsonov", 1, 3, 6,
         "Pavel Arkhipov", 10, 1, 6,
         "Igor Abramov", 10, 9, 6,
         4, 6, 7,
         "Mikhail Samsonov", 1, 3, 6)]
        [InlineData(
         "Igor Abramov", 5, 2, 6,
         "Kirill Fedotov", 9, 5, 6,
         "Roman Loginov", 7, 9, 6,
         1, 7, 7,
         "Roman Loginov", 7, 9, 6)]
        [InlineData(
         "Maxim Pavlov", 6, 2, 7,
         "Dmitry Loginov", 9, 9, 7,
         "Konstantin Morozov", 8, 9, 7,
         1, 7, 7,
         "Konstantin Morozov", 8, 9, 7)]
        [InlineData(
         "Ruslan Savin", 5, 2, 7,
         "Evgeny Belov", 9, 7, 7,
         "Sergey Vlasov", 6, 9, 7,
         4, 6, 7,
         "Sergey Vlasov", 6, 9, 7)]
        [InlineData(
         "Kirill Fedotov", 5, 1, 7,
         "Ivan Nikitin", 7, 6, 7,
         "Artem Spiridonov", 2, 10, 7,
         2, 5, 7,
         "Artem Spiridonov", 2, 10, 7)]
        [InlineData(
         "Konstantin Morozov", 1, 3, 7,
         "Denis Matveev", 1, 8, 7,
         "Denis Kirillov", 10, 8, 7,
         8, 3, 7,
         "Denis Kirillov", 10, 8, 7)]
        [InlineData(
         "Artyom Rybakov", 4, 3, 7,
         "Stepan Tarasov", 10, 2, 7,
         "Andrey Kozlov", 6, 9, 7,
         7, 5, 7,
         "Andrey Kozlov", 6, 9, 7)]
        [InlineData(
         "Semyon Kirillov", 3, 4, 7,
         "Stepan Gromov", 8, 6, 7,
         "Arseniy Dronov", 7, 1, 7,
         5, 10, 2,
         "Stepan Gromov", 8, 6, 7)]
        [InlineData(
         "Oleg Pavlov", 6, 3, 7,
         "Evgeny Gavrilov", 2, 5, 7,
         "Maxim Pavlov", 9, 9, 7,
         5, 9, 1,
         "Maxim Pavlov", 9, 9, 7)]
        [InlineData(
         "Vladimir Morozov", 5, 2, 7,
         "Sergey Nikolaev", 9, 5, 7,
         "Ivan Filippov", 3, 8, 7,
         8, 9, 7,
         "Sergey Nikolaev", 9, 5, 7)]
        [InlineData(
         "Kirill Ignatov", 3, 3, 7,
         "Oleg Savin", 9, 5, 7,
         "Sergey Yermakov", 6, 8, 7,
         1, 1, 2,
         "Kirill Ignatov", 3, 3, 7)]
        [InlineData(
         "Oleg Prokhorov", 4, 4, 7,
         "Pavel Arkhipov", 7, 5, 7,
         "Anton Egorov", 5, 6, 7,
         6, 2, 2,
         "Pavel Arkhipov", 7, 5, 7)]
        [InlineData(
         "Stepan Lapin", 7, 4, 8,
         "Dmitry Semyonov", 3, 6, 8,
         "Anton Bykov", 5, 8, 8,
         1, 8, 2,
         "Anton Bykov", 5, 8, 8)]
        [InlineData(
         "Filipp Chernov", 3, 3, 8,
         "Maxim Abramov", 7, 6, 8,
         "Sergey Borisov", 9, 1, 8,
         10, 2, 5,
         "Sergey Borisov", 9, 1, 8)]
        [InlineData(
         "Pavel Alekseev", 5, 2, 8,
         "Artem Spiridonov", 8, 8, 8,
         "Andrey Solovyov", 2, 9, 8,
         4, 5, 2,
         "Pavel Alekseev", 5, 2, 8)]
        [InlineData(
         "Sergey Kuzin", 3, 3, 8,
         "Fedor Spiridonov", 8, 3, 8,
         "Ivan Petrov", 6, 7, 8,
         2, 8, 2,
         "Ivan Petrov", 6, 7, 8)]
        [InlineData(
         "Platon Ignatov", 8, 3, 8,
         "Vsevolod Gusev", 9, 6, 8,
         "Pavel Belov", 3, 5, 8,
         6, 3, 2,
         "Platon Ignatov", 8, 3, 8)]
        [InlineData(
         "Denis Kirillov", 5, 6, 8,
         "Alexey Loginov", 2, 8, 8,
         "Mikhail Lebedev", 5, 9, 8,
         2, 1, 1,
         "Alexey Loginov", 2, 8, 8)]
        [InlineData(
         "Roman Antonov", 3, 5, 8,
         "Ivan Gusev", 8, 9, 8,
         "Mikhail Frolov", 3, 5, 8,
         5, 3, 1,
         "Mikhail Frolov", 3, 5, 8)]
        [InlineData(
         "Maxim Leonov", 1, 1, 8,
         "Anton Zhuravlev", 3, 4, 8,
         "Alexey Nazarov", 5, 7, 8,
         10, 10, 1,
         "Alexey Nazarov", 5, 7, 8)]
        [InlineData(
         "Evgeny Gavrilov", 9, 3, 8,
         "Sergey Ivanov", 7, 6, 8,
         "Evgeny Belov", 1, 10, 8,
         3, 3, 3,
         "Evgeny Gavrilov", 9, 3, 8)]
        [InlineData(
         "Timofey Zaitsev", 9, 3, 8,
         "Vadim Loginov", 3, 7, 8,
         "Filipp Chernov", 7, 9, 8,
         3, 2, 10,
         "Vadim Loginov", 3, 7, 8)]
        [InlineData(
         "Filipp Chernov", 1, 1, 9,
         "Fedor Antonov", 6, 3, 9,
         "Anton Popov", 6, 10, 9,
         10, 8, 10,
         "Anton Popov", 6, 10, 9)]
        [InlineData(
         "Vyacheslav Mikhailov", 4, 1, 9,
         "Stepan Tarasov", 6, 5, 9,
         "Eduard Borisov", 3, 7, 9,
         1, 4, 4,
         "Eduard Borisov", 3, 7, 9)]
        [InlineData(
         "Evgeny Belov", 8, 2, 9,
         "Vladislav Golubev", 4, 5, 9,
         "Ilya Efimov", 8, 10, 9,
         6, 8, 4,
         "Ilya Efimov", 8, 10, 9)]
        [InlineData(
         "Viktor Trofimov", 4, 4, 9,
         "Pavel Korolev", 7, 5, 9,
         "Sergey Kuzin", 3, 8, 9,
         6, 10, 2,
         "Sergey Kuzin", 3, 8, 9)]
        [InlineData(
         "Ivan Ignatiev", 3, 4, 9,
         "Kirill Egorov", 3, 7, 9,
         "Andrey Solovyov", 3, 10, 9,
         8, 8, 3,
         "Kirill Egorov", 3, 7, 9)]
        [InlineData(
         "Alexey Popov", 3, 2, 9,
         "Mikhail Danilov", 7, 5, 9,
         "Stepan Lapin", 8, 10, 9,
         9, 2, 1,
         "Mikhail Danilov", 7, 5, 9)]
        [InlineData(
         "Vladimir Titov", 6, 4, 9,
         "Denis Safronov", 9, 7, 9,
         "Anton Bykov", 3, 9, 9,
         3, 2, 9,
         "Vladimir Titov", 6, 4, 9)]
        [InlineData(
         "Timur Belyaev", 7, 3, 9,
         "Nikita Chistyakov", 3, 7, 9,
         "Oleg Ermakov", 9, 6, 9,
         2, 1, 1,
         "Nikita Chistyakov", 3, 7, 9)]
        [InlineData(
         "Artem Solovyov", 10, 1, 9,
         "Stanislav Trofimov", 8, 4, 9,
         "Roman Orlov", 5, 8, 9,
         4, 2, 1,
         "Stanislav Trofimov", 8, 4, 9)]
        [InlineData(
         "Sergey Nikolaev", 5, 1, 9,
         "Mikhail Frolov", 3, 8, 9,
         "Sergey Golubev", 6, 10, 9,
         10, 5, 1,
         "Sergey Golubev", 6, 10, 9)]
        [InlineData(
         "Pavel Arkhipov", 2, 2, 10,
         "Nikolay Afanasyev", 6, 5, 10,
         "Andrey Solovyov", 8, 9, 10,
         3, 8, 3,
         "Andrey Solovyov", 8, 9, 10)]
        [InlineData(
         "Sergey Nikitin", 1, 1, 10,
         "Ivan Sokolov", 10, 1, 10,
         "Alexey Popov", 10, 10, 10,
         1, 10, 3,
         "Alexey Popov", 10, 10, 10)]
        [InlineData(
         "Denis Vinogradov", 7, 1, 10,
         "Oleg Ermakov", 2, 8, 10,
         "Mark Elizarov", 6, 8, 10,
         1, 3, 3,
         "Oleg Ermakov", 2, 8, 10)]
        [InlineData(
         "Anatoly Panov", 5, 1, 10,
         "Pavel Korolev", 9, 6, 10,
         "Artem Solovyov", 4, 7, 10,
         2, 4, 1,
         "Artem Solovyov", 4, 7, 10)]
        [InlineData(
         "Mikhail Danilov", 6, 1, 10,
         "Dmitry Mironov", 4, 3, 10,
         "Kirill Kuznetsov", 6, 9, 10,
         10, 8, 3,
         "Kirill Kuznetsov", 6, 9, 10)]
        [InlineData(
         "Mikhail Smirnov", 2, 7, 10,
         "Anton Vasiliev", 10, 7, 10,
         "Vladimir Chernov", 6, 10, 10,
         5, 3, 2,
         "Mikhail Smirnov", 2, 7, 10)]
        [InlineData(
         "Alexey Denisov", 1, 5, 10,
         "Stanislav Ivanov", 1, 9, 10,
         "Stanislav Trofimov", 10, 8, 10,
         8, 2, 2,
         "Stanislav Trofimov", 10, 8, 10)]
        [InlineData(
         "Pavel Korolev", 5, 1, 10,
         "Roman Yashin", 5, 10, 10,
         "Yuri Gavrilov", 10, 10, 10,
         5, 1, 2,
         "Pavel Korolev", 5, 1, 10)]
        [InlineData(
         "Sergey Kuzin", 8, 3, 10,
         "Stepan Gromov", 1, 7, 10,
         "Filipp Chernov", 7, 9, 10,
         8, 9, 4,
         "Filipp Chernov", 7, 9, 10)]
        [InlineData(
         "Sergey Kuzin", 5, 8, 10,
         "Stepan Gromov", 6, 8, 10,
         "Filipp Chernov", 5, 9, 10,
         6, 9, 4,
         "Filipp Chernov", 5, 9, 10)]
        [InlineData(
         "Alexey Gudinov", 2, 1, 4,
         "Mihail Zakitskiy", 2, 2, 6,
         "Valeriy Samsonov", 2, 3, 1,
         5, 2, 5,
         "Mihail Zakitskiy", 2, 2, 6)]
        [InlineData(
         "Ivan Petrov", 1, 1, 8,
         "Nikolay Smornov", 2, 3, 3,
         "Sergey Ivanov", 2, 4, 1,
         4, 2, 10,
         "Ivan Petrov", 1, 1, 8)]
        [InlineData(
         "Dmitry Smirnov", 9, 10, 10,
         "Alexey Volkov", 6, 8, 7,
         "Nikolay Sokolov", 8, 1, 2,
         8, 5, 6,
         "Dmitry Smirnov", 9, 10, 10)]
        [InlineData(
         "Pavel Lebedev", 7, 1, 9,
         "Andrey Kozlov", 1, 10, 5,
         "Vladimir Morozov", 3, 5, 2,
         10, 10, 1,
         "Pavel Lebedev", 7, 1, 9)]
        [InlineData(
         "Oleg Novikov", 3, 3, 6,
         "Mikhail Egorov", 3, 7, 8,
         "Anton Popov", 9, 10, 3,
         2, 9, 5,
         "Mikhail Egorov", 3, 7, 8)]
        [InlineData(
         "Roman Orlov", 2, 4, 7,
         "Igor Kuznetsov", 6, 6, 10,
         "Yuri Stepanov", 3, 8, 1,
         4, 2, 9,
         "Roman Orlov", 2, 4, 7)]
        [InlineData(
         "Maxim Nikolaev", 6, 7, 2,
         "Kirill Pavlov", 7, 6, 9,
         "Denis Fedorov", 8, 5, 4,
         4, 3, 7,
         "Kirill Pavlov", 7, 6, 9)]
        [InlineData(
         "Vyacheslav Mikhailov", 1, 10, 3,
         "Artem Alexandrov", 10, 1, 6,
         "Boris Andreev", 1, 1, 5,
         10, 10, 5,
         "Artem Alexandrov", 10, 1, 6)]
        [InlineData(
         "Konstantin Vasiliev", 3, 3, 8,
         "Stanislav Sergeev", 5, 5, 1,
         "Leonid Romanov", 3, 8, 7,
         2, 6, 3,
         "Leonid Romanov", 3, 8, 7)]
        [InlineData(
         "Victor Bogdanov", 3, 4, 10,
         "Evgeny Belov", 8, 6, 4,
         "Anatoly Vinogradov", 6, 9, 5,
         6, 6, 6,
         "Evgeny Belov", 8, 6, 4)]
        [InlineData(
         "Stepan Tarasov", 4, 5, 2,
         "Gennady Filippov", 5, 8, 7,
         "Valery Markov", 8, 10, 6,
         7, 10, 2,
         "Valery Markov", 8, 10, 6)]
        [InlineData(
         "Stepan Tarasov", 4, 4, 9,
         "Gennady Filippov", 2, 10, 3,
         "Valery Markov", 8, 5, 8,
         2, 1, 4,
         "Stepan Tarasov", 4, 4, 9)]
        [InlineData(
         "Eduard Borisov", 6, 6, 5,
         "Timofey Zaitsev", 5, 8, 1,
         "Matvey Denisov", 3, 8, 6,
         8, 10, 7,
         "Matvey Denisov", 3, 8, 6)]
        [InlineData(
         "Egor Mironov", 6, 3, 7,
         "Fedor Antonov", 7, 3, 2,
         "German Gavrilov", 8, 3, 8,
         5, 8, 8,
         "Egor Mironov", 6, 3, 7)]
        [InlineData(
         "Orest Dmitriev", 5, 2, 10,
         "Platon Ignatov", 3, 4, 9,
         "Semyon Kirillov", 9, 3, 3,
         9, 9, 1,
         "Orest Dmitriev", 5, 2, 10)]
        [InlineData(
         "Arkady Afanasyev", 5, 2, 6,
         "Valentin Leonov", 3, 4, 2,
         "Vsevolod Gusev", 9, 3, 8,
         9, 9, 7,
         "Vsevolod Gusev", 9, 3, 8)]
        [InlineData(
         "Marat Nikitin", 1, 4, 1,
         "Rashid Karpov", 2, 4, 4,
         "Ruslan Savin", 8, 2, 7,
         3, 6, 2,
         "Rashid Karpov", 2, 4, 4)]
        [InlineData(
         "Vadim Loginov", 3, 10, 3,
         "Albert Solovyov", 4, 9, 9,
         "Rinat Krylov", 5, 10, 2,
         4, 7, 3,
         "Albert Solovyov", 4, 9, 9)]
        [InlineData(
         "Ilya Zhuravlev", 6, 5, 8,
         "Anton Bykov", 8, 5, 6,
         "Stepan Gromov", 5, 8, 10,
         2, 7, 1,
         "Stepan Gromov", 5, 8, 10)]
        [InlineData(
         "Kirill Grigoryev", 3, 6, 5,
         "Vladimir Zakharov", 4, 8, 7,
         "Stepan Gromov", 10, 3, 9,
         3, 1, 10,
         "Stepan Gromov", 10, 3, 9)]
        [InlineData(
         "Ilya Zhuravlev", 9, 5, 4,
         "Viktor Frolov", 2, 9, 2,
         "Stepan Gromov", 4, 10, 10,
         3, 2, 10,
         "Stepan Gromov", 4, 10, 10)]
        [InlineData(
         "Timur Belyaev", 1, 7, 1,
         "Arseniy Polyakov", 3, 8, 8,
         "Denis Matveev", 9, 1, 9,
         8, 6, 7,
         "Denis Matveev", 9, 1, 9)]
        [InlineData(
         "Viktor Trofimov", 3, 4, 6,
         "Maxim Kiselev", 4, 8, 3,
         "Pavel Alekseev", 6, 8, 10,
         10, 8, 4,
         "Pavel Alekseev", 6, 8, 10)]
        [InlineData(
         "Oleg Ignatov", 3, 7, 7,
         "Ilya Efimov", 3, 2, 4,
         "Egor Melnikov", 5, 1, 9,
         2, 10, 1,
         "Oleg Ignatov", 3, 7, 7)]
        [InlineData(
         "Anatoly Panov", 5, 3, 5,
         "Stanislav Voronov", 2, 5, 8,
         "Leonid Fedoseev", 8, 4, 2,
         5, 7, 3,
         "Stanislav Voronov", 2, 5, 8)]
        [InlineData(
         "Boris Rudenko", 2, 5, 10,
         "Artem Spiridonov", 5, 2, 6,
         "Stepan Lapin", 9, 2, 9,
         8, 6, 2,
         "Stepan Lapin", 9, 2, 9)]
        [InlineData(
         "Mark Kudryavtsev", 4, 1, 3,
         "Vladislav Sharapov", 7, 1, 1,
         "Nikita Chistyakov", 8, 10, 7,
         1, 6, 2,
         "Nikita Chistyakov", 8, 10, 7)]
        [InlineData(
         "Denis Safronov", 4, 3, 2,
         "Roman Pankratov", 10, 3, 5,
         "Sergey Belov", 3, 9, 6,
         7, 6, 9,
         "Sergey Belov", 3, 9, 6)]
        [InlineData(
         "Dmitry Loginov", 3, 5, 8,
         "Igor Orlov", 10, 5, 7,
         "Alexey Nazarov", 4, 7, 10,
         6, 3, 7,
         "Alexey Nazarov", 4, 7, 10)]
        [InlineData(
         "Nikolay Krylov", 4, 2, 4,
         "Mikhail Danilov", 8, 4, 1,
         "Kirill Chernov", 6, 6, 9,
         3, 8, 2,
         "Kirill Chernov", 6, 6, 9)]
        [InlineData(
         "Evgeny Gavrilov", 2, 3, 9,
         "Oleg Ermakov", 7, 2, 6,
         "Ivan Ignatiev", 2, 9, 7,
         8, 8, 5,
         "Ivan Ignatiev", 2, 9, 7)]
        [InlineData(
         "Vladimir Titov", 10, 4, 10,
         "Andrey Filatov", 1, 8, 5,
         "Pavel Korolev", 6, 10, 1,
         8, 8, 3,
         "Vladimir Titov", 10, 4, 10)]
        [InlineData(
         "Maxim Petukhov", 9, 10, 7,
         "Roman Yashin", 10, 9, 2,
         "Anton Makarov", 10, 10, 3,
         6, 4, 2,
         "Maxim Petukhov", 9, 10, 7)]
        [InlineData(
         "Sergey Kuzin", 2, 5, 8,
         "Denis Vinogradov", 6, 3, 4,
         "Artem Solovyov", 9, 7, 6,
         4, 6, 1,
         "Sergey Kuzin", 2, 5, 8)]
        [InlineData(
         "Fedor Spiridonov", 3, 4, 1,
         "Yuri Gavrilov", 6, 5, 2,
         "Mikhail Danilov", 10, 2, 9,
         8, 10, 1,
         "Mikhail Danilov", 10, 2, 9)]
        [InlineData(
         "Stanislav Trofimov", 4, 2, 3,
         "Kirill Egorov", 7, 5, 5,
         "Alexey Popov", 5, 9, 8,
         2, 5, 3,
         "Alexey Popov", 5, 9, 8)]
        [InlineData(
         "Vladislav Lebedev", 2, 3, 10,
         "Roman Makarov", 9, 1, 2,
         "Ivan Fedorov", 8, 8, 6,
         3, 10, 3,
         "Vladislav Lebedev", 2, 3, 10)]
        [InlineData(
         "Andrey Solovyov", 3, 4, 7,
         "Sergey Gusev", 8, 5, 1,
         "Oleg Petrov", 6, 10, 5,
         6, 1, 3,
         "Andrey Solovyov", 3, 4, 7)]
        [InlineData(
         "Pavel Belov", 2, 5, 4,
         "Anton Egorov", 9, 4, 3,
         "Dmitry Mironov", 8, 9, 8,
         6, 1, 3,
         "Dmitry Mironov", 8, 9, 8)]
        [InlineData(
         "Pavel Belov", 4, 3, 9,
         "Anton Egorov", 9, 5, 2,
         "Dmitry Mironov", 3, 8, 6,
         7, 1, 2,
         "Pavel Belov", 4, 3, 9)]
        [InlineData(
         "Maxim Pavlov", 10, 3, 1,
         "Kirill Kuznetsov", 10, 5, 7,
         "Nikolay Antonov", 10, 7, 8,
         3, 3, 2,
         "Kirill Kuznetsov", 10, 5, 7)]
        [InlineData(
         "Vladimir Morozov", 2, 4, 5,
         "Artem Vinogradov", 9, 4, 10,
         "Stanislav Ivanov", 6, 8, 7,
         5, 5, 5,
         "Artem Vinogradov", 9, 4, 10)]
        [InlineData(
         "Mikhail Smirnov", 5, 5, 2,
         "Ivan Sokolov", 9, 4, 3,
         "Alexey Volkov", 8, 9, 10,
         1, 1, 4,
         "Alexey Volkov", 8, 9, 10)]
        [InlineData(
         "Roman Orlov", 3, 4, 6,
         "Andrey Novikov", 1, 9, 4,
         "Denis Stepanov", 7, 9, 9,
         10, 1, 2,
         "Denis Stepanov", 7, 9, 9)]
        [InlineData(
         "Sergey Nikolaev", 10, 5, 3,
         "Pavel Mikhailov", 8, 8, 7,
         "Igor Andreev", 5, 10, 9,
         5, 2, 2,
         "Igor Andreev", 5, 10, 9)]
        [InlineData(
         "Anton Vasiliev", 3, 3, 10,
         "Oleg Romanov", 6, 6, 1,
         "Maxim Bogdanov", 10, 10, 8,
         7, 2, 1,
         "Anton Vasiliev", 3, 3, 10)]
        [InlineData(
         "Kirill Belov", 3, 3, 2,
         "Dmitry Tarasov", 6, 6, 8,
         "Ivan Filippov", 10, 10, 5,
         10, 1, 1,
         "Dmitry Tarasov", 6, 6, 8)]
        [InlineData(
         "Mikhail Markov", 10, 2, 7,
         "Sergey Borisov", 5, 10, 3,
         "Pavel Zaitsev", 10, 8, 6,
         1, 4, 1,
         "Mikhail Markov", 10, 2, 7)]
        [InlineData(
         "Alexey Denisov", 4, 3, 9,
         "Andrey Mironov", 7, 5, 1,
         "Roman Antonov", 3, 9, 10,
         10, 5, 1,
         "Alexey Denisov", 4, 3, 9)]
        [InlineData(
         "Anton Gavrilov", 5, 3, 4,
         "Vladimir Dmitriev", 8, 6, 2,
         "Kirill Ignatov", 9, 2, 5,
         2, 8, 1,
         "Anton Gavrilov", 5, 3, 4)]
        [InlineData(
         "Denis Kirillov", 3, 4, 8,
         "Nikolay Afanasyev", 6, 7, 9,
         "Maxim Leonov", 3, 9, 6,
         10, 4, 1,
         "Nikolay Afanasyev", 6, 7, 9)]
        [InlineData(
         "Ivan Gusev", 3, 4, 3,
         "Sergey Nikitin", 9, 4, 10,
         "Pavel Karpov", 6, 9, 5,
         6, 1, 3,
         "Sergey Nikitin", 9, 4, 10)]
        [InlineData(
         "Oleg Savin", 4, 5, 7,
         "Alexey Loginov", 7, 9, 8,
         "Andrey Solovyov", 8, 3, 2,
         2, 1, 2,
         "Oleg Savin", 4, 5, 7)]
        [InlineData(
         "Roman Krylov", 8, 2, 6,
         "Anton Zhuravlev", 8, 6, 1,
         "Mikhail Frolov", 2, 10, 4,
         3, 4, 2,
         "Roman Krylov", 8, 2, 6)]
        [InlineData(
         "Kirill Gromov", 2, 5, 5,
         "Dmitry Yashin", 9, 4, 9,
         "Ivan Korolev", 6, 10, 3,
         4, 1, 2,
         "Dmitry Yashin", 9, 4, 9)]
        [InlineData(
         "Sergey Yermakov", 5, 2, 10,
         "Pavel Belousov", 8, 6, 7,
         "Oleg Prokhorov", 3, 8, 4,
         1, 5, 3,
         "Sergey Yermakov", 5, 2, 10)]
        [InlineData(
         "Andrey Rogov", 1, 5, 2,
         "Anton Sorokin", 7, 10, 6,
         "Roman Maltsev", 10, 8, 9,
         8, 1, 1,
         "Roman Maltsev", 10, 8, 9)]
        [InlineData(
         "Alexey Elizarov", 2, 1, 8,
         "Nikolay Dronov", 7, 1, 5,
         "Vladimir Chernov", 10, 7, 4,
         5, 10, 1,
         "Alexey Elizarov", 2, 1, 8)]
        [InlineData(
         "Mikhail Samsonov", 1, 3, 1,
         "Pavel Arkhipov", 10, 1, 10,
         "Igor Abramov", 10, 9, 6,
         4, 6, 7,
         "Pavel Arkhipov", 10, 1, 10)]
        [InlineData(
         "Igor Abramov", 5, 2, 3,
         "Kirill Fedotov", 9, 5, 2,
         "Roman Loginov", 7, 9, 7,
         1, 7, 7,
         "Roman Loginov", 7, 9, 7)]
        [InlineData(
         "Maxim Pavlov", 6, 2, 9,
         "Dmitry Loginov", 9, 9, 8,
         "Konstantin Morozov", 8, 9, 10,
         1, 7, 7,
         "Konstantin Morozov", 8, 9, 10)]
        [InlineData(
         "Ruslan Savin", 5, 2, 5,
         "Evgeny Belov", 9, 7, 6,
         "Sergey Vlasov", 6, 9, 7,
         4, 6, 7,
         "Sergey Vlasov", 6, 9, 7)]
        [InlineData(
         "Kirill Fedotov", 5, 1, 2,
         "Ivan Nikitin", 7, 6, 1,
         "Artem Spiridonov", 2, 10, 8,
         2, 5, 7,
         "Artem Spiridonov", 2, 10, 8)]
        [InlineData(
         "Konstantin Morozov", 1, 3, 10,
         "Denis Matveev", 1, 8, 3,
         "Denis Kirillov", 10, 8, 4,
         8, 3, 7,
         "Konstantin Morozov", 1, 3, 10)]
        [InlineData(
         "Artyom Rybakov", 4, 3, 6,
         "Stepan Tarasov", 10, 2, 9,
         "Andrey Kozlov", 6, 9, 1,
         7, 5, 7,
         "Stepan Tarasov", 10, 2, 9)]
        [InlineData(
         "Semyon Kirillov", 3, 4, 7,
         "Stepan Gromov", 8, 6, 5,
         "Arseniy Dronov", 7, 1, 8,
         5, 10, 2,
         "Semyon Kirillov", 3, 4, 7)]
        [InlineData(
         "Oleg Pavlov", 6, 3, 3,
         "Evgeny Gavrilov", 2, 5, 10,
         "Maxim Pavlov", 9, 9, 9,
         5, 9, 1,
         "Maxim Pavlov", 9, 9, 9)]
        [InlineData(
         "Vladimir Morozov", 5, 2, 4,
         "Sergey Nikolaev", 9, 5, 7,
         "Ivan Filippov", 3, 8, 2,
         8, 9, 7,
         "Sergey Nikolaev", 9, 5, 7)]
        [InlineData(
         "Kirill Ignatov", 3, 3, 8,
         "Oleg Savin", 9, 5, 6,
         "Sergey Yermakov", 6, 8, 1,
         1, 1, 2,
         "Kirill Ignatov", 3, 3, 8)]
        [InlineData(
         "Oleg Prokhorov", 4, 4, 5,
         "Pavel Arkhipov", 7, 5, 3,
         "Anton Egorov", 5, 6, 2,
         6, 2, 2,
         "Oleg Prokhorov", 4, 4, 5)]
        [InlineData(
         "Stepan Lapin", 7, 4, 10,
         "Dmitry Semyonov", 3, 6, 8,
         "Anton Bykov", 5, 8, 7,
         1, 8, 2,
         "Dmitry Semyonov", 3, 6, 8)]
        [InlineData(
         "Filipp Chernov", 3, 3, 9,
         "Maxim Abramov", 7, 6, 4,
         "Sergey Borisov", 9, 1, 6,
         10, 2, 5,
         "Sergey Borisov", 9, 1, 6)]
        [InlineData(
         "Pavel Alekseev", 5, 2, 2,
         "Artem Spiridonov", 8, 8, 5,
         "Andrey Solovyov", 2, 9, 7,
         4, 5, 2,
         "Andrey Solovyov", 2, 9, 7)]
        [InlineData(
         "Sergey Kuzin", 3, 3, 6,
         "Fedor Spiridonov", 8, 3, 8,
         "Ivan Petrov", 6, 7, 9,
         2, 8, 2,
         "Ivan Petrov", 6, 7, 9)]
        [InlineData(
         "Platon Ignatov", 8, 3, 1,
         "Vsevolod Gusev", 9, 6, 3,
         "Pavel Belov", 3, 5, 10,
         6, 3, 2,
         "Pavel Belov", 3, 5, 10)]
        [InlineData(
         "Denis Kirillov", 5, 6, 4,
         "Alexey Loginov", 2, 8, 5,
         "Mikhail Lebedev", 5, 9, 9,
         2, 1, 1,
         "Mikhail Lebedev", 5, 9, 9)]
        [InlineData(
         "Roman Antonov", 3, 5, 7,
         "Ivan Gusev", 8, 9, 2,
         "Mikhail Frolov", 3, 5, 6,
         5, 3, 1,
         "Roman Antonov", 3, 5, 7)]
        [InlineData(
         "Maxim Leonov", 1, 1, 8,
         "Anton Zhuravlev", 3, 4, 10,
         "Alexey Nazarov", 5, 7, 5,
         10, 10, 1,
         "Anton Zhuravlev", 3, 4, 10)]
        [InlineData(
         "Evgeny Gavrilov", 9, 3, 9,
         "Sergey Ivanov", 7, 6, 1,
         "Evgeny Belov", 1, 10, 7,
         3, 3, 3,
         "Evgeny Gavrilov", 9, 3, 9)]
        [InlineData(
         "Timofey Zaitsev", 9, 3, 3,
         "Vadim Loginov", 3, 7, 4,
         "Filipp Chernov", 7, 9, 2,
         3, 2, 10,
         "Vadim Loginov", 3, 7, 4)]
        [InlineData(
         "Filipp Chernov", 1, 1, 10,
         "Fedor Antonov", 6, 3, 6,
         "Anton Popov", 6, 10, 8,
         10, 8, 10,
         "Anton Popov", 6, 10, 8)]
        [InlineData(
         "Vyacheslav Mikhailov", 4, 1, 2,
         "Stepan Tarasov", 6, 5, 7,
         "Eduard Borisov", 3, 7, 9,
         1, 4, 4,
         "Eduard Borisov", 3, 7, 9)]
        [InlineData(
         "Evgeny Belov", 8, 2, 5,
         "Vladislav Golubev", 4, 5, 1,
         "Ilya Efimov", 8, 10, 8,
         6, 8, 4,
         "Ilya Efimov", 8, 10, 8)]
        [InlineData(
         "Viktor Trofimov", 4, 4, 6,
         "Pavel Korolev", 7, 5, 3,
         "Sergey Kuzin", 3, 8, 7,
         6, 10, 2,
         "Sergey Kuzin", 3, 8, 7)]
        [InlineData(
         "Ivan Ignatiev", 3, 4, 4,
         "Kirill Egorov", 3, 7, 10,
         "Andrey Solovyov", 3, 10, 9,
         8, 8, 3,
         "Kirill Egorov", 3, 7, 10)]
        [InlineData(
         "Alexey Popov", 3, 2, 8,
         "Mikhail Danilov", 7, 5, 2,
         "Stepan Lapin", 8, 10, 6,
         9, 2, 1,
         "Alexey Popov", 3, 2, 8)]
        [InlineData(
         "Vladimir Titov", 6, 4, 7,
         "Denis Safronov", 9, 7, 5,
         "Anton Bykov", 3, 9, 3,
         3, 2, 9,
         "Vladimir Titov", 6, 4, 7)]
        [InlineData(
         "Timur Belyaev", 7, 3, 9,
         "Nikita Chistyakov", 3, 7, 8,
         "Oleg Ermakov", 9, 6, 4,
         2, 1, 1,
         "Timur Belyaev", 7, 3, 9)]
        [InlineData(
         "Artem Solovyov", 10, 1, 1,
         "Stanislav Trofimov", 8, 4, 6,
         "Roman Orlov", 5, 8, 2,
         4, 2, 1,
         "Stanislav Trofimov", 8, 4, 6)]
        [InlineData(
         "Sergey Nikolaev", 5, 1, 10,
         "Mikhail Frolov", 3, 8, 9,
         "Sergey Golubev", 6, 10, 7,
         10, 5, 1,
         "Sergey Nikolaev", 5, 1, 10)]
        [InlineData(
         "Pavel Arkhipov", 2, 2, 3,
         "Nikolay Afanasyev", 6, 5, 8,
         "Andrey Solovyov", 8, 9, 6,
         3, 8, 3,
         "Nikolay Afanasyev", 6, 5, 8)]
        [InlineData(
         "Sergey Nikitin", 1, 1, 2,
         "Ivan Sokolov", 10, 1, 10,
         "Alexey Popov", 10, 10, 4,
         1, 10, 3,
         "Ivan Sokolov", 10, 1, 10)]
        [InlineData(
         "Denis Vinogradov", 7, 1, 7,
         "Oleg Ermakov", 2, 8, 1,
         "Mark Elizarov", 6, 8, 9,
         1, 3, 3,
         "Mark Elizarov", 6, 8, 9)]
        [InlineData(
         "Anatoly Panov", 5, 1, 5,
         "Pavel Korolev", 9, 6, 2,
         "Artem Solovyov", 4, 7, 8,
         2, 4, 1,
         "Artem Solovyov", 4, 7, 8)]
        [InlineData(
         "Mikhail Danilov", 6, 1, 6,
         "Dmitry Mironov", 4, 3, 4,
         "Kirill Kuznetsov", 6, 9, 3,
         10, 8, 3,
         "Kirill Kuznetsov", 6, 9, 3)]
        [InlineData(
         "Mikhail Smirnov", 2, 7, 9,
         "Anton Vasiliev", 10, 7, 7,
         "Vladimir Chernov", 6, 10, 5,
         5, 3, 2,
         "Mikhail Smirnov", 2, 7, 9)]
        [InlineData(
         "Alexey Denisov", 1, 5, 10,
         "Stanislav Ivanov", 1, 9, 8,
         "Stanislav Trofimov", 10, 8, 1,
         8, 2, 2,
         "Alexey Denisov", 1, 5, 10)]
        [InlineData(
         "Pavel Korolev", 5, 1, 4,
         "Roman Yashin", 5, 10, 6,
         "Yuri Gavrilov", 10, 10, 2,
         5, 1, 2,
         "Pavel Korolev", 5, 1, 4)]
        [InlineData(
         "Sergey Kuzin", 8, 3, 3,
         "Stepan Gromov", 1, 7, 9,
         "Filipp Chernov", 7, 9, 5,
         8, 9, 4,
         "Filipp Chernov", 7, 9, 5)]
        [InlineData(
         "Sergey Kuzin", 5, 8, 8,
         "Stepan Gromov", 6, 8, 10,
         "Filipp Chernov", 5, 9, 6,
         6, 9, 4,
         "Stepan Gromov", 6, 8, 10)]


        public void WhenDistributingOrderOnCouriers_AndFoundTheMostMatchingCourierForOrder_ThenMethodShouldBeReturnSuccessResult(
            string firstCourierName,
            int xFirstCourier,
            int yFirstCourier,
            int firstCourierSpeed,
            string secondCourierName,
            int xSecondCourier,
            int ySecondCourier,
            int secondCourierSpeed,
            string thirdCourierName,
            int xThirdCourier,
            int yThirdCourier,
            int thirdCourierSpeed,
            int xOrder,
            int yOrder,
            int orderVolume,
            string chousingCourierName,
            int chousingCourierX,
            int chousingCourierY,
            int chousingCourierSpeed)
        {
            //Arrange
            CourierDistributorService courierDistributor = new();

            List<Courier> couriers = 
            [
                Courier.Create(firstCourierSpeed, firstCourierName, Location.Create(xFirstCourier, yFirstCourier).Value).Value,
                Courier.Create(secondCourierSpeed, secondCourierName, Location.Create(xSecondCourier, ySecondCourier).Value).Value,
                Courier.Create(thirdCourierSpeed, thirdCourierName, Location.Create(xThirdCourier, yThirdCourier).Value).Value,
            ];

            var basketId = Guid.NewGuid();
            var orderLocation = Location.Create(xOrder, yOrder).Value;
            var order = Order.Create(basketId, orderLocation, orderVolume).Value;

            //Act
            var courierDistributionResult = courierDistributor.DistributeOrderOnCouriers(order, couriers);

            //Assert
            Assert.True(courierDistributionResult.IsSuccess);
            Assert.False(courierDistributionResult.IsFailure);
            Assert.True(order.CourierId != null);
            Assert.True(order.Status.Name == "assigned");
            Assert.Contains(order.CourierId.Value, couriers.Select(courier => courier.Id).ToList());
            Assert.True(couriers.Where(courier => courier.StoragePlaces.Where(storagePlace => storagePlace.OrderId == order.Id).Count() == 1
                && courier.Name == chousingCourierName
                && courier.CourierLocation.X == chousingCourierX
                && courier.CourierLocation.Y == chousingCourierY
                && courier.Speed == chousingCourierSpeed).Count() == 1);
        }
    }
}