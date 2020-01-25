using BirthdayGifts.Infra.Record;
using Dapper.FluentMap.Mapping;

namespace BirthdayGifts.Infra.Schema
{
    public class UserSchema : EntityMap<UserRecord>
    {
        public UserSchema()
        {
            Map(i => i.Id).ToColumn("id");
            Map(i => i.Name).ToColumn("name");
            Map(i => i.UserName).ToColumn("username");
            Map(i => i.Password).ToColumn("password");
            Map(i => i.BirthDate).ToColumn("birthdate");
        }
    }
}
