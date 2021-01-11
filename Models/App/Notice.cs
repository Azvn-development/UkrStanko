using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UkrStanko.Models.App
{
    public class Notice
    {
        public DateTime Date { get; set; }
        public Requisition Requisition { get; set; }
        public Proposal Proposal { get; set; }
        public Message Message { get; set; }
    }
}