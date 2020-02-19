﻿using Framework.Core.Bus;
using Library.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Library.Domain.Models.Event
{
    public class CreateReservationEvent : BaseMessage
    {
        public string Number { get; set; }
        public MemberDetail Member { get; set; }

        public StatusReservation Status { get; set; }

        public DateTime RequestDate { get; set; }

        public List<LoanDetail> Loans { get; set; } = new List<LoanDetail>();

        public class MemberDetail
        {
            public string DocumentId { get; set; }

            public string Name { get; set; }
        }

        public class LoanDetail
        {
            public DateTime DueDate { get; set; }
            public DateTime? ReturnDate { get; set; }
            public string Title { get; set; }
            public string CopyNumber { get; set; }
        }
    }
}