using BirthdayGifts.Infra.Record;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        private void Validate(IEnumerable<VoteRecord> records, bool IsUpdate=false)
        {
            foreach (var record in records)
            {
                if (record.UserId==null)
                {
                    Log.Error($"{nameof(VoteRecord)}.{nameof(VoteRecord.UserId)} can't be null.");
                    throw new ArgumentException($"{nameof(VoteRecord)}.{nameof(VoteRecord.UserId)} can't be null.");
                }
                if (record.GiftId == null)
                {
                    Log.Error($"{nameof(VoteRecord)}.{nameof(VoteRecord.GiftId)} can't be null.");
                    throw new ArgumentException($"{nameof(VoteRecord)}.{nameof(VoteRecord.GiftId)} can't be null.");
                }
                if (record.VotingId == null)
                {
                    Log.Error($"{nameof(VoteRecord)}.{nameof(VoteRecord.VotingId)} can't be null.");
                    throw new ArgumentException($"{nameof(VoteRecord)}.{nameof(VoteRecord.VotingId)} can't be null.");
                }
                if (IsUpdate)
                {
                    if (record.Id==null)
                    {
                        Log.Error($"{nameof(VoteRecord)}.{nameof(VoteRecord.Id)} can't be null.");
                        throw new ArgumentException($"{nameof(VoteRecord)}.{nameof(VoteRecord.Id)} can't be null.");
                    }
                }
            }
        }

        public int Create(IEnumerable<VoteRecord> records)
        {
            Log.Trace($"{nameof(VoteRepo)}.{nameof(Create)} has been invoked.");

            Log.Trace("Preparing SQL statement...");


            var result = 0;
            if (records != null)
            {
                Validate(records);
                var sql = @$"INSERT INTO Votes (userID, giftID, votingID)
VALUES (@{nameof(VoteRecord.UserId)}, @{nameof(VoteRecord.GiftId)}, @{nameof(VoteRecord.VotingId)})
";

                Log.Trace("SQL statement prepared:");
                Log.Debug(sql);

                using var transaction = Connection.BeginTransaction();
                result = Connection.Execute(sql, records, transaction);
                transaction.Commit();
            }
            else
            {
                Log.Error($"The collection passed to {nameof(VoteRepo)}.{nameof(Create)} can't be null.");
                throw new ArgumentException($"The collection passed to {nameof(VoteRepo)}.{nameof(Create)} can't be null.");
            }

            Log.Trace($"Execution of {nameof(VoteRepo)}.{nameof(Create)} method completed.");

            return result;
        }

        public int Delete(IEnumerable<int> recordIds)
        {
            Log.Trace($"{nameof(VoteRepo)}.{nameof(Delete)} has been invoked.");

            Log.Trace("Preparing SQL statement...");

            var result = 0;

            if (recordIds != null)
            {

                var sql = "DELETE FROM Votes";

                var idList = recordIds.Aggregate(string.Empty, (text, id) => text += $"'{id}',")
                        .TrimEnd(',')
                        .Replace(",", ", ");

                sql += $" WHERE id IN({idList})";

                Log.Trace("SQL statement prepared:");
                Log.Debug(sql);

                using var transaction = Connection.BeginTransaction();
                result = Connection.Execute(sql, recordIds, transaction);
                transaction.Commit();
            }
            else
            {
                Log.Error($"The collection passed to {nameof(VoteRepo)}.{nameof(Delete)} can't be null.");
                throw new ArgumentException($"The collection passed to {nameof(VoteRepo)}.{nameof(Delete)} can't be null.");
            }

            Log.Trace($"Execution of {nameof(VoteRepo)}.{nameof(Delete)} method completed.");

            return result;
        }

        public IEnumerable<VoteRecord> Read(IEnumerable<int> recordIds=null)
        {
            Log.Trace($"{nameof(VoteRepo)}.{nameof(Read)} has been invoked.");

            Log.Trace("Preparing SQL statement...");

            var sql = @"SELECT *
FROM Votes
";

            if (recordIds?.Any() ?? false)
            {
                var idList = recordIds.Aggregate(string.Empty, (text, id) => text += $"'{id}',")
                    .TrimEnd(',')
                    .Replace(",", ", ")
                ;

                sql += $"WHERE id IN({idList})";

                Log.Trace("SQL statement prepared:");
                Log.Debug(sql);

                Log.Debug($"{nameof(VotingRepo)}.{nameof(Read)}(id = {recordIds.Count()}) query start. ");
            }

            var result = Connection.Query<VoteRecord>(sql);

            Log.Debug($"{recordIds?.Count() ?? 0} vote records retrieved.");

            Log.Trace($"{nameof(VoteRepo)}.{nameof(Read)} execution completed.");

            return result;
        }

        public int Update(IEnumerable<VoteRecord> records)
        {
            Log.Trace($"{nameof(VoteRepo)}.{nameof(Update)} has been invoked.");

            if (!records?.Any() ?? true)
            {
                Log.Error($"No votes to update.");
                throw new ArgumentException("No votes to update.");
            }

            Validate(records, true);

            Log.Trace("Preparing SQL statement...");

            var sql = $@"UPDATE Votes
SET  userID = @{nameof(VoteRecord.UserId)} , giftID = @{nameof(VoteRecord.GiftId)}, votingID = @{nameof(VoteRecord.VotingId)}
WHERE id = @{nameof(VoteRecord.Id)}";

            Log.Trace("SQL statement prepared:");
            Log.Debug(sql);

            var result = Connection.Execute(sql, records);

            Log.Debug($"{records} votes records updated.");

            Log.Trace($"{nameof(VoteRepo)}.{nameof(Update)} execution completed.");

            return result;
        }
    }
}
