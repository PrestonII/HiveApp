﻿using NUnit.Framework;
using System.Linq;
using System.IO;
using Hive.Domain.Services.Ventilation;

namespace AirChangeTracer.Services.Tests
{
    [TestFixture()]
    public class LookupServiceTests
    {
        private VentilationLookupService VentilationLookupService;

        [SetUp]
        public void Initialize()
        {
            //VentilationLookupService = new VentilationLookupService();
        }

        [Test]
        public void ShouldBeAbleToFindDataFile()
        {
            var path = VentilationLookupService.TypicalDataLocation;

            Assert.IsNotNull(path);
            Assert.IsTrue(path.Contains("Lookup_VentilationTable.csv"));
            Assert.IsTrue(File.Exists(path));
        }

        [Test]
        public void ShouldBeAbleToReadCSVToListOfOccupancyObjects()
        {
            var list = VentilationLookupService.ReadCSVToList(startOfData: 4);

            Assert.IsNotEmpty(list);
        }

        [Test]
        public void ShouldHave_AllIsolationAnteRoom_AsFirstCategory()
        {
            var list = VentilationLookupService.ReadCSVToList( startOfData: 4);

            var first = list.First();
            var expected = "AII - Isolation Anteroom";
            var actual = first.OccupancyCategory;

            Assert.IsNotNull(first);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(0.0, "Art classroom")]
        [TestCase(2.0, "Autopsy Rooms")]
        [TestCase(2.0, "Clean Workroom or Holding")]
        public void ShouldReturnCorrectValueofVentACHForSpaceCategory(double expectedValue, string category)
        {
            var actual = VentilationLookupService.GetOAACHRBasedOnOccupancyCategory(category);

            Assert.AreEqual(expectedValue, actual);
        }

        [TestCase(0.0, "Art classroom")]
        [TestCase(12.0, "Autopsy Rooms")]
        [TestCase(4.0, "Clean Workroom or Holding")]
        public void ShouldReturnCorrectValueofSupplyACHForSpaceCategory(double expectedValue, string category)
        {
            var actual = VentilationLookupService.GetACHRBasedOnOccupancyCategory(category);

            Assert.AreEqual(expectedValue, actual);
        }
    }
}