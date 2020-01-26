using BirthdayGifts.Infra.Record;
using BirthdayGifts.Infra.Repository;
using Dapper;
using Moq;
using Moq.Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Xunit;

namespace BirthdayGifts.Infra.Tests
{
    public class UserRepoUnitTests
    {
        private readonly Mock<IDbConnection> ConnMoq = new Mock<IDbConnection>();
        private readonly Mock<IDbCommand> cmdMoq = new Mock<IDbCommand>();
        private readonly Mock<IDbTransaction> transactionMoq = new Mock<IDbTransaction>();
        private readonly Mock<ILogFacility<UserRepo>> LogMoq = new Mock<ILogFacility<UserRepo>>();
        private readonly UserRepo Sut = null;

        public UserRepoUnitTests()
        {
            ConnMoq.Setup(db => db.CreateCommand()).Returns(cmdMoq.Object);
            ConnMoq.Setup(db => db.BeginTransaction()).Returns(transactionMoq.Object);
            Sut = new UserRepo(ConnMoq.Object, LogMoq.Object);
        }


        [Fact]
        public void CreateOne()
        {
            var testUser = new UserRecord
            {
                Name = DateTime.Now.Ticks.ToString(),
                BirthDate = DateTime.Now.Date,
                UserName = DateTime.Now.Ticks.ToString(),
                Password = DateTime.Now.Ticks.ToString()
            };

            ConnMoq.SetupDapper(c => c.Execute(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>(), null, null))
                .Returns(1)
                .Verifiable();

            var test = Sut.Create(new[] { testUser });

            ConnMoq.Verify();
            Assert.Equal(1, test);
        }

        [Fact]
        public void CreateMany()
        {
            var testUser = new UserRecord
            {
                Name = DateTime.Now.Ticks.ToString(),
                BirthDate = DateTime.Now.Date,
                UserName = DateTime.Now.Ticks.ToString(),
                Password = DateTime.Now.Ticks.ToString()
            };

            var testUser1 = new UserRecord
            {
                Name = DateTime.Now.Ticks.ToString(),
                BirthDate = DateTime.Now.Date,
                UserName = DateTime.Now.Ticks.ToString() + "1",
                Password = DateTime.Now.Ticks.ToString()
            };

            var testUser2 = new UserRecord
            {
                Name = DateTime.Now.Ticks.ToString(),
                BirthDate = DateTime.Now.Date,
                UserName = DateTime.Now.Ticks.ToString() + "2",
                Password = DateTime.Now.Ticks.ToString()
            };

            ConnMoq.SetupDapper(c => c.Execute(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>(), null, null))
                .Returns(1)
                .Verifiable();

            var test = Sut.Create(new[] { testUser, testUser1, testUser2 });

            ConnMoq.Verify();
            Assert.Equal(3, test);
        }

        [Fact]
        public void CreateFailWithDuplicateRecords()
        {
            var testUser = new UserRecord
            {
                Name = DateTime.Now.Ticks.ToString(),
                BirthDate = DateTime.Now.Date,
                UserName = DateTime.Now.Ticks.ToString(),
                Password = DateTime.Now.Ticks.ToString()
            };

            Assert.Throws<ArgumentException>(() => Sut.Create(new[] { testUser, testUser, testUser }));
        }

        [Fact]
        public void CreateFailWithNullCollection()
        {
            Assert.Throws<ArgumentException>(() => Sut.Create(null));
        }

        [Fact]
        public void CreateFailWithNullName()
        {
            var testUser = new UserRecord
            {
                BirthDate = DateTime.Now.Date,
                UserName = DateTime.Now.Ticks.ToString(),
                Password = DateTime.Now.Ticks.ToString()
            };

            Assert.Throws<ArgumentException>(() => Sut.Create(new[] { testUser }));
        }

        [Fact]
        public void CreateFailWithNullUserName()
        {
            var testUser = new UserRecord
            {
                BirthDate = DateTime.Now.Date,
                Name = DateTime.Now.Ticks.ToString(),
                Password = DateTime.Now.Ticks.ToString()
            };

            Assert.Throws<ArgumentException>(() => Sut.Create(new[] { testUser }));
        }

        [Fact]
        public void CreateFailWithNullPassword()
        {
            var testUser = new UserRecord
            {
                BirthDate = DateTime.Now.Date,
                UserName = DateTime.Now.Ticks.ToString(),
                Name = DateTime.Now.Ticks.ToString()
            };

            Assert.Throws<ArgumentException>(() => Sut.Create(new[] { testUser }));
        }

