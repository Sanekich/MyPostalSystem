using System.Text.Json.Serialization;

namespace Models
{
    public class ParcelModel
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public int FromBuildingId { get; set; }
        public int ToBuildingId { get; set; }
        public int TypeId { get; set; }
        public string? Description { get; set; }
        public int Distance { get; set; }
        public int Cost { get; set; }
        public override string ToString()
        {
            return Description;
        }
    }
}