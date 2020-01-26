using BirthdayGifts.Infra.Record;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BirthdayGifts.Infra.Repository
{
    public class VoteRepo : IVoteRepository
    {
        private readonly IDbConnection Connection = null;
        private readonly ILogFacility<VoteRepo> Log = null;

        public VoteRepo(IDbConnection conn, ILogFacility<VoteRepo> log)
        {
            Connection = conn;
            Log = log;
        }

        public int Create(IEnumerable<VoteRecord> records)
        {
            throw new NotImplementedException();
        }

        public int Delete(IEnumerable<int> recordIds)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<VoteRecord> Read(IEnumerable<int> recordIds)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<VoteRecord> Read(IEnumerable<string> recordNames)
        {
            throw new NotImplementedException();
        }

        public int Update(IEnumerable<VoteRecord> records)
        {
            throw new NotImplementedException();
        }
    }
}
