﻿using BirthdayGifts.Infra.Record;
using System;
using System.Collections.Generic;
using System.Text;

namespace BirthdayGifts.Infra.Repository
{
    public class UserRepo : IVoteRepository
    {
        // TODO : Add DI for the connection
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
