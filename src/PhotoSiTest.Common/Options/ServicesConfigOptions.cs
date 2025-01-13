namespace PhotoSiTest.Common.Options;

public class ServicesConfigOptions
{
    public required int? TimeoutSeconds { get; set; }

    public required string UsersApiBaseUrl { get; set; }


    public Uri GetUsersApiBaseUriOrThrow()
    {
        if (!Uri.TryCreate(UsersApiBaseUrl, UriKind.Absolute, out var uri))
        {
            throw new ApplicationException($"configured {nameof(UsersApiBaseUrl)} '{UsersApiBaseUrl}' is not a valid Uri");
        }

        return uri;
    }
}
