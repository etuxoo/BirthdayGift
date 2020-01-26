using BirthdayGifts.Infra.Record;
using System.Collections.Generic;

namespace BirthdayGifts.Infra
{
    interface IVoteRepository
    {
        public int Create(IEnumerable<VoteRecord> records);

        public IEnumerable<VoteRecord> Read(IEnumerable<int> recordIds);

        public IEnumerable<VoteRecord> Read(IEnumerable<string> recordNames);

        public int Update(IEnumerable<VoteRecord> records);

        public int Delete(IEnumerable<int> recordIds);
    }
}
