using BirthdayGifts.Infra.Record;
using System.Collections.Generic;

namespace BirthdayGifts.Infra
{
    public interface IUserRepository
    {
        public int Create(IEnumerable<UserRecord> records);

        public IEnumerable<UserRecord> Read(IEnumerable<int> recordIds);

        public IEnumerable<UserRecord> Read(IEnumerable<string> recordNames);

        public int Update(IEnumerable<UserRecord> records);

        public int Delete(IEnumerable<int> recordIds);
    }
}
