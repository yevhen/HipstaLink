using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

using Newtonsoft.Json;

namespace HipstaLink
{
    [JsonConverter(typeof(Converter))]
    public class Links : IEnumerable<Link>
    {
        readonly IDictionary<string, dynamic> pairs = new ExpandoObject();

        public void Add(Link link)
        {
            if (link == Link.None)
                return;

            pairs[link.Rel] = new {link.Href};
        }

        public IEnumerator<Link> GetEnumerator()
        {
            return pairs.Select(pair => new Link(pair.Key, pair.Value.Href)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public class Converter : JsonConverter
        {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, ((Links)value).pairs);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                throw new InvalidOperationException();
            }

            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(Links);
            }
        }
    }
}