using Framework.Core.Bus;
using MediatR;
using System.Collections.Generic;

namespace Library.Domain.CommandSide.Commands
{
    public class ReserveBookCommand : BusMessage, IRequest<bool>
    {
        public string Number { get; set; }
        public string MemberId { get; set; }

        public string MemberName { get; set; }

        public List<Item> Items { get; set; } = new List<Item>();

        public class Item
        {
            public string Number { get; set; }
            public string Name { get; set; }
        }
    }
}