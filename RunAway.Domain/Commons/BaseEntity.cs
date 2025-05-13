namespace RunAway.Domain.Commons
{
    public class BaseEntity<T>
    {
        public T ID { get; protected set; } = default!;

        protected BaseEntity() { }  // For Entity Framework

        protected BaseEntity(T id)
        {
            ID = id;
        }
    }
}
