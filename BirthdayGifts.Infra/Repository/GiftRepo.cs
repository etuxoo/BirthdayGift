using BirthdayGifts.Infra.Record;
using System;
using System.Collections.Generic;
using System.Text;

namespace BirthdayGifts.Infra.Repository
{
    public class GiftRepo : IGiftRepository
    {
        // TODO : Add DI for the connection

        public IEnumerable<GiftRecord> Create(IEnumerable<GiftRecord> records)
        {
            // TODO
            throw new NotImplementedException();
        }

        public IEnumerable<GiftRecord> Read(IEnumerable<GiftRecord> records)
        {
            // TODO
            throw new NotImplementedException();
        }

        public IEnumerable<GiftRecord> Update(IEnumerable<GiftRecord> records)
        {
            // TODO
            throw new NotImplementedException();
        }

        public IEnumerable<GiftRecord> Delete(IEnumerable<GiftRecord> records)
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}
