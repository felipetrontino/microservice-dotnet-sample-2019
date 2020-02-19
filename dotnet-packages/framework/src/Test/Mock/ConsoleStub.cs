using System;
using System.IO;

namespace Framework.Test.Mock
{
    public class ConsoleStub
    {
        private readonly StringWriter _textWriter;

        public static ConsoleStub Create(params string[] arg) => new ConsoleStub(arg);

        public ConsoleStub(params string[] args)
        {
            _textWriter = new StringWriter();
            Console.SetOut(_textWriter);

            var xArgs = string.Join(Environment.NewLine, args);
            var textReader = new StringReader(xArgs);

            Console.SetIn(textReader);
        }

        public string GetString()
        {
            return _textWriter.ToString();
        }
    }
}