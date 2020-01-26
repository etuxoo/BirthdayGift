using BirthdayGifts.Infra.Record;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BirthdayGifts.Infra.Repository
{
    public class VotingRepo : IVotingRepository
    {
        private readonly IDbConnection Connection = null;
        private readonly ILogFacility<VotingRepo> Log = null;

        public VotingRepo(IDbConnection conn, ILogFacility<VotingRepo> log)
        {
            Connection = conn;
            Log = log;
        }

        private void Validate(IEnumerable<VotingRecord> records, bool isUpdate = false)
        {
            var allUnique = records.GroupBy(x => x.Date).All(g => g.Count() == 1);

            if (!allUnique)
            {
                Log.Error($"There are duplicate records in the passed collection.");
                throw new ArgumentException($"There are duplicate records in the passed collection.");
            }

            foreach (var record in records)
            {
                if (record.Date == null)
                {
                    Log.Error($"{nameof(VotingRecord)}.{nameof(VotingRecord.Date)} can't be null.");
                    throw new ArgumentException($"{nameof(VotingRecord)}.{nameof(VotingRecord.Date)} can't be null.");
                }
                if (record.IsOpen == null)
                {
                    Log.Error($"{nameof(VotingRecord)}.{nameof(VotingRecord.IsOpen)} can't be null.");
                    throw new ArgumentException($"{nameof(VotingRecord)}.{nameof(VotingRecord.IsOpen)} can't be null.");
                }
                if (record.StartedBy == null)
                {
                    Log.Error($"{nameof(VotingRecord)}.{nameof(VotingRecord.StartedBy)} can't be null.");
                    throw new ArgumentException($"{nameof(VotingRecord)}.{nameof(VotingRecord.StartedBy)} can't be null.");
                }
                if (record.UserId == null)
                {
                    Log.Error($"{nameof(VotingRecord)}.{nameof(VotingRecord.UserId)} can't be null.");
                    throw new ArgumentException($"{nameof(VotingRecord)}.{nameof(VotingRecord.UserId)} can't be null.");
                }
                if (isUpdate)
                {
                    if (record.Id == null)
                    {
                        Log.Error($"{nameof(VotingRecord)}.{nameof(VotingRecord.Id)} can't be null.");
                        throw new ArgumentException($"{nameof(VotingRecord)}.{nameof(VotingRecord.Id)} can't be null.");
                    }
                }
            }
        }

        public int Create(IEnumerable<VotingRecord> records)
        {
            Log.Trace($"{nameof(VotingRepo)}.{nameof(Create)} has been invoked.");

            Log.Trace("Preparing SQL statement...");


            var result = 0;
            if (records != null)
            {
                Validate(records);
                var sql = @$"INSERT INTO Votings (startedBy, isOpen, date, userID)
VALUES (@{nameof(VotingRecord.StartedBy)}, @{nameof(VotingRecord.IsOpen)}, @{nameof(VotingRecord.Date)}, @{nameof(VotingRecord.UserId)})
";

                Log.Trace("SQL statement prepared:");
                Log.Debug(sql);

                using var transaction = Connection.BeginTransaction();
                result = Connection.Execute(sql, records, transaction);
                transaction.Commit();
            }
            else
            {
                Log.Error($"The collection passed to {nameof(VotingRepo)}.{nameof(Create)} can't be null.");
                throw new ArgumentException($"The collection passed to {nameof(VotingRepo)}.{nameof(Create)} can't be null.");
            }

            Log.Trace($"Execution of {nameof(VotingRepo)}.{nameof(Create)} method completed.");

            return result;
        }

        public int Delete(IEnumerable<int> recordIds)
        {
            Log.Trace($"{nameof(VotingRepo)}.{nameof(Delete)} has been invoked.");

            Log.Trace("Preparing SQL statement...");

            var result = 0;

            if (recordIds != null)
            {

                var sql = "DELETE FROM Votings";

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
                Log.Error($"The collection passed to {nameof(VotingRepo)}.{nameof(Delete)} can't be null.");
                throw new ArgumentException($"The collection passed to {nameof(VotingRepo)}.{nameof(Delete)} can't be null.");
            }

            Log.Trace($"Execution of {nameof(VotingRepo)}.{nameof(Delete)} method completed.");

            return result;
        }

        public IEnumerable<VotingRecord> Read(IEnumerable<int> recordIds = null)
        {
            Log.Trace($"{nameof(VotingRepo)}.{nameof(Read)} has been invoked.");

            Log.Trace("Preparing SQL statement...");

            var sql = @"SELECT *
FROM Users
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

            var result = Connection.Query<VotingRecord>(sql);

            Log.Debug($"{recordIds?.Count() ?? 0} user records retrieved.");

            Log.Trace($"{nameof(VotingRepo)}.{nameof(Read)} execution completed.");

            return result;
        }


        // TODO
        public IEnumerable<VotingRecord> Read(IEnumerable<string> recordDate)
        {
            //            Log.Trace($"{nameof(VotingRepo)}.{nameof(Read)} has been invoked.");

            //            Log.Trace("Preparing SQL statement...");

            //            var sql = @"SELECT *
            //FROM Users
            //";

            //            if (recordDate?.Any() ?? false)
            //            {
            //                var dateList = recordDate.Aggregate(string.Empty, (text, name) => text += $"'{name}',")
            //                    .TrimEnd(',')
            //                    .Replace(",", ", ")
            //                ;

            //                sql += $"WHERE date IN({dateList})";

            //                Log.Trace("SQL statement prepared:");
            //                Log.Debug(sql);

            //                Log.Debug($"{nameof(VotingRepo)}.{nameof(Read)}(id = {recordDate.Count()}) query start. ");
            //            }

            //            var result = Connection.Query<VotingRecord>(sql);

            //            Log.Debug($"{recordDate?.Count() ?? 0} voting records retrieved.");

            //            Log.Trace($"{nameof(VotingRepo)}.{nameof(Read)} execution completed.");

            //            return result;
            throw new NotImplementedException();
        }

        public int Update(IEnumerable<VotingRecord> records)
        {
            Log.Trace($"{nameof(VotingRepo)}.{nameof(Update)} has been invoked.");

            if (!records?.Any() ?? true)
            {
                Log.Error($"No votings to update.");
                throw new ArgumentException("No votings to update.");
            }

            Validate(records, true);

            Log.Trace("Preparing SQL statement...");

            var sql = $@"UPDATE Users
SET  startedBy = @{nameof(VotingRecord.StartedBy)} , isOpen = @{nameof(VotingRecord.IsOpen)}, date = @{nameof(VotingRecord.Date)}, userID = @{nameof(VotingRecord.UserId)}
WHERE id = @{nameof(VotingRecord.Id)}";

            Log.Trace("SQL statement prepared:");
            Log.Debug(sql);

            var result = Connection.Execute(sql, records);

            Log.Debug($"{records} user records updated.");

            Log.Trace($"{nameof(VotingRepo)}.{nameof(Update)} execution completed.");

            return result;
        }
    }
}
