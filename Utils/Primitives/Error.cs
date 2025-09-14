using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;

namespace Primitives;

[ExcludeFromCodeCoverage]
public sealed class Error : ValueObject
{
    private const string Separator = "||";

    [ExcludeFromCodeCoverage]
    private Error() { }

    public Error(string code, string message, Error error = null)
    {
        Code = code;
        Message = message;
        InnerError = error;
    }

    public string Code { get; }

    public string Message { get; }

    public Error InnerError  { get; set; }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Code;
        yield return Message;
    }

    public string Serialize()
    {
        return $"{Code}{Separator}{Message}{InnerError.Message}";
    }

    public static Error Deserialize(string serialized)
    {
        if (serialized == "A non-empty request body is required.")
            return GeneralErrors.ValueIsRequired(nameof(serialized));

        var data = serialized.Split(new[] { Separator }, StringSplitOptions.RemoveEmptyEntries);

        if (data.Length < 2)
            throw new FormatException($"Invalid error serialization: '{serialized}'");

        return new Error(data[0], data[1]);
    }
}