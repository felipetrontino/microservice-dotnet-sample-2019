using Framework.Test.Common;
using Library.Domain.CommandSide.Commands;
using Library.Tests.Utils;
using System.Collections.Generic;

namespace Library.Tests.Mocks.Commands
{
    public class ReserveBookCommandMock : MockBuilder<ReserveBookCommandMock, ReserveBookCommand>
    {
        public static ReserveBookCommand Get(string key)
        {
            return Create(key).Default().Build();
        }

        public ReserveBookCommandMock Default()
        {
            Value.Number = Fake.GetReservationNumber(Key);
            Value.MemberId = MockBuilder.GetId(Key).ToString();
            Value.MemberName = Fake.GetMemberName(Key);

            Value.Items = new List<ReserveBookCommand.Item>
            {
                GetItem(Key)
            };

            return this;
        }

        private ReserveBookCommand.Item GetItem(string key)
        {
            var ret = CreateModel<ReserveBookCommand.Item>(key);
            ret.Name = Fake.GetTitle(key);
            ret.Number = Fake.GetCopyNumber(key);

            return ret;
        }
    }
}