using BirthdayGifts.Infra.Record;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BirthdayGifts.Infra.Repository
{
    public class GiftRepo : IGiftRepository
    {
        private readonly IDbConnection Connection = null;
        private readonly ILogFacility<GiftRepo> Log = null;

        public GiftRepo(IDbConnection conn, ILogFacility<GiftRepo> log)
        {
            Connection = conn;
            Log = log;
        }

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
