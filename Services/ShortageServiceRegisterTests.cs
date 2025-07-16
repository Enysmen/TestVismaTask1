using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Moq;

namespace TestVismaTask1.Services
{
    public class ShortageServiceRegisterTests
    {
        private readonly Mock<IShortageRepository> _repoMock;
        private readonly ShortageService _service;

        public ShortageServiceRegisterTests()
        {
            _repoMock = new Mock<IShortageRepository>();
            var logger = new LoggerFactory().CreateLogger<ShortageService>();
            _service = new ShortageService(_repoMock.Object, logger);
        }

        [Fact]
        public void Register_NewShortage_CallsSaveAllWithSingleItem()
        {
            _repoMock.Setup(r => r.LoadAll()).Returns(new List<Shortage>());
            List<Shortage> saved = null;
            _repoMock.Setup(r => r.SaveAll(It.IsAny<List<Shortage>>()))
                     .Callback<List<Shortage>>(lst => saved = lst);

            var input = new Shortage
            {
                Title = "NewItem",
                Room = Room.Kitchen,
                Category = Category.Food,
                Priority = 5
            };

            _service.Register(input, "alice");

            Assert.NotNull(saved);
            Assert.Single(saved);
            Assert.Equal("alice", saved[0].Name);
            Assert.Equal(5, saved[0].Priority);
            Assert.True((DateTime.UtcNow - saved[0].CreatedOn).TotalSeconds < 5);
        }

        [Fact]
        public void Register_DuplicateLowerPriority_ThrowsInvalidOperationException()
        {
            var existing = new Shortage
            {
                Title = "X",
                Room = Room.MeetingRoom,
                Category = Category.Electronics,
                Priority = 8,
                Name = "bob",
                CreatedOn = DateTime.UtcNow.AddHours(-1)
            };
            _repoMock.Setup(r => r.LoadAll()).Returns(new List<Shortage> { existing });

            var input = new Shortage
            {
                Title = "X",
                Room = Room.MeetingRoom,
                Category = Category.Electronics,
                Priority = 5
            };

            Assert.Throws<InvalidOperationException>(() => _service.Register(input, "bob"));
        }

        [Fact]
        public void Register_DuplicateHigherPriority_ReplacesExisting()
        {
            var existing = new Shortage
            {
                Title = "Y",
                Room = Room.Bathroom,
                Category = Category.Other,
                Priority = 2,
                Name = "charlie",
                CreatedOn = DateTime.UtcNow.AddHours(-2)
            };
            _repoMock.Setup(r => r.LoadAll()).Returns(new List<Shortage> { existing });

            List<Shortage> saved = null;
            _repoMock.Setup(r => r.SaveAll(It.IsAny<List<Shortage>>()))
                     .Callback<List<Shortage>>(lst => saved = lst);

            var input = new Shortage
            {
                Title = "Y",
                Room = Room.Bathroom,
                Category = Category.Other,
                Priority = 9
            };

            _service.Register(input, "charlie");

            Assert.Single(saved);
            Assert.Equal(9, saved[0].Priority);
            Assert.Equal("charlie", saved[0].Name);
        }
    }
}
