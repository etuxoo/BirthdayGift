using BirthdayGifts.Infra.Record;
using System.Collections.Generic;

namespace BirthdayGifts.Infra
{
    interface IVoteRepository
    {
        public IEnumerable<VoteRecord> Create(IEnumerable<VoteRecord> records);

        public IEnumerable<VoteRecord> Read(IEnumerable<VoteRecord> records);

        public IEnumerable<VoteRecord> Update(IEnumerable<VoteRecord> records);

        public IEnumerable<VoteRecord> Delete(IEnumerable<VoteRecord> records);
    }
}
