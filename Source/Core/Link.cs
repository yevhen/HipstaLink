using System.Linq;
using System;

namespace HipstaLink
{
    public struct Link
    {
        public static readonly Link None = new Link();

        public readonly string Rel;
        public readonly Uri Href;

        public Link(string rel, string uri)
            : this(rel, new Uri(uri))
        {}

        public Link(string rel, Uri uri)
        {
            Rel = rel;
            Href = uri;
        }

        public Link(string rel, RouteLink link)
            : this(rel, link.AbsoluteUri)
        {}

        public Link(RouteLink link)
            : this(link.Method.Name, link)
        {}

        public bool Equals(Link other)
        {
            return string.Equals(Rel, other.Rel) && Equals(Href, other.Href);
        }

        public override bool Equals(object obj)
        {
            return !ReferenceEquals(null, obj) && (obj is Link && Equals((Link)obj));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Rel != null ? Rel.GetHashCode() : 0) * 397) ^ (Href != null ? Href.GetHashCode() : 0);
            }
        }

        public static bool operator ==(Link left, Link right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Link left, Link right)
        {
            return !left.Equals(right);
        }
    }
}