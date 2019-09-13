using NUnit.Framework;
using System.Diagnostics;

namespace Tests
{
    public class DniNieTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GeneratingADniReturnsA10LengthString()
        {
            var dni = Obfuscator.Domain.DniNie.GenerateDNI();
            Assert.AreEqual(9, dni.Length);
        }

        [Test]
        public void GeneratingANieReturnsA10LengthString()
        {
            var nie = Obfuscator.Domain.DniNie.GenerateNIE();
            Assert.AreEqual(9, nie.Length);
        }

        [Test]
        public void GeneratingANIFReturnsA10LengthString()
        {
            var dniOrNie = Obfuscator.Domain.DniNie.GenerateNIF();            
            Assert.AreEqual(9, dniOrNie.Length);
        }

        [Test]
        public void Generating10NifsReturns10DifferentStrings()
        {
            var expectedResults = 20;

            var nifsGenerated = Obfuscator.Domain.DniNie.GenerateNIF(expectedResults);

            var differentStrings = nifsGenerated.Count;
            Assert.AreEqual(expectedResults, differentStrings);
        }
    }
}