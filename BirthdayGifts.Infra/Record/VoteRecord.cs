namespace BirthdayGifts.Infra.Record
{
    public class VoteRecord
    {
        public int Id { get; set; }

        public int GiftId { get; set; }

        public int UserId { get; set; }

        public int VotingId { get; set; }
    }
}
