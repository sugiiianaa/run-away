using RunAway.Domain.Commons;

namespace RunAway.Domain.Entities
{
    public class RoomAvailableRecordEntity : AuditableEntity<Guid>
    {
        public Guid RoomId { get; private set; }
        public DateOnly Date { get; private set; }
        public int AvailableRooms { get; private set; }

        public RoomEntity Room { get; private set; }

        private RoomAvailableRecordEntity() { }

        public static RoomAvailableRecordEntity Create(Guid id, Guid roomId, DateOnly date, int availableRooms)
        {
            if (availableRooms < 0)
                throw new ArgumentException("Available rooms cannot be negative", nameof(availableRooms));

            var today = DateOnly.FromDateTime(DateTime.Today);

            if (date < today)
                throw new ArgumentException("Available date cannot be in the past", nameof(date));

            return new RoomAvailableRecordEntity
            {

                ID = id,
                RoomId = roomId,
                Date = date,
                AvailableRooms = availableRooms,
            };
        }

        public void UpdateAvailableRooms(int availableRooms)
        {
            if (availableRooms < 0)
                throw new ArgumentException("Available rooms cannot be negative", nameof(availableRooms));

            AvailableRooms = availableRooms;
        }

        public void DecreaseAvailableRooms(int count)
        {
            if (count <= 0)
                throw new ArgumentException("Count must be greater than zero", nameof(count));

            if (AvailableRooms < count)
                throw new InvalidOperationException("Not enough rooms available");

            AvailableRooms -= count;
        }
    }
}
