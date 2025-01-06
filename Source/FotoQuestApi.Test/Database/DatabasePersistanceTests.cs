using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FotoQuestApi.Database;
using FotoQuestApi.Model;
using Moq;
using NUnit.Framework;

namespace FotoQuestApi.Test.Database
{
    [TestFixture]
    public class DatabasePersistanceTests
    {

        private DbPersistanceProvider dbPersistanceProvider;
        private Mock<IDatabaseProvider> databaseProvider;

        [SetUp]
        public void Setup()
        {
            databaseProvider = new Mock<IDatabaseProvider>();
        }

        [Test]
        public async Task LoadDataTest()
        {
            //arrange
            var id = Guid.NewGuid().ToString();
            databaseProvider.SetReturnsDefault(Task.Run(() => GetData()));

            dbPersistanceProvider = new DbPersistanceProvider(databaseProvider.Object);

            //act
            var response = await dbPersistanceProvider.LoadImageData(id);

            //assert
            Assert.That(response is not null);
            Assert.That(response.Id.Equals(id));


            FileData GetData()
            {
                return new FileData { Id = id };
            }
        }


        [Test]
        public async Task SaveImageDataTest()
        {
            //arrange
            var id = Guid.NewGuid().ToString();
            databaseProvider.SetReturnsDefault(Task.Run(() => 1));
            dbPersistanceProvider = new DbPersistanceProvider(databaseProvider.Object);

            //act
            var response = await dbPersistanceProvider.SaveImageData(new FileData());

            //assert
            Assert.That(1, Is.EqualTo(response));
        }

        [Test]
        public async Task SaveImageDataTest_Fail()
        {
            //arrange
            var id = Guid.NewGuid().ToString();
            databaseProvider.Setup(x => x.SaveImageData(new GetImageDataRequest { Id = id })).Returns(Task.Run(() => 0));
            dbPersistanceProvider = new DbPersistanceProvider(databaseProvider.Object);

            //act
            Exception ex = Assert.ThrowsAsync<Exception>(async () => await dbPersistanceProvider.SaveImageData(new FileData { Id = id }));

            //assert
            Assert.That(ex.Message, Is.EqualTo($"Not able to save Data in Database with id {id}"));
        }
    }
}
