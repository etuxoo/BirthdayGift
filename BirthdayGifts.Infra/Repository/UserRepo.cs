using BirthdayGifts.Infra.Record;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BirthdayGifts.Infra.Repository
{
    public class UserRepo : IUserRepository
    {
        private readonly IDbConnection Connection = null;
        private readonly ILogFacility<UserRepo> Log = null;

        public UserRepo(IDbConnection conn, ILogFacility<UserRepo> log)
        {
            Connection = conn;
            Log = log;
        }

        private void Validate(IEnumerable<UserRecord> records, bool isUpdate = false)
        {
            var allUnique = records.GroupBy(x => x.UserName).All(g => g.Count() == 1);

            if (!allUnique)
            {
                Log.Error($"There are duplicate records in the passed collection.");
                throw new ArgumentException($"There are duplicate records in the passed collection.");
            }

            foreach (var record in records)
            {
                if (record.Name == null)
                {
                    Log.Error($"{nameof(UserRecord)}.{nameof(UserRecord.Name)} can't be null.");
                    throw new ArgumentException($"{nameof(UserRecord)}.{nameof(UserRecord.Name)} can't be null.");
                }
                if (record.Name.Length > 32)
                {
                    Log.Error($"{nameof(UserRecord)}.{nameof(UserRecord.Name)} should have 32 lenght or less.");
                    throw new ArgumentException($"{nameof(UserRecord)}.{nameof(UserRecord.Name)} should have 32 lenght or less.");
                }
                if (record.UserName == null)
                {
                    Log.Error($"{nameof(UserRecord)}.{nameof(UserRecord.UserName)} can't be null.");
                    throw new ArgumentException($"{nameof(UserRecord)}.{nameof(UserRecord.UserName)} can't be null.");
                }
                if (record.UserName.Length > 32)
                {
                    Log.Error($"{nameof(UserRecord)}.{nameof(UserRecord.UserName)} should have 32 lenght or less.");
                    throw new ArgumentException($"{nameof(UserRecord)}.{nameof(UserRecord.UserName)} should have 32 lenght or less.");
                }
                if (record.Password == null)
                {
                    Log.Error($"{nameof(UserRecord)}.{nameof(UserRecord.Password)} can't be null.");
                    throw new ArgumentException($"{nameof(UserRecord)}.{nameof(UserRecord.Password)} can't be null.");
                }
                if (record.Password.Length > 256)
                {
                    Log.Error($"{nameof(UserRecord)}.{nameof(UserRecord.Password)} should have 32 lenght or less.");
                    throw new ArgumentException($"{nameof(UserRecord)}.{nameof(UserRecord.Password)} should have 32 lenght or less.");
                }
                if (record.BirthDate == null)
                {
                    Log.Error($"{nameof(UserRecord)}.{nameof(UserRecord.BirthDate)} can't be null.");
                    throw new ArgumentException($"{nameof(UserRecord)}.{nameof(UserRecord.BirthDate)} can't be null.");
                }
                if (isUpdate)
                {
                    if (record.Id == null)
                    {
                        Log.Error($"{nameof(UserRecord)}.{nameof(UserRecord.Id)} can't be null.");
                        throw new ArgumentException($"{nameof(UserRecord)}.{nameof(UserRecord.Id)} can't be null.");
                    }
                }
            }
        }

        public int Create(IEnumerable<UserRecord> records)
        {
            Log.Trace($"{nameof(UserRepo)}.{nameof(Create)} has been invoked.");

            Log.Trace("Preparing SQL statement...");


            var result = 0;
            if (records != null)
            {
                Validate(records);
                var sql = @$"INSERT INTO Users (name, username, password, birthdate)
VALUES (@{nameof(UserRecord.Name)}, @{nameof(UserRecord.UserName)}, @{nameof(UserRecord.Password)}, @{nameof(UserRecord.BirthDate)})
";

                Log.Trace("SQL statement prepared:");
                Log.Debug(sql);

                using var transaction = Connection.BeginTransaction();
                result = Connection.Execute(sql, records, transaction);
                transaction.Commit();
            }
            else
            {
                Log.Error($"The collection passed to {nameof(UserRepo)}.{nameof(Create)} can't be null.");
                throw new ArgumentException($"The collection passed to {nameof(UserRepo)}.{nameof(Create)} can't be null.");
            }

            Log.Trace($"Execution of {nameof(UserRepo)}.{nameof(Create)} method completed.");

            return result;
        }

        public IEnumerable<UserRecord> Read(IEnumerable<int> recordIds = null)
        {
            Log.Trace($"{nameof(UserRepo)}.{nameof(Read)} has been invoked.");

            Log.Trace("Preparing SQL statement...");

            var sql = @"SELECT id, name, username, password, birthdate
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

                Log.Debug($"{nameof(UserRepo)}.{nameof(Read)}(id = {recordIds.Count()}) query start. ");
            }

            var result = Connection.Query<UserRecord>(sql);

            Log.Debug($"{recordIds?.Count() ?? 0} user records retrieved.");

            Log.Trace($"{nameof(UserRepo)}.{nameof(Read)} execution completed.");

            return result;
        }

        public IEnumerable<UserRecord> Read(IEnumerable<string> recordNames)
        {
            Log.Trace($"{nameof(UserRepo)}.{nameof(Read)} has been invoked.");

            Log.Trace("Preparing SQL statement...");

            var sql = @"SELECT id, name, username, password, birthdate
FROM Users
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

                Log.Debug($"{nameof(UserRepo)}.{nameof(Read)}(id = {recordNames.Count()}) query start. ");
            }

            var result = Connection.Query<UserRecord>(sql);

            Log.Debug($"{recordNames?.Count() ?? 0} user records retrieved.");

            Log.Trace($"{nameof(UserRepo)}.{nameof(Read)} execution completed.");

            return result;
        }

        public int Update(IEnumerable<UserRecord> records)
        {
            Log.Trace($"{nameof(UserRepo)}.{nameof(Update)} has been invoked.");


            if (!records?.Any() ?? true)
            {
                Log.Error($"No users to update.");
                throw new ArgumentException("No users to update.");
            }

            Validate(records, true);

            Log.Trace("Preparing SQL statement...");

            var sql = $@"UPDATE Users
SET  name = @{nameof(UserRecord.Name)} , username = @{nameof(UserRecord.UserName)}, password = @{nameof(UserRecord.Password)}, birthdate = @{nameof(UserRecord.BirthDate)}
WHERE id = @{nameof(UserRecord.Id)}";

            Log.Trace("SQL statement prepared:");
            Log.Debug(sql);

            var result = Connection.Execute(sql, records);

            Log.Debug($"{records} user records updated.");

            Log.Trace($"{nameof(UserRepo)}.{nameof(Update)} execution completed.");

            return result;
        }

        public int Delete(IEnumerable<int> recordIds)
        {
            Log.Trace($"{nameof(UserRepo)}.{nameof(Delete)} has been invoked.");

            Log.Trace("Preparing SQL statement...");

            var result = 0;

            if (recordIds != null)
            {

                var sql = "DELETE FROM Users";

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
                Log.Error($"The collection passed to {nameof(UserRepo)}.{nameof(Delete)} can't be null.");
                throw new ArgumentException($"The collection passed to {nameof(UserRepo)}.{nameof(Delete)} can't be null.");
            }

            Log.Trace($"Execution of {nameof(UserRepo)}.{nameof(Delete)} method completed.");

            return result;
        }
    }
}
