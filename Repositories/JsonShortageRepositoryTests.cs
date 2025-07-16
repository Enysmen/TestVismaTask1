using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Moq;

namespace TestVismaTask1.Repositories
{
    public class JsonShortageRepositoryTests : IDisposable
    {
        private readonly string _tempFile;
        private readonly Mock<ILogger<JsonShortageRepository>> _loggerMock;

        public JsonShortageRepositoryTests()
        {
            _tempFile = Path.GetTempFileName();
            _loggerMock = new Mock<ILogger<JsonShortageRepository>>();
        }

        [Fact]
        public void LoadAll_FileDoesNotExist_ReturnsEmptyList()
        {
            File.Delete(_tempFile);
            var repo = new JsonShortageRepository(_tempFile, _loggerMock.Object);

            var result = repo.LoadAll();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void LoadAll_WithValidJson_ReturnsDeserializedList()
        {
            var list = new List<Shortage>
            {
                new Shortage { Title="A", Room=Room.Kitchen, Category=Category.Food, Priority=3, Name="u1", CreatedOn=DateTime.UtcNow }
            };
            var json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_tempFile, json);

            var repo = new JsonShortageRepository(_tempFile, _loggerMock.Object);
            var result = repo.LoadAll();

            Assert.Single(result);
            Assert.Equal("A", result[0].Title);
        }

        [Fact]
        public void SaveAll_WritesJsonFile()
        {
            var list = new List<Shortage>
            {
                new Shortage { Title="B", Room=Room.Bathroom, Category=Category.Other, Priority=7, Name="u2", CreatedOn=DateTime.UtcNow }
            };
            var repo = new JsonShortageRepository(_tempFile, _loggerMock.Object);

            repo.SaveAll(list);

            Assert.True(File.Exists(_tempFile));
            var text = File.ReadAllText(_tempFile);
            var deserialized = JsonSerializer.Deserialize<List<Shortage>>(text);
            Assert.Single(deserialized);
            Assert.Equal("B", deserialized[0].Title);
        }

        public void Dispose()
        {
            if (File.Exists(_tempFile))
                File.Delete(_tempFile);
        }
    }
}
