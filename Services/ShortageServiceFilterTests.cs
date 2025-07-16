using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Moq;

namespace TestVismaTask1.Services
{
    public class ShortageServiceFilterTests
    {
        private readonly ShortageService _service;

        public ShortageServiceFilterTests()
        {
            // Репозиторий в фильтре не используется
            var repoMock = new Mock<IShortageRepository>();
            var logger = new LoggerFactory().CreateLogger<ShortageService>();
            _service = new ShortageService(repoMock.Object, logger);
        }

        [Fact]
        public void Filter_ByTitleAndCategoryAndRoomAndDate_ReturnsMatching()
        {
            var list = new[]
            {
                new Shortage { Title="wireless speaker", CreatedOn=new DateTime(2025,1,10), Priority=5, Category=Category.Electronics, Room=Room.Kitchen },
                new Shortage { Title="office desk",       CreatedOn=new DateTime(2025,2, 5), Priority=3, Category=Category.Other,       Room=Room.Bathroom },
                new Shortage { Title="Food tray",         CreatedOn=new DateTime(2025,1,15), Priority=8, Category=Category.Food,        Room=Room.Kitchen },
            };

            var result = _service.Filter(
                list,
                title: "Speaker",
                from: new DateTime(2025, 1, 1),
                to: new DateTime(2025, 1, 31),
                category: Category.Electronics,
                room: Room.Kitchen);

            var only = Assert.Single(result);
            Assert.Contains("speaker", only.Title, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Filter_NoParameters_ReturnsAllSortedByPriorityThenDate()
        {
            var a = new Shortage { Title = "A", Priority = 2, CreatedOn = new DateTime(2025, 1, 1) };
            var b = new Shortage { Title = "B", Priority = 5, CreatedOn = new DateTime(2025, 1, 2) };
            var c = new Shortage { Title = "C", Priority = 5, CreatedOn = new DateTime(2025, 1, 1) };
            var list = new[] { a, b, c };

            var result = _service.Filter(list);

            var ordered = result.ToList();
            Assert.Equal(new[] { b, c, a }, ordered);
        }
    }
}
