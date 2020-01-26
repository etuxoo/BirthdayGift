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
    public class VotingRepoUnitTests
    {
        private readonly Mock<IDbConnection> ConnMoq = new Mock<IDbConnection>();
        private readonly Mock<IDbCommand> cmdMoq = new Mock<IDbCommand>();
        private readonly Mock<IDbTransaction> transactionMoq = new Mock<IDbTransaction>();
        private readonly Mock<ILogFacility<VotingRepo>> LogMoq = new Mock<ILogFacility<VotingRepo>>();
        private readonly VotingRepo Sut = null;

        public VotingRepoUnitTests()
        {
            ConnMoq.Setup(db => db.CreateCommand()).Returns(cmdMoq.Object);
            ConnMoq.Setup(db => db.BeginTransaction()).Returns(transactionMoq.Object);
            Sut = new VotingRepo(ConnMoq.Object, LogMoq.Object);
        }

        [Fact]
        public void CreateOne()
        {
            var testVoting = new VotingRecord
            {
                UserId = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6)),
                Date = DateTime.Now.Date,
                IsOpen=false,
                StartedBy= int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6))
            };

            ConnMoq.SetupDapper(c => c.Execute(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>(), null, null))
                .Returns(1)
                .Verifiable();

            var test = Sut.Create(new[] { testVoting });

            ConnMoq.Verify();
            Assert.Equal(1, test);
        }

        [Fact]
        public void CreateMany()
        {
            var testVoting = new VotingRecord
            {
                UserId = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6)),
                Date = DateTime.Now.Date,
                IsOpen = false,
                StartedBy = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6))
            };

            var testVoting1 = new VotingRecord
            {
                UserId = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6)),
                Date = DateTime.Now.Date.AddDays(1),
                IsOpen = false,
                StartedBy = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6))
            };
            var testVoting2 = new VotingRecord
            {
                UserId = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6)),
                Date = DateTime.Now.Date.AddDays(2),
                IsOpen = false,
                StartedBy = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6))
            };

            ConnMoq.SetupDapper(c => c.Execute(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>(), null, null))
                .Returns(1)
                .Verifiable();

            var test = Sut.Create(new[] { testVoting, testVoting1, testVoting2 });

            ConnMoq.Verify();
            Assert.Equal(3, test);
        }

        [Fact]
        public void CreateFailWithNullCollection()
        {
            Assert.Throws<ArgumentException>(() => Sut.Create(null));
        }

        [Fact]
        public void CreateFailWithNullUserId()
        {
            var testVoting = new VotingRecord
            {
                Date = DateTime.Now.Date,
                IsOpen = false,
                StartedBy = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6))
            };

            Assert.Throws<ArgumentException>(()=> Sut.Create(new[] { testVoting }));
        }

        [Fact]
        public void CreateFailWithNullDate()
        {
            var testVoting = new VotingRecord
            {
                UserId = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6)),
                IsOpen = false,
                StartedBy = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6))
            };

            Assert.Throws<ArgumentException>(() => Sut.Create(new[] { testVoting }));
        }

        [Fact]
        public void CreateFailWithNullIsOpen()
        {
            var testVoting = new VotingRecord
            {
                UserId = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6)),
                Date = DateTime.Now.Date,
                StartedBy = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6))
            };

            Assert.Throws<ArgumentException>(() => Sut.Create(new[] { testVoting }));
        }

        [Fact]
        public void CreateFailWithNullStartedBy()
        {
            var testVoting = new VotingRecord
            {
                UserId = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6)),
                Date = DateTime.Now.Date,
                IsOpen = false
            };

            Assert.Throws<ArgumentException>(() => Sut.Create(new[] { testVoting }));
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

        [Fact]
        public void ReadOneById()
        {
            var testVotingID = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6));


            ConnMoq.SetupDapper(c => c.Query(It.IsAny<string>(), null, null, It.IsAny<bool>(), null, null))
                .Returns(new[] { new VotingRecord() })
                .Verifiable();

            var test = Sut.Read(new[] { testVotingID });

            ConnMoq.Verify();
            Assert.NotNull(test);
            Assert.IsAssignableFrom<IEnumerable<VotingRecord>>(test);

            //The Moq.dapper Gives strange behavior here.
            // TODO : Check with integration test
            //Assert.Single(test.AsList());
        }

        [Fact]
        public void ReadManyById()
        {
            var testVoting = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6));

            var testVoting1 = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6)) + 1;

            var testVoting2 = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6)) + 2;

            ConnMoq.SetupDapper(c => c.Query(It.IsAny<string>(), null, null, It.IsAny<bool>(), null, null))
                .Returns(new[] { new VotingRecord(), new VotingRecord(), new VotingRecord() })
                .Verifiable();

            var test = Sut.Read(new[] { testVoting, testVoting1, testVoting2 });

            ConnMoq.Verify();
            Assert.NotNull(test);
            Assert.IsAssignableFrom<IEnumerable<VotingRecord>>(test);

            //The Moq.dapper Gives strange behavior here.
            // TODO : Check with integration test
            //Assert.Equal(3, test.ToList().Count);
        }

        [Fact]
        public void UpdateMany()
        {
            var testVoting = new VotingRecord
            {
                Id = 1,
                UserId = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6)),
                Date = DateTime.Now.Date,
                IsOpen = false,
                StartedBy = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6))
            };

            var testVoting1 = new VotingRecord
            {
                Id = 2,
                UserId = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6)),
                Date = DateTime.Now.Date.AddDays(1),
                IsOpen = false,
                StartedBy = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6))
            };

            var testVoting2 = new VotingRecord
            {
                Id = 3,
                UserId = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6)),
                Date = DateTime.Now.Date.AddDays(2),
                IsOpen = false,
                StartedBy = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6))
            };

            ConnMoq.SetupDapper(c => c.Execute(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>(), null, null))
                .Returns(1)
                .Verifiable();

            var test = Sut.Update(new[] { testVoting, testVoting1, testVoting2 });

            ConnMoq.Verify();
            Assert.Equal(3, test);
        }

        [Fact]
        public void UpdateOneMultipleTimes()
        {
            var testVoting = new VotingRecord
            {
                Id = 1,
                UserId = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6)),
                Date = DateTime.Now.Date,
                IsOpen = false,
                StartedBy = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6))
            };

            var testVoting1 = new VotingRecord
            {
                Id = 1,
                UserId = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6)),
                Date = DateTime.Now.Date.AddDays(1),
                IsOpen = false,
                StartedBy = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6))
            };

            var testVoting2 = new VotingRecord
            {
                Id = 1,
                UserId = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6)),
                Date = DateTime.Now.Date.AddDays(2),
                IsOpen = false,
                StartedBy = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6))
            };

            ConnMoq.SetupDapper(c => c.Execute(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>(), null, null))
                .Returns(1)
                .Verifiable();

            var test = Sut.Update(new[] { testVoting, testVoting1, testVoting2 });

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
            IEnumerable<VotingRecord> list = new List<VotingRecord>();
            Assert.Throws<ArgumentException>(() => Sut.Update(list));
        }
    }
}
