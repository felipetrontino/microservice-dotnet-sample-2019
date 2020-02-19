using FluentAssertions;
using Framework.Core.Entities;
using Xunit;

namespace Framework.Tests.UnitTests.Entities
{
    public class EntityTest
    {
        [Fact]
        public void Constructor_Valid()
        {
            // arrange

            // act
            var result = Constructor();

            // assert
            var resultExpected = GetObj();
            resultExpected.Id = result.Id;
            result.Should().BeEquivalentTo(resultExpected);
        }

        private static Test Constructor()
        {
            return new Test();
        }

        #region Mocks

        private static Test GetObj()
        {
            return new Test();
        }

        public class Test : Entity
        {
            public string Name { get; set; }
        }

        #endregion Mocks
    }
}