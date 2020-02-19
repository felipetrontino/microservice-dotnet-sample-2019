using FluentAssertions;
using Framework.Core.Helpers;
using Framework.Test.Mock;
using Xunit;

namespace Framework.Tests.UnitTests.Core.Helpers
{
    public class PromptHelperTest
    {
        [Fact]
        public void GetOption_Valid()
        {
            // arrange
            var prompt = GetPrompt();
            var console = ConsoleStub.Create("1");

            // act
            var result = GetOption<Options>(prompt);

            // assert
            var outString = console.GetString();
            var outStringExpected = GetOutString();
            outString.Should().Be(outStringExpected);

            const Options resultExpected = Options.Opt1;
            result.Should().Be(resultExpected);
        }

        private static T GetOption<T>(string prompt, int? defaultAnswer = null)
            where T : struct
        {
            return PromptHelper.GetOption<T>(prompt, defaultAnswer);
        }

        #region Mocks

        private static string GetPrompt()
        {
            return "Choose: ";
        }

        private static string GetOutString()
        {
            return "0. Opt0\r\n1. Opt1\r\nChoose:  ";
        }

        public enum Options
        {
            Opt0 = 0,

            Opt1 = 1,
        }

        #endregion Mocks
    }
}