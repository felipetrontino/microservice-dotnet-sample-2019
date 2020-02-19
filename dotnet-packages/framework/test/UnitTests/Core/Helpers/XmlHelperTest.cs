using FluentAssertions;
using Framework.Core.Helpers;
using System;
using System.IO;
using Xunit;

namespace Framework.Tests.UnitTests.Core.Helpers
{
    public class XmlHelperTest
    {
        #region Serialize

        [Fact]
        public void Serialize_Valid()
        {
            // arrange
            var obj = GetObj();

            // act
            var result = Serialize(obj);

            // assert
            var resultExpected = GetXmlSerialized();
            result.Should().Be(resultExpected);
        }

        [Fact]
        public void Serialize_Obj_Null()
        {
            // arrange
            var obj = GetObjNull();

            // act
            var result = Serialize(obj);

            // assert
            result.Should().BeNull();
        }

        #endregion Serialize

        #region XmlSerialize

        [Fact]
        public void XmlSerialize_Valid()
        {
            // arrange
            var obj = GetObj();

            // act
            var result = XmlSerialize(obj);

            // assert
            var resultExpected = GetXmlSerialized();
            result.Should().Be(resultExpected);
        }

        [Fact]
        public void XmlSerialize_Obj_Null()
        {
            // arrange
            var obj = GetObjNull();

            // act
            var result = XmlSerialize(obj);

            // assert
            result.Should().BeNull();
        }

        #endregion XmlSerialize

        #region XmlDeserialize

        [Fact]
        public void XmlDeserialize_Valid()
        {
            // arrange
            var xml = GetXmlSerialized();

            // act
            var result = XmlDeserialize<Test>(xml);

            // assert
            var resultExpected = GetObj();
            result.Should().BeEquivalentTo(resultExpected);
        }

        [Fact]
        public void XmlDeserializee_Xml_Null()
        {
            // arrange
            var xml = GetXmlNull();

            // act
            var result = XmlDeserialize<Test>(xml);

            // assert
            result.Should().BeNull();
        }

        #endregion XmlDeserialize

        #region Deserialize

        [Fact]
        public void Deserialize_Valid()
        {
            // arrange
            var xml = GetXmlSerialized();

            // act
            var result = Deserialize<Test>(xml);

            // assert
            var resultExpected = GetObj();
            result.Should().BeEquivalentTo(resultExpected);
        }

        [Fact]
        public void Deserialize_Xml_Null()
        {
            // arrange
            var xml = GetXmlNull();

            // act
            var result = Deserialize<Test>(xml);

            // assert
            result.Should().BeNull();
        }

        #endregion Deserialize

        #region SerializeInMemory

        [Fact]
        public void SerializeInMemory_Valid()
        {
            // arrange
            var obj = GetObj();

            // act
            var result = SerializeInMemory(obj);

            // assert
            var xResult = GetXmlString(result);
            var xResultExpected = GetXmlString();
            xResult.Should().Be(xResultExpected);
        }

        [Fact]
        public void SerializeInMemory_Obj_Null()
        {
            // arrange
            var obj = GetObjNull();

            // act
            Action result = () => SerializeInMemory(obj);

            // assert
            result.Should().Throw<Exception>();
        }

        #endregion SerializeInMemory

        private static string Serialize<T>(T obj)
        {
            return XmlHelper.Serialize(obj);
        }

        private static string XmlSerialize(object obj)
        {
            return obj.XmlSerialize();
        }

        private static T Deserialize<T>(string xml)
        {
            return XmlHelper.Deserialize<T>(xml);
        }

        private static T XmlDeserialize<T>(string xml)
        {
            return xml.XmlDeserialize<T>();
        }

        private static MemoryStream SerializeInMemory<T>(T obj)
        {
            return XmlHelper.SerializeInMemory(obj);
        }

        #region Mock

        private static Test GetObj()
        {
            return new Test() { Name = nameof(Test) };
        }

        private static Test GetObjNull()
        {
            return null;
        }

        private static string GetXmlNull()
        {
            return null;
        }

        private static string GetXmlString(Stream ms)
        {
            using var reader = new StreamReader(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return reader.ReadToEnd();
        }

        private static string GetXmlString()
        {
            return "<XmlHelperTest.Test xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.datacontract.org/2004/07/Framework.Tests.UnitTests.Core.Helpers\">\r\n\t<Name>Test</Name>\r\n</XmlHelperTest.Test>";
        }

        private static string GetXmlSerialized()
        {
            return "<?xml version=\"1.0\" encoding=\"utf-8\"?><Test xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><Name>Test</Name></Test>";
        }

        public class Test
        {
            public string Name { get; set; }
        }

        #endregion Mock
    }
}