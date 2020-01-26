using BirthdayGifts.Infra.Record;
using Dapper.FluentMap.Mapping;

namespace BirthdayGifts.Infra.Schema
{
    public class VotingSchema : EntityMap<VotingRecord> 
    {
        public VotingSchema()
        {
            Map(i => i.Id).ToColumn("id");
            Map(i => i.Date).ToColumn("date");
            Map(i => i.IsOpen).ToColumn("isOpen");
            Map(i => i.StartedBy).ToColumn("startedBy");
            Map(i => i.UserId).ToColumn("userID");
        }
    }
}
