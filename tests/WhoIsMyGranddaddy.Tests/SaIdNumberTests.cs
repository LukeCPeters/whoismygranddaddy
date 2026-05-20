using WhoIsMyGranddaddy.Core;
using Xunit;

namespace WhoIsMyGranddaddy.Tests;

public class SaIdNumberTests
{
    [Theory]
    [InlineData("8801235111088")]
    [InlineData("0001015000084")]
    [InlineData("6401015020088")]
    public void Valid_idNumbers_pass(string id) => Assert.True(SaIdNumber.IsValid(id));

    [Theory]
    [InlineData("8801235111080")]
    [InlineData("000101500008")]
    [InlineData("00010150000840")]
    [InlineData("0001015O00084")]
    [InlineData("")]
    [InlineData(null)]
    public void Invalid_idNumbers_fail(string? id) => Assert.False(SaIdNumber.IsValid(id));
}
