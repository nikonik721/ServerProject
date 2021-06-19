using MessagePack;

namespace PS.Models.Models
{
    [MessagePackObject]

    public class Mail
    {
        [Key(0)]
        public int Id { get; set; }
        [Key(1)]
        public int EnvelopeNumber { get; set; }
    }
}
