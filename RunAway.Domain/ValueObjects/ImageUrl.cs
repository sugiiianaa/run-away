using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunAway.Domain.ValueObjects
{
    public sealed class ImageUrl : IEquatable<ImageUrl>
    {
        public string Value { get; }

        public ImageUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("Image URL cannot be empty.", nameof(url));

            if (!Uri.TryCreate(url, UriKind.Absolute, out _))
                throw new ArgumentException("Invalid URL format.", nameof(url));

            Value = url;
        }

        public override bool Equals(object? obj)
        {
            return obj is ImageUrl other && Equals(other);
        }

        public bool Equals(ImageUrl? other)
        {
            if (other is null) return false;

            return other != null
                && Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
