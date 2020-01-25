using BirthdayGifts.Infra.Record;
using System.Collections.Generic;

namespace BirthdayGifts.Infra
{
    public interface IGiftRepository
    {
        public IEnumerable<GiftRecord> Create(IEnumerable<GiftRecord> records);
       
        public IEnumerable<GiftRecord> Read(IEnumerable<GiftRecord> records);

        public IEnumerable<GiftRecord> Update(IEnumerable<GiftRecord> records);

        public IEnumerable<GiftRecord> Delete(IEnumerable<GiftRecord> records);
    }
}
