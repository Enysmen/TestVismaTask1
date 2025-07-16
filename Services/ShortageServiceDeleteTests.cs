using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using VismaTask1.Models;
using VismaTask1.Repositories;
using VismaTask1.Services;
using Moq;

namespace TestVismaTask1.Services
{
    public class ShortageServiceDeleteTests
    {
        private readonly Mock<IShortageRepository> _repoMock;
        private readonly ShortageService _service;

        public ShortageServiceDeleteTests()
        {
            _repoMock = new Mock<IShortageRepository>();
            var logger = new LoggerFactory().CreateLogger<ShortageService>();
            _service = new ShortageService(_repoMock.Object, logger);
        }

        [Fact]
        public void Delete_ByCreator_RemovesItem()
        {
            var item = new Shortage { Title = "T", Room = Room.Kitchen, Name = "alice" };
            _repoMock.Setup(r => r.LoadAll()).Returns(new List<Shortage> { item });

            List<Shortage> saved = null;
            _repoMock.Setup(r => r.SaveAll(It.IsAny<List<Shortage>>())).Callback<List<Shortage>>(lst => saved = lst);

            _service.Delete("T", Room.Kitchen, "alice", isAdmin: false);

            Assert.NotNull(saved);
            Assert.Empty(saved);
        }

        [Fact]
        public void Delete_ByAdmin_RemovesItem()
        {
            var item = new Shortage { Title = "T", Room = Room.Bathroom, Name = "bob" };
            _repoMock.Setup(r => r.LoadAll()).Returns(new List<Shortage> { item });

            List<Shortage> saved = null;
            _repoMock.Setup(r => r.SaveAll(It.IsAny<List<Shortage>>())).Callback<List<Shortage>>(lst => saved = lst);

            _service.Delete("T", Room.Bathroom, "alice", isAdmin: true);

            Assert.Empty(saved);
        }

        [Fact]
        public void Delete_NotFound_ThrowsInvalidOperationException()
        {
            _repoMock.Setup(r => r.LoadAll()).Returns(new List<Shortage>());

            Assert.Throws<InvalidOperationException>(() => _service.Delete("NonExistent", Room.Kitchen, "user", isAdmin: false));
        }

        [Fact]
        public void Delete_Unauthorized_ThrowsUnauthorizedAccessException()
        {
            var item = new Shortage { Title = "X", Room = Room.Kitchen, Name = "owner" };
            _repoMock.Setup(r => r.LoadAll()).Returns(new List<Shortage> { item });

            Assert.Throws<UnauthorizedAccessException>(() => _service.Delete("X", Room.Kitchen, "notOwner", isAdmin: false));
        }
    }
}
