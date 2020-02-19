using FluentAssertions;
using Framework.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Framework.Tests.UnitTests.Core.Extensions
{
    public class EnumerableExtensionsTest
    {
        #region ForEachAsync

        [Fact]
        public void ForEachAsync_Valid()
        {
            // arrange
            var executed = false;
            var source = GetSource();
            const int dop = 1;

            Task Body(Test x)
            {
                executed = true;
                return Task.CompletedTask;
            }

            // act
            ForEachAsync(source, dop, Body);

            // assert
            executed.Should().BeTrue();
        }

        [Fact]
        public void ForEachAsync_Source_Null()
        {
            // arrange

            var source = GetSourceNull();
            const int dop = 1;

            static Task Body(Test x)
            {
                return Task.CompletedTask;
            }

            // act
            Action action = () => ForEachAsync(source, dop, Body);

            // assert
            action.Should().Throw<Exception>();
        }

        [Fact]
        public void ForEachAsync_Source_Empty()
        {
            // arrange
            var executed = false;
            var source = GetSourceEmpty();
            const int dop = 1;

            Task Body(Test x)
            {
                executed = true;
                return Task.CompletedTask;
            }

            // act
            ForEachAsync(source, dop, Body);

            // assert
            executed.Should().BeFalse();
        }

        #endregion ForEachAsync

        private static void ForEachAsync<T>(IEnumerable<T> source, int dop, Func<T, Task> body)
        {
            source.ForEachAsync(dop, body).Wait();
        }

        #region Mocks

        private static IEnumerable<Test> GetSource(int qtd = 1)
        {
            var ret = new List<Test>();

            for (var i = 0; i < qtd; i++)
            {
                ret.Add(new Test());
            }

            return ret;
        }

        private static IEnumerable<Test> GetSourceNull()
        {
            return null;
        }

        private static IEnumerable<Test> GetSourceEmpty()
        {
            return new List<Test>();
        }

        public class Test
        {
        }

        #endregion Mocks
    }
}