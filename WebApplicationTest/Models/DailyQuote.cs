using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationTest.Models
{
    public class DailyQuote
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string Source { get; set; }

        public DateTime Day { get; set; }

        public string Creator { get; set; }

        public DailyQuote()
        {

        }

    }
}
