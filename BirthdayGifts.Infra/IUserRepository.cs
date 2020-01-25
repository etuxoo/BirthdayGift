using BirthdayGifts.Infra.Record;
using System.Collections.Generic;

namespace BirthdayGifts.Infra
{
    public interface IUserRepository
    {
        public IEnumerable<UserRecord> Create(IEnumerable<UserRecord> records);

        public IEnumerable<UserRecord> Read(IEnumerable<UserRecord> records);

        public IEnumerable<UserRecord> Update(IEnumerable<UserRecord> records);

        public IEnumerable<UserRecord> Delete(IEnumerable<UserRecord> records);
    }
}
