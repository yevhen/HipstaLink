using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace HipstaLink
{
    public class MediaTypeFormatter : System.Net.Http.Formatting.MediaTypeFormatter
    {
        readonly JsonMediaTypeFormatter formatter = new JsonMediaTypeFormatter();

        public MediaTypeFormatter()
        {
            var settings = formatter.SerializerSettings;

            settings.Converters.Add(new StringEnumConverter());
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            #if DEBUG
                settings.Formatting = Formatting.Indented;
            #endif

            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/json"));        // put your own media types here!
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json")); // put your own media types here!
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            return formatter.WriteToStreamAsync(type, value, writeStream, content, transportContext);
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            return formatter.ReadFromStreamAsync(type, readStream, content, formatterLogger);
        }

        public override bool CanReadType(Type type)
        {
            return formatter.CanReadType(type);
        }

        public override bool CanWriteType(Type type)
        {
            return formatter.CanWriteType(type);
        }
    }
}