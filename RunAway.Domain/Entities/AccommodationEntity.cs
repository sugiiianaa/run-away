using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RunAway.Domain.Commons;
using RunAway.Domain.ValueObjects;

namespace RunAway.Domain.Entities
{
    public class AccommodationEntity : AuditableEntity<Guid>
    {
        public string Name { get; private set; } = string.Empty;
        public string Address { get; private set; } = string.Empty;
        public Coordinate Coordinate { get; private set; } = default!;

        private readonly List<ImageUrl> _imageUrls = [];
        
        public IReadOnlyCollection<ImageUrl> ImageUrls => _imageUrls.AsReadOnly();

        private AccommodationEntity() { } // For entity framework

        public AccommodationEntity(
            Guid id,
            string name,
            string address,
            Coordinate coordinate,
            List<ImageUrl> imageUrls) : base(id)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Accommodation name cannot be empty.", nameof(name));

            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Accommodation address cannot be empty.", nameof(address));

            if (coordinate == null)
                throw new ArgumentNullException(nameof(coordinate), "Coordinate is required.");

            if (imageUrls == null || !imageUrls.Any())
                throw new ArgumentException("At least one image is required.", nameof(imageUrls));


            Id = id;
            Name = name;
            Address = address;
            Coordinate = coordinate;
            _imageUrls.AddRange(imageUrls);
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("New name cannot be empty.", nameof(newName));

            Name = newName;
            SetUpdatedAt();
        }

        public void UpdateAddress(string newAddress)
        {
            if (string.IsNullOrWhiteSpace(newAddress))
                throw new ArgumentException("New address cannot be empty.", nameof(newAddress));

            Address = newAddress;
            SetUpdatedAt();
        }

        public void AddImage(ImageUrl imageUrl)
        {
            if (imageUrl == null)
                throw new ArgumentNullException(nameof(imageUrl), "Image URL cannot be null.");

            _imageUrls.Add(imageUrl);
            SetUpdatedAt();
        }
    }
}
