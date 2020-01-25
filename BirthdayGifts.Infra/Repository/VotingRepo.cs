using BirthdayGifts.Infra.Record;
using System;
using System.Collections.Generic;
using System.Data;

namespace BirthdayGifts.Infra.Repository
{
    public class VotingRepo : IVoteRepository
    {
        private readonly IDbConnection Connection = null;
        private readonly ILogFacility<VotingRepo> Log = null;

        public VotingRepo(IDbConnection conn, ILogFacility<VotingRepo> log)
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
