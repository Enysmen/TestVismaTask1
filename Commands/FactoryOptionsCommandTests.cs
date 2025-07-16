using System;
using System.Collections.Generic;
using System.CommandLine;
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
        public void CreateRegisterOptions_ContainsAllExpectedOptions()
        {
            var opts = FactoryOptionsCommand.CreateRegisterOptions().ToList();

            // --title : Option<string>
            Assert.Contains(opts, o =>
                o.Name == "--title"
                && o.GetType().IsGenericType
                && o.GetType().GetGenericTypeDefinition() == typeof(Option<>)
                && o.GetType().GetGenericArguments()[0] == typeof(string)
            );

            // --room : Option<Room>
            Assert.Contains(opts, o =>
                o.Name == "--room"
                && o.GetType().GetGenericTypeDefinition() == typeof(Option<>)
                && o.GetType().GetGenericArguments()[0] == typeof(Room)
            );

            // --category : Option<Category>
            Assert.Contains(opts, o =>
                o.Name == "--category"
                && o.GetType().GetGenericTypeDefinition() == typeof(Option<>)
                && o.GetType().GetGenericArguments()[0] == typeof(Category)
            );

            // --priority : Option<int>
            Assert.Contains(opts, o =>
                o.Name == "--priority"
                && o.GetType().GetGenericTypeDefinition() == typeof(Option<>)
                && o.GetType().GetGenericArguments()[0] == typeof(int)
            );

            Assert.Equal(4, opts.Count);
        }

        [Fact]
        public void CreateListOptions_ContainsAllExpectedOptions()
        {
            var opts = FactoryOptionsCommand.CreateListOptions().ToList();

            // --title : Option<string>
            Assert.Contains(opts, o =>
                o.Name == "--title"
                && o.GetType().GetGenericArguments()[0] == typeof(string)
            );

            // --from : Option<DateTime?>
            Assert.Contains(opts, o =>
                o.Name == "--from"
                && o.GetType().GetGenericArguments()[0] == typeof(DateTime?)
            );

            // --to : Option<DateTime?>
            Assert.Contains(opts, o =>
                o.Name == "--to"
                && o.GetType().GetGenericArguments()[0] == typeof(DateTime?)
            );

            // --category : Option<Category?>
            Assert.Contains(opts, o =>
                o.Name == "--category"
                && o.GetType().GetGenericArguments()[0] == typeof(Category?)
            );

            // --room : Option<Room?>
            Assert.Contains(opts, o =>
                o.Name == "--room"
                && o.GetType().GetGenericArguments()[0] == typeof(Room?)
            );

            Assert.Equal(5, opts.Count);
        }

        [Fact]
        public void CreateDeleteOptions_ContainsTitleAndRoom()
        {
            var opts = FactoryOptionsCommand.CreateDeleteOptions().ToList();

            // --title : Option<string>
            Assert.Contains(opts, o =>
                o.Name == "--title"
                && o.GetType().GetGenericArguments()[0] == typeof(string)
            );

            // --room : Option<Room>
            Assert.Contains(opts, o =>
                o.Name == "--room"
                && o.GetType().GetGenericArguments()[0] == typeof(Room)
            );

            Assert.Equal(2, opts.Count);
        }
    }
}
