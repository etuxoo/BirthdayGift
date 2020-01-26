using BirthdayGifts.Infra.Record;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BirthdayGifts.Infra.Repository
{
    public class GiftRepo : IGiftRepository
    {
        private readonly IDbConnection Connection = null;
        private readonly ILogFacility<GiftRepo> Log = null;

        public GiftRepo(IDbConnection conn, ILogFacility<GiftRepo> log)
        {

            Connection = conn;
            Log = log;
        }

        private void Validate(IEnumerable<GiftRecord> records, bool isUpdate = false)
        {
            var allUnique = records.GroupBy(x => x.Name).All(g => g.Count() == 1);

            if (!allUnique)
            {
                Log.Error($"There are duplicate records in the passed collection.");
                throw new ArgumentException($"There are duplicate records in the passed collection.");
            }

            foreach (var record in records)
            {
                if (record.Name == null)
                {
                    Log.Error($"{nameof(GiftRecord)}.{nameof(GiftRecord.Name)} can't be null.");
                    throw new ArgumentException($"{nameof(GiftRecord)}.{nameof(GiftRecord.Name)} can't be null.");
                }
                if (record.Name.Length > 32)
                {
                    Log.Error($"{nameof(GiftRecord)}.{nameof(GiftRecord.Name)} should have 32 lenght or less.");
                    throw new ArgumentException($"{nameof(GiftRecord)}.{nameof(GiftRecord.Name)} should have 32 lenght or less.");
                }
                if (isUpdate)
                {
                    if (record.Id == null)
                    {
                        Log.Error($"{nameof(GiftRecord)}.{nameof(GiftRecord.Id)} can't be null.");
                        throw new ArgumentException($"{nameof(GiftRecord)}.{nameof(GiftRecord.Id)} can't be null.");
                    }
                }
            }
        }

        public int Create(IEnumerable<GiftRecord> records)
        {

            Log.Trace($"{nameof(GiftRepo)}.{nameof(Create)} has been invoked.");

            Log.Trace("Preparing SQL statement...");


            var result = 0;
            if (records != null)
            {
                Validate(records);

                var sql = @$"INSERT INTO Gifts (name)
VALUES (@{nameof(GiftRecord.Name)})
";

                Log.Trace("SQL statement prepared:");
                Log.Debug(sql);

                using var transaction = Connection.BeginTransaction();
                result = Connection.Execute(sql, records, transaction);
                transaction.Commit();
            }
            else
            {
                Log.Error($"The collection passed to {nameof(GiftRepo)}.{nameof(Create)} can't be null.");
                throw new ArgumentException($"The collection passed to {nameof(GiftRepo)}.{nameof(Create)} can't be null.");
            }

            Log.Trace($"Execution of {nameof(GiftRepo)}.{nameof(Create)} method completed.");

            return result;
        }

        public IEnumerable<GiftRecord> Read(IEnumerable<int> recordIds = null)
        {
            Log.Trace($"{nameof(GiftRepo)}.{nameof(Read)} has been invoked.");

            Log.Trace("Preparing SQL statement...");

            var sql = @"SELECT id, name
FROM Gifts
";

            if (recordIds?.Any() ?? false)
            {
                var idList = recordIds.Aggregate(string.Empty, (text, id) => text += $"'{id}',")
                    .TrimEnd(',')
                    .Replace(",", ", ")
                ;

                sql += $"WHERE id IN({idList})";

                Log.Debug($"{nameof(GiftRepo)}.{nameof(Read)}(id = {recordIds.Count()}) query start. ");
            }

            Log.Trace("SQL statement prepared:");
            Log.Debug(sql);

            var result = Connection.Query<GiftRecord>(sql);

            Log.Debug($"{recordIds?.Count() ?? 0} gift records retrieved.");

            Log.Trace($"{nameof(GiftRepo)}.{nameof(Read)} execution completed.");

            return result;
        }

        public IEnumerable<GiftRecord> Read(IEnumerable<string> recordNames)
        {
            Log.Trace($"{nameof(GiftRepo)}.{nameof(Read)} has been invoked.");

            Log.Trace("Preparing SQL statement...");

            var sql = @"SELECT id, name
FROM Gifts
";

            if (recordNames?.Any() ?? false)
            {
                var nameList = recordNames.Aggregate(string.Empty, (text, name) => text += $"'{name}',")
                    .TrimEnd(',')
                    .Replace(",", ", ")
                ;

                sql += $"WHERE name IN({nameList})";

                Log.Trace("SQL statement prepared:");
                Log.Debug(sql);

                Log.Debug($"{nameof(GiftRepo)}.{nameof(Read)}(id = {recordNames.Count()}) query start. ");
            }

            var result = Connection.Query<GiftRecord>(sql);

            Log.Debug($"{recordNames?.Count() ?? 0} gift records retrieved.");

            Log.Trace($"{nameof(GiftRepo)}.{nameof(Read)} execution completed.");

            return result;
        }

        public int Update(IEnumerable<GiftRecord> records)
        {
            Log.Trace($"{nameof(GiftRepo)}.{nameof(Update)} has been invoked.");


            if (!records?.Any() ?? true)
            {
                Log.Error($"No gifts to update.");
                throw new ArgumentException("No gifts to update.");
            }

            Validate(records, true);

            Log.Trace("Preparing SQL statement...");

            var sql = $@"UPDATE Gifts
SET  name = @{nameof(GiftRecord.Name)}
WHERE id = @{nameof(GiftRecord.Id)}";

            Log.Trace("SQL statement prepared:");
            Log.Debug(sql);

            var result = Connection.Execute(sql, records);

            Log.Debug($"{records} gift records updated.");

            Log.Trace($"{nameof(GiftRepo)}.{nameof(Update)} execution completed.");

            return result;
        }

        public int Delete(IEnumerable<int> recordIds)
        {
            Log.Trace($"{nameof(GiftRepo)}.{nameof(Delete)} has been invoked.");

            Log.Trace("Preparing SQL statement...");

            var result = 0;

            if (recordIds != null)
            {

                var sql = "DELETE FROM Gifts";

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
                Log.Error($"The collection passed to {nameof(GiftRepo)}.{nameof(Delete)} can't be null.");
                throw new ArgumentException($"The collection passed to {nameof(GiftRepo)}.{nameof(Delete)} can't be null.");
            }

            Log.Trace($"Execution of {nameof(GiftRepo)}.{nameof(Delete)} method completed.");

            return result;
        }
    }
}
