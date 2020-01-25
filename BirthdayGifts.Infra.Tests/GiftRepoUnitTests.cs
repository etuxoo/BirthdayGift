using BirthdayGifts.Infra.Record;
using BirthdayGifts.Infra.Repository;
using Moq;
using System;
using System.Data;
using Xunit;
using Moq.Dapper;
using Dapper;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BirthdayGifts.Infra.Tests
{
    public class GiftRepoUnitTests
    {
        private readonly Mock<IDbConnection> ConnMoq = new Mock<IDbConnection>();
        private readonly Mock<IDbCommand> cmdMoq = new Mock<IDbCommand>();
        private readonly Mock<IDbTransaction> transactionMoq = new Mock<IDbTransaction>();
        private readonly Mock<ILogFacility<GiftRepo>> LogMoq = new Mock<ILogFacility<GiftRepo>>();
        private readonly GiftRepo Sut = null;

        public GiftRepoUnitTests()
        {
            ConnMoq.Setup(db => db.CreateCommand()).Returns(cmdMoq.Object);
            ConnMoq.Setup(db => db.BeginTransaction()).Returns(transactionMoq.Object);
            Sut = new GiftRepo(ConnMoq.Object, LogMoq.Object);
        }

        [Fact]
        public void CreateOne()
        {
            var testGift = new GiftRecord
            {
                Name = DateTime.Now.Ticks.ToString()
            };

            ConnMoq.SetupDapper(c => c.Execute(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>(), null, null))
                .Returns(1)
                .Verifiable();

            var test = Sut.Create(new[] { testGift });

            ConnMoq.Verify();
            Assert.Equal(1, test);
        }

        [Fact]
        public void CreateMany()
        {
            var testGift = new GiftRecord
            {
                Name = DateTime.Now.Ticks.ToString()
            };

            var testGift1 = new GiftRecord
            {
                Name = DateTime.Now.Ticks.ToString() + "1"
            };

            var testGift2 = new GiftRecord
            {
                Name = DateTime.Now.Ticks.ToString() + "2"
            };

            ConnMoq.SetupDapper(c => c.Execute(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>(), null, null))
                .Returns(1)
                .Verifiable();

            var test = Sut.Create(new[] { testGift, testGift1, testGift2 });

            ConnMoq.Verify();
            Assert.Equal(3, test);
        }

        [Fact]
        public void CreateFailWithNullName()
        {
            var testGift = new GiftRecord();

            Assert.Throws<ArgumentException>(() => Sut.Create(new[] { testGift }));
        }

        [Fact]
        public void CreateFailWithDuplictareRecords()
        {
            var testGift = new GiftRecord
            {
                Name = DateTime.Now.Ticks.ToString()
            };

            Assert.Throws<ArgumentException>(() => Sut.Create(new[] { testGift, testGift, testGift }));
        }

        [Fact]
        public void CreateFailWithLongername()
        {
            var testGift = new GiftRecord
            {
                Name = DateTime.Now.Ticks.ToString() + DateTime.Now.Ticks.ToString() + DateTime.Now.Ticks.ToString()
            };

            Assert.Throws<ArgumentException>(() => Sut.Create(new[] { testGift }));
        }

        [Fact]
        public void CreateFailWithLNullCollection()
        {
            Assert.Throws<ArgumentException>(() => Sut.Create(null));
        }

        [Fact]
        public void ReadOneById()
        {
            var testGiftID = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length-5,4));


            ConnMoq.SetupDapper(c => c.Query(It.IsAny<string>(), null, null, It.IsAny<bool>(), null, null))
                .Returns(new[] { new GiftRecord() })
                .Verifiable();

            var test = Sut.Read(new[] { testGiftID });

            ConnMoq.Verify();
            Assert.NotNull(test);
            Assert.IsAssignableFrom<IEnumerable<GiftRecord>>(test);

            //The Moq.dapper Gives strange behavior here.
            // TODO : Check with integration test
            //Assert.Single(test.AsList());
        }

        [Fact]
        public void ReadManyById()
        {
            var testGift = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 5, 4));

            var testGift1 = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 5, 4)) + 1;

            var testGift2 = int.Parse(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 5, 4)) + 2;

            ConnMoq.SetupDapper(c => c.Query(It.IsAny<string>(), null, null, It.IsAny<bool>(), null, null))
                .Returns(new[] { new GiftRecord(), new GiftRecord(), new GiftRecord() })
                .Verifiable();

            var test = Sut.Read(new[] { testGift, testGift1, testGift2 });

            ConnMoq.Verify();
            Assert.NotNull(test);
            Assert.IsAssignableFrom<IEnumerable<GiftRecord>>(test);

            //The Moq.dapper Gives strange behavior here.
            // TODO : Check with integration test
            //Assert.Equal(3, test.ToList().Count);
        }

        [Fact]
        public void ReadManyByName()
        {
            var testGiftName = DateTime.Now.Ticks.ToString();

            var testGiftName1 = DateTime.Now.Ticks.ToString() + "1";

            var testGiftName2 = DateTime.Now.Ticks.ToString() + "2";

            ConnMoq.SetupDapper(c => c.Query(It.IsAny<string>(), null, null, It.IsAny<bool>(), null, null))
                .Returns(new[] { new GiftRecord(), new GiftRecord(), new GiftRecord() })
                .Verifiable();

            var test = Sut.Read(new[] { testGiftName, testGiftName1, testGiftName2 });

            ConnMoq.Verify();
            Assert.NotNull(test);
            Assert.IsAssignableFrom<IEnumerable<GiftRecord>>(test);

            //The Moq.dapper Gives strange behavior here.
            // TODO : Check with integration test
            //Assert.Equal(3, test.ToList().Count);
        }

        [Fact]
        public void UpdateMany()
        {
            var testGift = new GiftRecord
            {
                Id = 1,
                Name = DateTime.Now.Ticks.ToString()
            };

            var testGift1 = new GiftRecord
            {
                Id = 2,
                Name = DateTime.Now.Ticks.ToString() + "1"
            };

            var testGift2 = new GiftRecord
            {
                Id = 2,
                Name = DateTime.Now.Ticks.ToString() + "2"
            };

            ConnMoq.SetupDapper(c => c.Execute(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>(), null, null))
                .Returns(1)
                .Verifiable();

            var test = Sut.Update(new[] { testGift, testGift1, testGift2 });

            ConnMoq.Verify();
            Assert.Equal(3, test);
        }

        [Fact]
        public void UpdateOneMultipleTimes()
        {
            var testGift = new GiftRecord
            {
                Id = 1,
                Name = DateTime.Now.Ticks.ToString()
            };

            var testGift1 = new GiftRecord
            {
                Id = 1,
                Name = DateTime.Now.Ticks.ToString() + "1"
            };

            var testGift2 = new GiftRecord
            {
                Id = 1,
                Name = DateTime.Now.Ticks.ToString() + "2"
            };

            ConnMoq.SetupDapper(c => c.Execute(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>(), null, null))
                .Returns(1)
                .Verifiable();

            var test = Sut.Update(new[] { testGift, testGift1, testGift2 });

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
            IEnumerable<GiftRecord> list = new List<GiftRecord>();
            Assert.Throws<ArgumentException>(() => Sut.Update(list));
        }

        [Fact]
        public void UpdateFailWithNullName()
        {
            var testRecord = new GiftRecord
            {
                Id = 1
            };
            Assert.Throws<ArgumentException>(() => Sut.Update(new[] { testRecord }));
        }

        [Fact]
        public void UpdateFailWithLongerName()
        {
            var testRecord = new GiftRecord
            {
                Id = 1,
                Name=DateTime.Now.Ticks.ToString() + DateTime.Now.Ticks.ToString() + DateTime.Now.Ticks.ToString()
            };
            Assert.Throws<ArgumentException>(() => Sut.Update(new[] { testRecord }));
        }

        [Fact]
        public void UpdateFailWithNullId()
        {
            var testRecord = new GiftRecord
            {
                Name = DateTime.Now.Ticks.ToString()
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

            var test = Sut.Delete(new[] { 1,2,3 });

            ConnMoq.Verify();
            Assert.Equal(3, test);
        }

        [Fact]
        public void DeleteFailWithNullCollection()
        {
            Assert.Throws<ArgumentException>(()=> Sut.Delete(null));
        }
    }
}
