﻿using Framework.Core.Entities;
using Library.Domain.Entities;
using Library.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Library.Entities
{
    public class Reservation : Entity
    {
        public string Number { get; set; }
        public Member Member { get; set; }

        public StatusReservation Status { get; set; }

        public DateTime RequestDate { get; set; }

        public List<Loan> Loans { get; set; } = new List<Loan>();
    }
}