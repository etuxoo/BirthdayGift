using BirthdayGifts.Infra.Record;
using System.Collections.Generic;

namespace BirthdayGifts.Infra
{
    public interface IGiftRepository
    {
        public int Create(IEnumerable<GiftRecord> records);
       
        public IEnumerable<GiftRecord> Read(IEnumerable<int> recordIds);

        public IEnumerable<GiftRecord> Read(IEnumerable<string> recordNames);

        public int Update(IEnumerable<GiftRecord> records);

        public int Delete(IEnumerable<int> recordIds);
    }
}
