using System;

namespace BirthdayGifts.Infra.Record
{
    public class UserRecord
    {
        public DateTime? BirthDate { get; set; }

        public int? Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }
    }
}
