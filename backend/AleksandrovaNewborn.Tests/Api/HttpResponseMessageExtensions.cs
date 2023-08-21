namespace AleksandrovaNewborn.Tests.Api;

public static class HttpResponseMessageExtensions
{
    public static int? ExtractIdFromLocation(this HttpResponseMessage response, string resourcePrefix)
    {
        if (response.Headers.Location is null) return null;

        var rawId = response.Headers.Location.ToString().Replace(resourcePrefix, string.Empty).Replace("/", string.Empty);
        if (int.TryParse(rawId, out int result))
        {
            return result;
        }

        return null;
    }    
}