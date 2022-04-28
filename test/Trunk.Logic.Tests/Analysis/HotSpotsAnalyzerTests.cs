using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Trunk.Logic.Analysis;
using Trunk.Logic.Dimensions.Complexities;
using Trunk.Logic.Dimensions.Frequencies;
using Xunit;

namespace Trunk.Logic.Tests.Analysis;

public class HotSpotsAnalyzerTests
{
   [Fact]
   public void Should_calculate_hot_spots_for_a_single_file_from_single_revision()
   {
      const string path = "simple.cs";
      var revisionFrequency = new List<FrequencyOfChanges>
      {
         FrequencyOfChanges.From(path, 321)
      };
      var linesOfCode = new List<CodeLines>
      {
         CodeLines.From(path, 123)
      };

      var hotspots = HotSpotsAnalyzer.CalculateHotSpots(revisionFrequency, linesOfCode);

      hotspots.Count.Should().Be(1);
      var hotSpot = hotspots.FirstOrDefault();
      
      HotSpotShouldBe(hotSpot, path, 321,123);
   }
   
   [Fact]
   public void Should_calculate_hot_spots_for_a_single_file_from_multiple_revisions()
   {
      const string path = "simple.cs";
      var revisionFrequency = new List<FrequencyOfChanges>
      {
         FrequencyOfChanges.From(path, 1),
         FrequencyOfChanges.From(path, 2)
      };
      var linesOfCode = new List<CodeLines>
      {
         CodeLines.From(path, 123)
      };

      var hotspots = HotSpotsAnalyzer.CalculateHotSpots(revisionFrequency, linesOfCode);

      hotspots.Count.Should().Be(1);
      var hotSpot = hotspots.FirstOrDefault();
      
      HotSpotShouldBe(hotSpot, path, 3,123);
   }

   [Fact]
   public void Should_calculate_hot_spots_for_multiple_files_across_multiple_revisions()
   {
      const string path = "simple.cs";
      const string anotherPath = "anotherSimple.cs";
      var revisionFrequency = new List<FrequencyOfChanges>
      {
         FrequencyOfChanges.From(path, 1),
         FrequencyOfChanges.From(anotherPath, 2),
         FrequencyOfChanges.From(path, 3),
         FrequencyOfChanges.From(anotherPath, 4)
      };
      var linesOfCode = new List<CodeLines>
      {
         CodeLines.From(path, 123),
         CodeLines.From(anotherPath, 34)
      };

      var hotspots = HotSpotsAnalyzer.CalculateHotSpots(revisionFrequency, linesOfCode);

      hotspots.Count.Should().Be(2);
      var hotSpot = hotspots.FirstOrDefault(hs => hs.File == path);
      HotSpotShouldBe(hotSpot, path, 4,123);
      var anotherHotSpot = hotspots.FirstOrDefault(hs => hs.File == anotherPath);
      HotSpotShouldBe(anotherHotSpot, anotherPath, 6,34);
   }
   
   private static void HotSpotShouldBe(HotSpot? hotSpot, string fileName, long numberOfChanges, long numberOfLines)
   {
      hotSpot.Should().NotBeNull();
      
      hotSpot?.File.Should().Be(fileName);
      hotSpot?.NumberOfChanges.Should().Be(numberOfChanges);
      hotSpot?.NumberOfLines.Should().Be(numberOfLines);
   }
}