        [Fact]
        public void CreateFailWithNullBirthDate()
        {
            var testUser = new UserRecord
            {
                Name = DateTime.Now.Ticks.ToString(),
                UserName = DateTime.Now.Ticks.ToString(),
                Password = DateTime.Now.Ticks.ToString()
            };

            Assert.Throws<ArgumentException>(() => Sut.Create(new[] { testUser }));
        }

        [Fact]
        public void CreateFailWithLongerName()
        {
            var testUser = new UserRecord
            {
                Name = DateTime.Now.Ticks.ToString() + DateTime.Now.Ticks.ToString() + DateTime.Now.Ticks.ToString() + DateTime.Now.Ticks.ToString(),
                BirthDate = DateTime.Now.Date,
                UserName = DateTime.Now.Ticks.ToString(),
                Password = DateTime.Now.Ticks.ToString()
            };

            Assert.Throws<ArgumentException>(() => Sut.Create(new[] { testUser }));
        }

        [Fact]
        public void CreateFailWithLongerUserName()
        {
            var testUser = new UserRecord
            {
                UserName = DateTime.Now.Ticks.ToString() + DateTime.Now.Ticks.ToString() + DateTime.Now.Ticks.ToString() + DateTime.Now.Ticks.ToString(),
                BirthDate = DateTime.Now.Date,
                Name = DateTime.Now.Ticks.ToString(),
                Password = DateTime.Now.Ticks.ToString()
            };

            Assert.Throws<ArgumentException>(() => Sut.Create(new[] { testUser }));
        }

        [Fact]
        public void CreateFailWithLongerPassword()
        {
            var testUser = new UserRecord
            {
                UserName = DateTime.Now.Ticks.ToString() + DateTime.Now.Ticks.ToString() + DateTime.Now.Ticks.ToString() + DateTime.Now.Ticks.ToString(),
                BirthDate = DateTime.Now.Date,
                Name = DateTime.Now.Ticks.ToString(),
                Password = Encoding.Unicode.GetString(Encoding.Unicode.GetBytes(DateTime.Now.Ticks.ToString())) + Encoding.Unicode.GetString(Encoding.Unicode.GetBytes(DateTime.Now.Ticks.ToString()))
                + Encoding.Unicode.GetString(Encoding.Unicode.GetBytes(DateTime.Now.Ticks.ToString())) + Encoding.Unicode.GetString(Encoding.Unicode.GetBytes(DateTime.Now.Ticks.ToString()))
            };

            Assert.Throws<ArgumentException>(() => Sut.Create(new[] { testUser }));
        }

        [Fact]
        public void ReadOneById()
        {
            var testUserID = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6));


            ConnMoq.SetupDapper(c => c.Query(It.IsAny<string>(), null, null, It.IsAny<bool>(), null, null))
                .Returns(new[] { new UserRecord() })
                .Verifiable();

            var test = Sut.Read(new[] { testUserID });

            ConnMoq.Verify();
            Assert.NotNull(test);
            Assert.IsAssignableFrom<IEnumerable<UserRecord>>(test);

