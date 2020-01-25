using System.Collections.Generic;
using BirthdayGifts.Infra.Record;

namespace BirthdayGifts.Infra
{
    public interface IVotingRepository
    {
        public IEnumerable<VotingRecord> Create(IEnumerable<VotingRecord> records);

        public IEnumerable<VotingRecord> Read(IEnumerable<VotingRecord> records);

        public IEnumerable<VotingRecord> Update(IEnumerable<VotingRecord> records);

        public IEnumerable<VotingRecord> Delete(IEnumerable<VotingRecord> records);
    }
}
