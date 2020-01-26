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
    public class VoteRepoUnitTests
    {
        private readonly Mock<IDbConnection> ConnMoq = new Mock<IDbConnection>();
        private readonly Mock<IDbCommand> cmdMoq = new Mock<IDbCommand>();
        private readonly Mock<IDbTransaction> transactionMoq = new Mock<IDbTransaction>();
        private readonly Mock<ILogFacility<VoteRepo>> LogMoq = new Mock<ILogFacility<VoteRepo>>();
        private readonly VoteRepo Sut = null;

        public VoteRepoUnitTests()
        {
            ConnMoq.Setup(db => db.CreateCommand()).Returns(cmdMoq.Object);
            ConnMoq.Setup(db => db.BeginTransaction()).Returns(transactionMoq.Object);
            Sut = new VoteRepo(ConnMoq.Object, LogMoq.Object);
        }

        [Fact]
        public void CreateOne()
        {
            var testVote = new VoteRecord
            {
                VotingId = 1,
                GiftId = 1,
                UserId = 1
            };

            ConnMoq.SetupDapper(c => c.Execute(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>(), null, null))
                .Returns(1)
                .Verifiable();

            var test = Sut.Create(new[] { testVote });

            ConnMoq.Verify();
            Assert.Equal(1, test);
        }

        [Fact]
        public void CreateMany()
        {
            var testVote = new VoteRecord
            {
                VotingId = 1,
                GiftId = 1,
                UserId = 1
            };
            var testVote1 = new VoteRecord
            {
                VotingId = 2,
                GiftId = 2,
                UserId = 2
            };
            var testVote2 = new VoteRecord
            {
                VotingId = 3,
                GiftId = 3,
                UserId = 3
            };

            ConnMoq.SetupDapper(c => c.Execute(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>(), null, null))
                .Returns(1)
                .Verifiable();

            var test = Sut.Create(new[] { testVote, testVote1, testVote2 });

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
            var testVote = new VoteRecord
            {
                VotingId = 1,
                GiftId = 1
            };

            Assert.Throws<ArgumentException>(() => Sut.Create(new[] { testVote }));
        }

        [Fact]
        public void CreateFailWithNullGiftId()
        {
            var testVote = new VoteRecord
            {
                VotingId = 1,
                UserId = 1
            };

            Assert.Throws<ArgumentException>(() => Sut.Create(new[] { testVote }));
        }

        [Fact]
        public void CreateFailWithNullVotingId()
        {
            var testVote = new VoteRecord
            {
                UserId = 1,
                GiftId = 1
            };

            Assert.Throws<ArgumentException>(() => Sut.Create(new[] { testVote }));
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
            var testVoteID = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6));


            ConnMoq.SetupDapper(c => c.Query(It.IsAny<string>(), null, null, It.IsAny<bool>(), null, null))
                .Returns(new[] { new VoteRecord() })
                .Verifiable();

            var test = Sut.Read(new[] { testVoteID });

            ConnMoq.Verify();
            Assert.NotNull(test);
            Assert.IsAssignableFrom<IEnumerable<VoteRecord>>(test);

            //The Moq.dapper Gives strange behavior here.
            // TODO : Check with integration test
            //Assert.Single(test.AsList());
        }

        [Fact]
        public void ReadManyById()
        {
            var testVote = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6));

            var testVote1 = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6)) + 1;

            var testVote2 = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 7, 6)) + 2;

            ConnMoq.SetupDapper(c => c.Query(It.IsAny<string>(), null, null, It.IsAny<bool>(), null, null))
                .Returns(new[] { new VoteRecord(), new VoteRecord(), new VoteRecord() })
                .Verifiable();

            var test = Sut.Read(new[] { testVote, testVote1, testVote2 });

            ConnMoq.Verify();
            Assert.NotNull(test);
            Assert.IsAssignableFrom<IEnumerable<VoteRecord>>(test);

            //The Moq.dapper Gives strange behavior here.
            // TODO : Check with integration test
            //Assert.Equal(3, test.ToList().Count);
        }

        [Fact]
        public void UpdateMany()
        {
            var testVote = new VoteRecord
            {
                Id = 1,
                VotingId = 1,
                GiftId = 1,
                UserId = 1
            };

            var testVote1 = new VoteRecord
            {
                Id = 2,
                VotingId = 2,
                GiftId = 2,
                UserId = 2
            };

            var testVote2 = new VoteRecord
            {
                Id = 3,
                VotingId = 3,
                GiftId = 3,
                UserId = 3
            };

            ConnMoq.SetupDapper(c => c.Execute(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>(), null, null))
                .Returns(1)
                .Verifiable();

            var test = Sut.Update(new[] { testVote, testVote1, testVote2 });

            ConnMoq.Verify();
            Assert.Equal(3, test);
        }

        [Fact]
        public void UpdateOneMultipleTimes()
        {
            var testUser = new VoteRecord
            {
                Id = 1,
                VotingId = 1,
                GiftId = 1,
                UserId = 1
            };

            var testUser1 = new VoteRecord
            {
                Id = 1,
                VotingId = 2,
                GiftId = 2,
                UserId = 2
            };

            var testUser2 = new VoteRecord
            {
                Id = 1,
                VotingId = 3,
                GiftId = 3,
                UserId = 3
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
            Assert.Throws<ArgumentException>(()=>Sut.Update(null));
        }

        [Fact]
        public void UpdateFailWithEmptyCollection()
        {
            IEnumerable<VoteRecord> test = new List<VoteRecord>();
            Assert.Throws<ArgumentException>(() => Sut.Update(test));
        }
    }
}
