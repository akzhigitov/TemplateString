using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace TemplateString.Tests
{
    [TestFixture]
    public class TemplateStringProviderTests
    {
        [Test]
        public void TemplateWithSingleCharSeparatorTest()
        {
            string templateString = "<name>_<code>_<number>";
            string fileName = "File_20_2014";

            var templateStringProvider = new TemplateStringProvider();

            var dictionary = templateStringProvider.Provide(templateString, fileName);

            Assert.AreEqual(3, dictionary.Count);

            DictionaryAssert(dictionary, "name", "File");
            DictionaryAssert(dictionary, "code", "20");
            DictionaryAssert(dictionary, "number", "2014");
        }

        [Test]
        public void TemplateWithoutSeparatorTest()
        {
            string templateString = "<name><code><number>";
            string fileName = "File_20_2014";

            var templateStringProvider = new TemplateStringProvider();

            Assert.Throws<ArgumentException>(
                () => templateStringProvider.Provide(templateString, fileName));
        }

        [Test]
        public void TemplateDoesntMatchNameTest()
        {
            string templateString = "<name>.<code>,<number>";
            string fileName = "File,20.2014";

            var templateStringProvider = new TemplateStringProvider();

            Assert.Throws<ArgumentException>(
                () => templateStringProvider.Provide(templateString, fileName));
        }

        [Test]
        public void TemplateWithOneValueTest()
        {
            string templateString = "<name>";
            string fileName = "File";

            var templateStringProvider = new TemplateStringProvider();

            var dictionary = templateStringProvider.Provide(templateString, fileName);

            DictionaryAssert(dictionary, "name", "File");
        }

        [Test]
        public void TemplateWithMultipleCharSeparatorTest()
        {
            string templateString = "<name>_____<code>,<number>";
            string fileName = "File_____20,2014";

            var templateStringProvider = new TemplateStringProvider();

            var dictionary = templateStringProvider.Provide(templateString, fileName);

            Assert.AreEqual(3, dictionary.Count);

            DictionaryAssert(dictionary, "name", "File");
            DictionaryAssert(dictionary, "code", "20");
            DictionaryAssert(dictionary, "number", "2014");
        }

        private void DictionaryAssert(IReadOnlyDictionary<string, string> dictionary, string key, string expectedValue)
        {
            string actualValue;
            Assert.IsTrue(dictionary.TryGetValue(key, out actualValue));
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
