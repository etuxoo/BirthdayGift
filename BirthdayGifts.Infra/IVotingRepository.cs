using System.Collections.Generic;
using BirthdayGifts.Infra.Record;

namespace BirthdayGifts.Infra
{
    public interface IVotingRepository
    {
        public int Create(IEnumerable<VotingRecord> records);

        public IEnumerable<VotingRecord> Read(IEnumerable<int> recordIds);

        public IEnumerable<VotingRecord> Read(IEnumerable<string> recordDates);

        public int Update(IEnumerable<VotingRecord> records);

        public int Delete(IEnumerable<int> recordIds);
    }
}
