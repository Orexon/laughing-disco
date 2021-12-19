namespace LaneSelection.Domain.Entities
{
    public class Load
    {
        public int Id { get; private set; }

        public Load(int id)
        {
            Id = id;
        }
    }
}