using System.Text.RegularExpressions;

namespace FamilyHubs.ServiceDirectory.Api.Helper;

public static class UtilHelper
{
    public static bool IsValidURL(string URL)
    {
        if (string.IsNullOrEmpty(URL))
            return true;
        string Pattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
        Regex Rgx = new(Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        return Rgx.IsMatch(URL);
    }
}
