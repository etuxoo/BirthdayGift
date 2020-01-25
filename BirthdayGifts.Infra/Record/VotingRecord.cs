using System;

namespace BirthdayGifts.Infra.Record
{
    public class VotingRecord
    {
        public DateTime Date { get; set; }

        public int Id { get; set; }

        public bool IsOpen { get; set; }

        public int StartedBy { get; set; }

        public int UserId { get; set; }
    }
}
