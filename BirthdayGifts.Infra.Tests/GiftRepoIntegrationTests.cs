using BirthdayGifts.Infra.Record;
using BirthdayGifts.Infra.Repository;
using BirthdayGifts.Infra.Schema;
using Dapper.FluentMap;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Xunit;

namespace BirthdayGifts.Infra.Tests
{
    public class GiftRepoIntegrationTests
    {

        private readonly SqlConnection Conn = null;
        private readonly GiftRepo Sut = null;
        private readonly ConnectionStringProvider cs = null;

        public GiftRepoIntegrationTests()
        {
            cs = new ConnectionStringProvider();

            Conn = new SqlConnection(cs.ConnectionString);

            var log = new SeriLogFacility<GiftRepo>(Log.Logger);
            Sut = new GiftRepo(Conn, log);

            FluentMapper.EntityMaps.Clear();
            FluentMapper.Initialize(cfg => cfg.AddMap(new GiftSchema()));
            Conn.Open();
        }

        [Fact]
        public void ReadWithNoFilter()
        {
            var test = Sut.Read();

            Assert.NotNull(test);
            Assert.IsAssignableFrom<IEnumerable<GiftRecord>>(test);
        }

        [Fact]
        public void CreateReadWithFilterNameDelete()
        {
            var gift = new GiftRecord
            {
                Name = DateTime.Now.Ticks.ToString()
            };

            var insertionTest = Sut.Create(new[] { gift });

            Assert.Equal(1, insertionTest);

            var test = Sut.Read(new[] { gift.Name });

            var testDelete = Sut.Delete(new[] { test.FirstOrDefault().Id ?? 0 });

            Assert.Equal(1, testDelete);

            Assert.NotNull(test);
            Assert.IsAssignableFrom<IEnumerable<GiftRecord>>(test);
            Assert.NotEmpty(test);
        }

        [Fact]
        public void CreateReadWithFilterNameReadWithFilterIdDelete()
        {
            var gift = new GiftRecord
            {
                Name = DateTime.Now.Ticks.ToString()
            };

            var insertionTest = Sut.Create(new[] { gift });

            Assert.Equal(1, insertionTest);

            var testName = Sut.Read(new[] { gift.Name });

            Assert.NotNull(testName);
            Assert.IsAssignableFrom<IEnumerable<GiftRecord>>(testName);
            Assert.NotEmpty(testName);

            var testId = Sut.Read(new[] { testName.FirstOrDefault().Id ?? 0 });

            var testDelete = Sut.Delete(new[] { testName.FirstOrDefault().Id ?? 0 });

            Assert.Equal(1, testDelete);

            Assert.NotNull(testId);
            Assert.IsAssignableFrom<IEnumerable<GiftRecord>>(testId);
            Assert.NotEmpty(testId);
        }

        [Fact]
        public void CreateReadDelete()
        {
            var gift = new GiftRecord
            {
                Name = DateTime.Now.Ticks.ToString()
            };

            var insertionTest = Sut.Create(new[] { gift });

            Assert.Equal(1, insertionTest);

            var testName = Sut.Read(new[] { gift.Name });

            Assert.NotNull(testName);
            Assert.IsAssignableFrom<IEnumerable<GiftRecord>>(testName);
            Assert.NotEmpty(testName);

            var testDelete = Sut.Delete(new[] { testName.FirstOrDefault().Id ?? 0 });

            Assert.Equal(1, testDelete);

            var areValuesInsertedCorrected = gift.Name == testName.FirstOrDefault().Name;

            Assert.True(areValuesInsertedCorrected);
        }

        [Fact]
        public void CreateReadUpdateReadDelete()
        {
            var gift = new GiftRecord
            {
                Name = DateTime.Now.Ticks.ToString()
            };

            var insertionTest = Sut.Create(new[] { gift });

            Assert.Equal(1, insertionTest);

            var testName = Sut.Read(new[] { gift.Name });

            Assert.NotNull(testName);
            Assert.IsAssignableFrom<IEnumerable<GiftRecord>>(testName);
            Assert.NotEmpty(testName);

            testName.FirstOrDefault().Name +="11";

            var testUpdate = Sut.Update(testName);

            Assert.Equal(1, testUpdate);

            var testDelete = Sut.Delete(new[] { testName.FirstOrDefault().Id ?? 0 });

            Assert.Equal(1, testDelete);

            var areValuesInsertedCorrected = gift.Name + "11" == testName.FirstOrDefault().Name;

            Assert.True(areValuesInsertedCorrected);
        }
    }
}
