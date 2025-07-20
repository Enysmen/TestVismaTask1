using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.CommandLine.NamingConventionBinder;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaTask1.Commands;  
using VismaTask1.Models;
using VismaTask1.Repositories;
namespace TestVismaTask1.Commands
{
    public class FactoryOptionsCommandTests
    {
        [Fact]
        public void CreateRegisterOptions_HasExpectedOptions()
        {
            var opts = FactoryOptionsCommand.CreateRegisterOptions().ToList();

            Assert.Equal(4, opts.Count);
            var names = opts.SelectMany(o => o.Aliases).ToHashSet();
            Assert.Contains("--title", names);
            Assert.Contains("--room", names);
            Assert.Contains("--category", names);
            Assert.Contains("--priority", names);
            Assert.All(opts, o => Assert.True(o.IsRequired));
        }

        [Theory]
        [InlineData("0")]
        [InlineData("11")]
        public void PriorityOption_ParserRejectsOutOfRange(string token)
        {
            var priorityOpt = FactoryOptionsCommand
                .CreateRegisterOptions()
                .OfType<Option<int>>()
                .Single(o => o.Aliases.Contains("--priority"));

            var root = new RootCommand { priorityOpt };
            var parser = new Parser(root);
            var result = parser.Parse($"--priority {token}");

            Assert.NotEmpty(result.Errors);
            Assert.Contains("range from 1 to 10", result.Errors[0].Message);
        }

        [Fact]
        public void CreateDeleteOptions_HasExpectedOptions()
        {
            var opts = FactoryOptionsCommand.CreateDeleteOptions().ToList();
            Assert.Equal(2, opts.Count);
            var names = opts.SelectMany(o => o.Aliases).ToHashSet();
            Assert.Contains("--title", names);
            Assert.Contains("--room", names);
            Assert.All(opts, o => Assert.True(o.IsRequired));
        }

        [Fact]
        public void CreateListOptions_HasExpectedOptionsAndOptional()
        {
            var opts = FactoryOptionsCommand.CreateListOptions().ToList();
            var names = opts.SelectMany(o => o.Aliases).ToHashSet();
            var expected = new[] { "--title", "--from", "--to", "--category", "--room" };
            foreach (var alias in expected)
            {
                Assert.Contains(alias, names);
            }
            Assert.All(opts, o => Assert.False(o.IsRequired));
        }

        [Theory]
        [InlineData("invalid_date")]
        [InlineData("2025/01/01")]
        public void FromOption_ParserRejectsBadDate(string bad)
        {
            var fromOpt = FactoryOptionsCommand
                .CreateListOptions()
                .OfType<Option<DateTime?>>()
                .Single(o => o.Aliases.Contains("--from"));

            var root = new RootCommand { fromOpt };
            var parser = new Parser(root);
            var result = parser.Parse($"--from {bad}");

            Assert.NotEmpty(result.Errors);
            Assert.Contains("expected yyyy-MM-dd", result.Errors[0].Message);
        }

        [Theory]
        [InlineData("UnknownCat")]
        public void CategoryOption_ValidatorRejectsInvalidEnum(string bad)
        {
            var opt = FactoryOptionsCommand
                .CreateListOptions()
                .OfType<Option<Category?>>()
                .Single(o => o.Aliases.Contains("--category"));

            var root = new RootCommand { opt };
            var parser = new Parser(root);
            var result = parser.Parse($"--category {bad}");

            Assert.NotEmpty(result.Errors);
            Assert.Contains("Invalid category", result.Errors[0].Message);
        }

        [Theory]
        [InlineData("UnknownRoom")]
        public void RoomOption_ValidatorRejectsInvalidEnum(string bad)
        {
            var opt = FactoryOptionsCommand
                .CreateListOptions()
                .OfType<Option<Room?>>()
                .Single(o => o.Aliases.Contains("--room"));

            var root = new RootCommand { opt };
            var parser = new Parser(root);
            var result = parser.Parse($"--room {bad}");

            Assert.NotEmpty(result.Errors);
            Assert.Contains("Invalid room", result.Errors[0].Message);
        }
    }
}

