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

        public IEnumerable<VoteRecord> Create(IEnumerable<VoteRecord> records)
        {
            // TODO
            throw new NotImplementedException();
        }

        public IEnumerable<VoteRecord> Delete(IEnumerable<VoteRecord> records)
        {
            // TODO
            throw new NotImplementedException();
        }

        public IEnumerable<VoteRecord> Read(IEnumerable<VoteRecord> records)
        {
            // TODO
            throw new NotImplementedException();
        }

        public IEnumerable<VoteRecord> Update(IEnumerable<VoteRecord> records)
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}
