using System.Net;
using PicPay.Api.Errors;

namespace PicPay.Tests.Extensions;

public static class JsonExtensions
{
    public static async Task AssertBadRequest(this HttpResponseMessage httpResponse, PicPayError PicPayError)
    {
        var error = await httpResponse.DeserializeTo<ErrorOut>();
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(PicPayError.Message);
        error.Code.Should().Be(PicPayError.Code);
    }

    public static void ShouldBeError<S>(this OneOf<S, ErrorOut> oneOf, PicPayError expected)
    {
        oneOf.IsSuccess().Should().BeFalse();
        oneOf.IsError().Should().BeTrue();
        oneOf.GetError().Code.Should().Be(expected.Code);
        oneOf.GetError().Message.Should().Be(expected.Message);
    }

    public static void ShouldBeSuccess<S, E>(this OneOf<S, E> oneOf)
    {
        oneOf.IsSuccess().Should().BeTrue();
        oneOf.IsError().Should().BeFalse();
    }

    public static void ShouldBeError<S>(this OneOf<S, PicPayError> oneOf, PicPayError expected)
    {
        oneOf.IsSuccess().Should().BeFalse();
        oneOf.IsError().Should().BeTrue();
        oneOf.GetError().Should().BeOfType(expected.GetType());
        oneOf.GetError().Code.Should().Be(expected.Code);
        oneOf.GetError().Message.Should().Be(expected.Message);
    }

    public static void ShouldBeError<S>(this OneOf<S, ErrorOut> oneOf, ErrorOut expected)
    {
        oneOf.IsSuccess().Should().BeFalse();
        oneOf.IsError().Should().BeTrue();
        oneOf.GetError().Code.Should().Be(expected.Code);
        oneOf.GetError().Message.Should().Be(expected.Message);
    }
}