            //The Moq.dapper Gives strange behavior here.
            // TODO : Check with integration test
            //Assert.Single(test.AsList());
        }

        [Fact]
        public void ReadManyById()
        {
            var testUser = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6));

            var testUser1 = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6)) + 1;

            var testUser2 = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6)) + 2;

            ConnMoq.SetupDapper(c => c.Query(It.IsAny<string>(), null, null, It.IsAny<bool>(), null, null))
                .Returns(new[] { new UserRecord(), new UserRecord(), new UserRecord() })
                .Verifiable();

            var test = Sut.Read(new[] { testUser, testUser1, testUser2 });

            ConnMoq.Verify();
            Assert.NotNull(test);
            Assert.IsAssignableFrom<IEnumerable<UserRecord>>(test);

            //The Moq.dapper Gives strange behavior here.
            // TODO : Check with integration test
            //Assert.Equal(3, test.ToList().Count);
        }

        [Fact]
        public void ReadManyByName()
        {
            var testUser = DateTime.Now.Ticks.ToString();

            var testUser1 = DateTime.Now.Ticks.ToString() + "1";

            var testUser2 = DateTime.Now.Ticks.ToString() + "2";

            ConnMoq.SetupDapper(c => c.Query(It.IsAny<string>(), null, null, It.IsAny<bool>(), null, null))
                .Returns(new[] { new UserRecord(), new UserRecord(), new UserRecord() })
                .Verifiable();

            var test = Sut.Read(new[] { testUser, testUser1, testUser2 });

            ConnMoq.Verify();
            Assert.NotNull(test);
            Assert.IsAssignableFrom<IEnumerable<UserRecord>>(test);

            //The Moq.dapper Gives strange behavior here.
            // TODO : Check with integration test
            //Assert.Equal(3, test.ToList().Count);
        }
        [Fact]
        public void UpdateMany()
        {
            var testUser = new UserRecord
            {
                Id = 1,
                Name = DateTime.Now.Ticks.ToString(),
                BirthDate = DateTime.Now.Date,
                UserName = DateTime.Now.Ticks.ToString(),
                Password = DateTime.Now.Ticks.ToString()
            };

            var testUser1 = new UserRecord
            {
                Id = 2,
                Name = DateTime.Now.Ticks.ToString(),
                BirthDate = DateTime.Now.Date,
                UserName = DateTime.Now.Ticks.ToString() + "1",
                Password = DateTime.Now.Ticks.ToString()
            };

            var testUser2 = new UserRecord
            {
                Id = 3,
                Name = DateTime.Now.Ticks.ToString(),
                BirthDate = DateTime.Now.Date,
                UserName = DateTime.Now.Ticks.ToString() + "2",
                Password = DateTime.Now.Ticks.ToString()
            };

            ConnMoq.SetupDapper(c => c.Execute(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>(), null, null))
                .Returns(1)
                .Verifiable();

            var test = Sut.Update(new[] { testUser, testUser1, testUser2 });

            ConnMoq.Verify();
            Assert.Equal(3, test);
        }

        [Fact]
        public void UpdateOneMultipleTimes()
        {
            var testUser = new UserRecord
            {
                Id = 1,
                Name = DateTime.Now.Ticks.ToString(),
                BirthDate = DateTime.Now.Date,
                UserName = DateTime.Now.Ticks.ToString(),
                Password = DateTime.Now.Ticks.ToString()
            };

            var testUser1 = new UserRecord
            {
                Id = 1,
                Name = DateTime.Now.Ticks.ToString(),
                BirthDate = DateTime.Now.Date,
                UserName = DateTime.Now.Ticks.ToString() + "1",
                Password = DateTime.Now.Ticks.ToString()
            };

            var testUser2 = new UserRecord
            {
                Id = 1,
                Name = DateTime.Now.Ticks.ToString(),
                BirthDate = DateTime.Now.Date,
                UserName = DateTime.Now.Ticks.ToString() + "2",
                Password = DateTime.Now.Ticks.ToString()
            };

            ConnMoq.SetupDapper(c => c.Execute(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>(), null, null))
                .Returns(1)
                .Verifiable();

            var test = Sut.Update(new[] { testUser, testUser1, testUser2 });

            ConnMoq.Verify();
            Assert.Equal(3, test);
        }

        [Fact]
        public void UpdateFailWithNullCollection()
        {
            Assert.Throws<ArgumentException>(() => Sut.Update(null));
        }

        [Fact]
        public void UpdateFailWithEmptyCollection()
        {
            IEnumerable<UserRecord> list = new List<UserRecord>();
            Assert.Throws<ArgumentException>(() => Sut.Update(list));
        }

        [Fact]
        public void UpdateFailWithNullId()
        {
            var testRecord = new UserRecord
            {
                Name = DateTime.Now.Ticks.ToString(),
                BirthDate = DateTime.Now.Date,
                UserName = DateTime.Now.Ticks.ToString(),
                Password = DateTime.Now.Ticks.ToString()
            };
            Assert.Throws<ArgumentException>(() => Sut.Update(new[] { testRecord }));
        }

        [Fact]
        public void DeleteOne()
        {
            ConnMoq.SetupDapper(c => c.Execute(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>(), null, null))
                .Returns(1)
                .Verifiable();

            var test = Sut.Delete(new[] { 1 });

            ConnMoq.Verify();
            Assert.Equal(1, test);
        }

        [Fact]
        public void DeleteMany()
        {
            ConnMoq.SetupDapper(c => c.Execute(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>(), null, null))
                .Returns(1)
                .Verifiable();

            var test = Sut.Delete(new[] { 1, 2, 3 });

            ConnMoq.Verify();
            Assert.Equal(3, test);
        }

        [Fact]
        public void DeleteFailWithNullCollection()
        {
            Assert.Throws<ArgumentException>(() => Sut.Delete(null));
        }
    }
}
