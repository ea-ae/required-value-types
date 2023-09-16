using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;

namespace RequiredValueTypes.RequiredValueTypes.ValueProviders;

public class JsonValueProviderFactory : IValueProviderFactory
{
    public async Task CreateValueProviderAsync(ValueProviderFactoryContext context)
    {
        var body = await GetBody(context.ActionContext.HttpContext);
        var provider = new JsonValueProvider(body);
        context.ValueProviders.Insert(0, provider);
    }

    private static async Task<string> GetBody(HttpContext httpContext)
    {
        var encoding = GetEncoding(httpContext);
        var inputStream = GetInputStream(httpContext, encoding);

        using var reader = new StreamReader(inputStream);
        var body = await reader.ReadToEndAsync()
                               .ConfigureAwait(continueOnCapturedContext: false);

        return body;
    }

    private static Stream GetInputStream(HttpContext httpContext, Encoding encoding)
    {
        if (encoding.CodePage == Encoding.UTF8.CodePage)
        {
            return httpContext.Request.Body;
        }

        return Encoding.CreateTranscodingStream(httpContext.Request.Body, 
                                                encoding,
                                                Encoding.UTF8, 
                                                leaveOpen: true);
    }

    private static Encoding GetEncoding(HttpContext httpContext)
    {
        var contentType = httpContext.Request.ContentType;

        var mediaType = string.IsNullOrEmpty(contentType)
            ? default
            : new MediaType(contentType);

        var encoding = mediaType.Charset.HasValue ? mediaType.Encoding : null;
        return encoding ?? Encoding.UTF8;
    }
}
