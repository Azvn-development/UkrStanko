using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UkrStanko.Models.App
{
    public class ReadedNotices
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ReadedNoticesCount { get; set; }
    }
}