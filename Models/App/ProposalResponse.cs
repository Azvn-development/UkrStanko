using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace UkrStanko.Models.App
{
    public class ProposalResponse
    {
        public int Id { get; set; }

        public int MessageId { get; set; }
        public Message Message { get; set; }

        public int? ProposalId { get; set; }
        public Proposal Proposal { get; set; }
    }
}