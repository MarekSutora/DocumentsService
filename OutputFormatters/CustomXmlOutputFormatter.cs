using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text;
using System.Xml;
using DocumentsService.API.Models;
using ExtendedXmlSerializer;
using ExtendedXmlSerializer.Configuration;
using DocumentsService.API.DTOs.Response;

namespace DocumentsService.API.OutputFormatters
{
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
            if (typeof(ReadDocumentDto).IsAssignableFrom(type))
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
            var document = context.Object as ReadDocumentDto;

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
        }
    }
}


