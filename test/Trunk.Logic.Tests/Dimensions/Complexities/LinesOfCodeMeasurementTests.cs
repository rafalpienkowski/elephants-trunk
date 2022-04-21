using System;
using System.Linq;
using FluentAssertions;
using Trunk.Logic.Dimensions.Complexities;
using Xunit;

namespace Trunk.Logic.Tests.Dimensions.Complexities;

public class LinesOfCodeMeasurementTests
{
    [Fact]
    public void Count_lines_in_files_in_directory()
    {
        const string dirName = @"./Resources/SubDir";
        var result = LinesOfCodeMeasurement.Measure(dirName);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        var oneLineFile = result.FirstOrDefault(fl => fl.Path == "OneLineFile.txt");
        AsserFileLines("OneLineFile.txt", 1, oneLineFile);
        var sixteenLinesFile = result.FirstOrDefault(fl => fl.Path == "SixteenLinesFile.txt");
        AsserFileLines("SixteenLinesFile.txt", 16, sixteenLinesFile);
    }

    [Fact]
    public void Count_lines_in_files_in_directory_and_subdirectories()
    {
        const string dirName = @"./Resources";
        var result = LinesOfCodeMeasurement.Measure(dirName);

        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        var oneLineFile = result.FirstOrDefault(fl => fl.Path == "SubDir/OneLineFile.txt");
        AsserFileLines("SubDir/OneLineFile.txt", 1, oneLineFile);
        var sixteenLinesFile = result.FirstOrDefault(fl => fl.Path == "SubDir/SixteenLinesFile.txt");
        AsserFileLines("SubDir/SixteenLinesFile.txt", 16, sixteenLinesFile);
        var codeMaatLogFile = result.FirstOrDefault(fl => fl.Path == "code_maat.log");
        AsserFileLines("code_maat.log", 1395, codeMaatLogFile);
    }

    [Fact]
    public void Does_not_count_lines_when_given_directory_not_exists()
    {
        var act = () => { LinesOfCodeMeasurement.Measure("some_random_path"); };

        act.Should().Throw<ArgumentOutOfRangeException>();
    }
    
    private static void AsserFileLines(string filePath, long linesCount, CodeLines? givenFileLines)
    {
        givenFileLines.Should().NotBeNull();
        givenFileLines?.Lines.Should().Be(linesCount);
        givenFileLines?.Path.Should().Be(filePath);
    }
}