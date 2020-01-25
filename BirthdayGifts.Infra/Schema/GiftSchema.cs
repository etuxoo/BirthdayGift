﻿using BirthdayGifts.Infra.Record;
using Dapper.FluentMap.Mapping;

namespace BirthdayGifts.Infra.Schema
{
    public class GiftSchema : EntityMap<UserRecord>
    {
        public GiftSchema()
        {
            Map(i => i.Id).ToColumn("id");
            Map(i => i.Name).ToColumn("name");
        }
    }
}
