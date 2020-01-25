using BirthdayGifts.Infra.Record;
using Dapper.FluentMap.Mapping;

namespace BirthdayGifts.Infra.Schema
{
    public class VoteSchema : EntityMap<VoteRecord>
    {
        public VoteSchema()
        {
            Map(i => i.Id).ToColumn("id");
            Map(i => i.UserId).ToColumn("userID");
            Map(i => i.VotingId).ToColumn("votingID");
            Map(i => i.GiftId).ToColumn("giftID");
        }
    }
}
