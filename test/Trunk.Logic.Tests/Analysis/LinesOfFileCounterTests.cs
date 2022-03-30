using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Trunk.Logic.Analysis;
using Xunit;

namespace Trunk.Logic.Tests.Analysis;

public class LinesOfFileCounterTests
{
    private readonly LinesOfFileCounter _sut = new();

    [Fact]
    public void Count_lines_in_files_in_directory()
    {
        const string dirName = @"./Resources/SubDir";
        var result = LinesOfFileCounter.Count(dirName);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        var oneLineFile = result.FirstOrDefault(fl => fl.Path == $"{dirName}/OneLineFile.txt");
        AsserFileLines($"{dirName}/OneLineFile.txt", 1, oneLineFile);
        var sixteenLinesFile = result.FirstOrDefault(fl => fl.Path == $"{dirName}/SixteenLinesFile.txt");
        AsserFileLines($"{dirName}/SixteenLinesFile.txt", 16, sixteenLinesFile);
    }

    [Fact]
    public void Count_lines_in_files_in_directory_and_subdirectories()
    {
        const string dirName = @"./Resources";
        var result = LinesOfFileCounter.Count(dirName);

        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        var oneLineFile = result.FirstOrDefault(fl => fl.Path == $"{dirName}/SubDir/OneLineFile.txt");
        AsserFileLines($"{dirName}/SubDir/OneLineFile.txt", 1, oneLineFile);
        var sixteenLinesFile = result.FirstOrDefault(fl => fl.Path == $"{dirName}/SubDir/SixteenLinesFile.txt");
        AsserFileLines($"{dirName}/SubDir/SixteenLinesFile.txt", 16, sixteenLinesFile);
        var codeMaatLogFile = result.FirstOrDefault(fl => fl.Path == $"{dirName}/code_maat.log");
        AsserFileLines($"{dirName}/code_maat.log", 1395, codeMaatLogFile);
    }

    [Fact]
    public void Does_not_count_lines_when_given_directory_not_exists()
    {
        var act = () => { LinesOfFileCounter.Count("some_random_path"); };

        act.Should().Throw<ArgumentOutOfRangeException>();
    }
    
    private static void AsserFileLines(string filePath, long linesCount, FileLines? givenFileLines)
    {
        givenFileLines.Should().NotBeNull();
        givenFileLines?.Lines.Should().Be(linesCount);
        givenFileLines?.Path.Should().Be(filePath);
    }
}