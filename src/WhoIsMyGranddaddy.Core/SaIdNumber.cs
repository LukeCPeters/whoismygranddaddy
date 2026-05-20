namespace WhoIsMyGranddaddy.Core;

public static class SaIdNumber
{
    public static bool IsValid(string? value)
    {
        if (value is not { Length: 13 } || !value.All(char.IsDigit))
            return false;

        var sum = 0;
        for (var i = 0; i < 13; i++)
        {
            var digit = value[12 - i] - '0';
            if (i % 2 == 1)
            {
                digit *= 2;
                if (digit > 9)
                    digit -= 9;
            }
            sum += digit;
        }
        return sum % 10 == 0;
    }
}
