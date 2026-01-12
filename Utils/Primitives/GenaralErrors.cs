using Microsoft.EntityFrameworkCore.Storage.Json;
using System.Diagnostics.CodeAnalysis;

namespace Primitives;

/// <summary>
///     Общие ошибки
/// </summary>
public static class GeneralErrors
{
    [ExcludeFromCodeCoverage]
    public static Error NotFound(long? id = null)
    {
        var forId = id == null ? "" : $" for Id '{id}'";
        return new Error("record.not.found", $"Record not found{forId}");
    }

    [ExcludeFromCodeCoverage]
    public static Error NotFound(string name)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentException(name);
        return new Error("value.not.found", $"Value not found {name}");
    }

    [ExcludeFromCodeCoverage]
    public static Error OrderCannotBeStored(Error innerError = null)
    {
        return new Error(
            "order.null.or.order.volume.is.not.correct.or.storage.place.already.have.order", 
            "Order cannot be stored, order volume equal zero or order volume greeter than total volume and order id is not null", 
            innerError);
    }

    [ExcludeFromCodeCoverage]
    public static Error StorageCannotBeCreated(Error innerError = null)
    { 
        return new Error(
            "courier.cannot.store.order.to.storage.place", 
            "Storage cannot be created, storage name or capacity is not correct", 
            innerError);
    }

    [ExcludeFromCodeCoverage]
    public static Error NotFoundMatchingStorageForStoreOrder(Error innerError = null)
    {
        return new Error(
            "not.found.matching.storage.for.store",
            "Courier cannot take order, not found mathcing storage for store",
            innerError);
    }

    [ExcludeFromCodeCoverage]
    public static Error TakeOrderError(Error innerError = null)
    {
        return new Error(
            "take.order.result.false.or.an.error.occured",
            "When courier take order, result false or an error occured",
            innerError);
    }

    [ExcludeFromCodeCoverage]
    public static Error CreateOrderError(Error innerError = null)
    {
        return new Error(
            "create.order.with.kafka,error",
            "When creating order, error occured",
            innerError);
    }

    [ExcludeFromCodeCoverage]
    public static Error DistributeOrderOnCouriersError(Error innerError = null)
    {
        return new Error(
            "cant.distribute.order.on.courier",
            "Cant distribute order on courier",
            innerError);
    }

    [ExcludeFromCodeCoverage]
    public static Error IsCourierCanTakeOrderError(Error innerError = null)
    {
        return new Error(
            "is.courier.can.take.order.error",
            "When checking is courier can take the order, an error occured",
            innerError);
    }

    [ExcludeFromCodeCoverage]
    public static Error CourierCannotFinishOrder(Error innerError = null)
    {
        return new Error(
            "order.id.is.already.exist.in.storage.place", 
            "Courier cannot finishing order, order id is already exist in storage place", 
            innerError);
    }

    [ExcludeFromCodeCoverage]
    public static Error CourierCannotCalculateDistanceToTargetLocation(Error innerError = null)
    {
        return new Error(
            "target.location.is.null.or.calculating.result.less.or.equal.zero", 
            "Courier cannot calculate distance to target location, target location is null or calculating result less or equal zero", 
            innerError);
    }

    [ExcludeFromCodeCoverage]
    public static Error OrderCannotBeDistributedError(Error innerError = null)
    {
        return new Error(
            "order.cannot.be.distributed",
            "Order cannot be distributed, some of input values is not correct or not found matching courer for distribute",
            innerError);
    }

    [ExcludeFromCodeCoverage]
    public static Error NotFoundMatchingCourierForOrder()
    {
        return new Error(
            "not.found.matching.courier.for.order",
            "When trying find matching courier for the order, not found matching couriers for the order");
    }

    [ExcludeFromCodeCoverage]
    public static Error CourierCannotMoveToTargetLocation(Error innerError = null)
    {
        return new Error(
            "target.location.is.null.or.x.and.y.coordinates.is.not.correct", 
            "Courier cannot move to target location, target location null or X and Y coordinates is not correct", 
            innerError);
    }

    [ExcludeFromCodeCoverage]
    public static Error ValueIsInvalid(string name)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentException(name);
        return new Error("value.is.invalid", $"Value is invalid for {name}");
    }

    [ExcludeFromCodeCoverage]
    public static Error ValueIsRequired(string name)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentException(name);
        return new Error("value.is.required", $"Value is required for {name}");
    }

    [ExcludeFromCodeCoverage]
    public static Error InvalidLength(string name)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentException(name);
        return new Error("invalid.string.length", $"Invalid {name} length");
    }

    [ExcludeFromCodeCoverage]
    public static Error CollectionIsTooSmall(int min, int current)
    {
        return new Error(
            "collection.is.too.small",
            $"The collection must contain {min} items or more. It contains {current} items.");
    }

    [ExcludeFromCodeCoverage]
    public static Error CollectionIsTooLarge(int max, int current)
    {
        return new Error(
            "collection.is.too.large",
            $"The collection must contain {max} items or more. It contains {current} items.");
    }

    [ExcludeFromCodeCoverage]
    public static Error InternalServerError(string message)
    {
        return new Error("internal.server.error", message);
    }
}