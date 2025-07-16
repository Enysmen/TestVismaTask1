using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VismaTask1.Commands;  
using VismaTask1.Models;
using VismaTask1.Repositories;
namespace TestVismaTask1.Commands
{
    public static class FactoryOptionsCommand
    {
        public static IEnumerable<Option> CreateRegisterOptions()
        {
            // Собираем в List вместо yield, чтобы потом пройтись и исправить Name
            var options = new List<Option>
            {
                new Option<string>("--title")
                {
                    Description = "Application title",
                    IsRequired = true
                },
                new Option<Room>("--room")
                {
                    Description = "Room",
                    IsRequired = true
                },
                new Option<Category>("--category")
                {
                    Description = "Category",
                    IsRequired = true
                },
                new Option<int>("--priority")
                {
                    Description = "Priority (1–10)",
                    IsRequired = true
                }
            };

            // Добавляем ту же валидацию для priority, как у вас было
            options.OfType<Option<int>>()
                   .Single(o => o.Name == "priority")
                   .AddValidator(r =>
                   {
                       var v = r.GetValueOrDefault<int>();
                       if (v < 1 || v > 10)
                           r.ErrorMessage = "The priority must be in the range from 1 to 10.";
                   });

            // Рефлексией корректируем Name у каждой опции
            var nameProp = typeof(Option).GetProperty("Name");
            foreach (var opt in options)
            {
                // Достаём старое имя без дефисов
                var orig = (string)nameProp.GetValue(opt);
                // Если нет ведущих дефисов — добавляем их
                if (!orig.StartsWith("--"))
                    nameProp.SetValue(opt, "--" + orig);
            }

            return options;
        }

        public static IEnumerable<Option> CreateDeleteOptions()
        {
            var options = new List<Option>
            {
                new Option<string>("--title")
                {
                    Description = "Application title",
                    IsRequired = true
                },
                new Option<Room>("--room")
                {
                    Description = "Room",
                    IsRequired = true
                }
            };

            var nameProp = typeof(Option).GetProperty("Name");
            foreach (var opt in options)
            {
                var orig = (string)nameProp.GetValue(opt);
                if (!orig.StartsWith("--"))
                    nameProp.SetValue(opt, "--" + orig);
            }

            return options;
        }

        public static IEnumerable<Option> CreateListOptions()
        {
            var options = new List<Option>
            {
                new Option<string?>("--title") { Description = "Filter by title" },
                new Option<DateTime?>(new[] { "--from" }, parseArgument: ParseDate) { Description = "Date from (yyyy-MM-dd)" },
                new Option<DateTime?>(new[] { "--to"   }, parseArgument: ParseDate) { Description = "Date to (yyyy-MM-dd)" },
                new Option<Category?>(new[] { "--category" }) { Description = "Filter by category" },
                new Option<Room?>(new[] { "--room"     }) { Description = "Filter by room" }
            };

            // Добавим валидацию, как у вас было
            options.Single(o => (string)typeof(Option).GetProperty("Name").GetValue(o) == "category")
                   .AddValidator(r =>
                   {
                       if (r.Tokens.Count == 0) return;
                       var raw = r.Tokens.Single().Value;
                       if (!Enum.TryParse<Category>(raw, true, out _))
                           r.ErrorMessage = $"Invalid category '{raw}'. Valid: Electronics, Food, Other.";
                   });
            options.Single(o => (string)typeof(Option).GetProperty("Name").GetValue(o) == "room")
                   .AddValidator(r =>
                   {
                       if (r.Tokens.Count == 0) return;
                       var raw = r.Tokens.Single().Value;
                       if (!Enum.TryParse<Room>(raw, true, out _))
                           r.ErrorMessage = $"Invalid room '{raw}'. Valid: MeetingRoom, Kitchen, Bathroom.";
                   });

            var nameProp = typeof(Option).GetProperty("Name");
            foreach (var opt in options)
            {
                var orig = (string)nameProp.GetValue(opt);
                if (!orig.StartsWith("--"))
                    nameProp.SetValue(opt, "--" + orig);
            }

            return options;
        }

        private static DateTime? ParseDate(ArgumentResult result)
        {
            var token = result.Tokens.SingleOrDefault()?.Value;
            if (DateTime.TryParseExact(token, "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var d))
            {
                return d;
            }

            result.ErrorMessage = $"Invalid date format '{token}', expected yyyy-MM-dd";
            return null;
        }
    }
}
