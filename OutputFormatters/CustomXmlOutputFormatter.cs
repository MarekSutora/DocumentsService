using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DocumentsService.API.Models;
using ExtendedXmlSerializer;
using ExtendedXmlSerializer.Configuration;

public class CustomXmlOutputFormatter : TextOutputFormatter
{
    public CustomXmlOutputFormatter()
    {
        SupportedMediaTypes.Add("application/xml");
        SupportedMediaTypes.Add("text/xml");

        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    protected override bool CanWriteType(Type type)
    {
        if (typeof(Document).IsAssignableFrom(type))
        {
            return true;
        }
        return false;
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var response = context.HttpContext.Response;
        var document = context.Object as Document;

        if (document == null)
        {
            throw new InvalidOperationException("Invalid document type");
        }

        IExtendedXmlSerializer serializer = new ConfigurationContainer().UseAutoFormatting().UseOptimizedNamespaces().Create();

        using (var xmlWriter = XmlWriter.Create(response.Body, new XmlWriterSettings { Async = true, Encoding = selectedEncoding }))
        {
            serializer.Serialize(xmlWriter, document);
            await xmlWriter.FlushAsync();
        }

        //var dataContractSerializer = new DataContractSerializer(typeof(Document));

        //using (var stream = new MemoryStream())
        //using (var writer = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true, Encoding = selectedEncoding }))
        //{
        //    dataContractSerializer.WriteObject(writer, document);
        //    writer.Flush();
        //    stream.Position = 0;
        //    await stream.CopyToAsync(response.Body);
        //}
    }
}
