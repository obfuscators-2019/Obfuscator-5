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
            var dni = Ofuscator.Domain.DniNie.GenerateDNI();
            Assert.AreEqual(9, dni.Length);
        }

        [Test]
        public void GeneratingANieReturnsA10LengthString()
        {
            var nie = Ofuscator.Domain.DniNie.GenerateNIE();
            Assert.AreEqual(9, nie.Length);
        }

        [Test]
        public void GeneratingANIFReturnsA10LengthString()
        {
            var dniOrNie = Ofuscator.Domain.DniNie.GenerateNIF();            
            Assert.AreEqual(9, dniOrNie.Length);
        }

        [Test]
        public void Generating10NifsReturns10DifferentStrings()
        {
            var expectedResults = 20;

            var nifsGenerated = Ofuscator.Domain.DniNie.GenerateNIF(expectedResults);

            var differentStrings = nifsGenerated.Count;
            Assert.AreEqual(expectedResults, differentStrings);
        }
    }
}