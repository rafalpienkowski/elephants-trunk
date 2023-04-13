using System.Collections.Generic;
using System.Linq;
using Easy.Common;
using FluentAssertions;
using Trunk.Logic.Analysis;
using Trunk.Logic.Dimensions.Frequencies;
using Xunit;

namespace Trunk.Logic.Tests.Analysis;

public class KnowledgeMapAnalyzerTests
{
    [Fact]
    public void Should_calculate_knowledge_map_for_a_single_author_for_single_file()
    {
        var singleAuthor = CreateAuthorWithFile("raf", "src/Lib/File.cs", 5);
        var authorsCodeLinesAdded = new List<AuthorsCodeLinesAdded> { singleAuthor };
      
        var knowledgeMap = KnowledgeMapAnalyzer.CalculateKnowledgeMap(authorsCodeLinesAdded);

        knowledgeMap.Count.Should().Be(1);
        var firstNode = knowledgeMap.Single();
        AssertKnowledgeNode(firstNode, "raf", "src/Lib", 5);
    }

    [Fact]
    public void Should_calculate_knowledge_map_for_a_single_author_for_multiple_files_in_different_dirs()
    {
        var singleAuthor = CreateAuthorWithFile("raf", "src/Lib/File.cs", 5);
        singleAuthor.AddLines("raf", "src/Api/Api.cs", 100);
        var authorsCodeLinesAdded = new List<AuthorsCodeLinesAdded> { singleAuthor };
      
        var knowledgeMap = KnowledgeMapAnalyzer.CalculateKnowledgeMap(authorsCodeLinesAdded);
        
        knowledgeMap.Count.Should().Be(2);
        var firstNode = knowledgeMap.OrderBy(km => km.NumberOfLines).Take(1).Single();
        var secondNode = knowledgeMap.OrderBy(km => km.NumberOfLines).Skip(1).Take(1).Single();
        AssertKnowledgeNode(firstNode, "raf", "src/Lib", 5);
        AssertKnowledgeNode(secondNode, "raf", "src/Api", 100);
    }
    
    [Fact]
    public void Should_calculate_knowledge_map_for_a_single_author_for_multiple_files_in_the_same_dir()
    {
        var singleAuthor = CreateAuthorWithFile("raf", "src/Lib/File.cs", 5);
        singleAuthor.AddLines("raf", "src/Lib/AnotherFile.cs", 100);
        var authorsCodeLinesAdded = new List<AuthorsCodeLinesAdded> { singleAuthor };
      
        var knowledgeMap = KnowledgeMapAnalyzer.CalculateKnowledgeMap(authorsCodeLinesAdded);
        
        knowledgeMap.Count.Should().Be(1);
        var firstNode = knowledgeMap.Single();
        AssertKnowledgeNode(firstNode, "raf", "src/Lib", 105);
    }

    [Fact]
    public void Should_calculate_knowledge_map_for_multiple_authors_for_single_file()
    {
        var firstAuthor = CreateAuthorWithFile("raf", "src/Lib/File.cs", 5);
        var secondAuthor = CreateAuthorWithFile("tom", "src/Lib/File.cs", 55);
        var authorsCodeLinesAdded = new List<AuthorsCodeLinesAdded> { firstAuthor, secondAuthor };
      
        var knowledgeMap = KnowledgeMapAnalyzer.CalculateKnowledgeMap(authorsCodeLinesAdded);
        
        knowledgeMap.Count.Should().Be(1);
        var firstNode = knowledgeMap.Single();
        AssertKnowledgeNode(firstNode, "tom", "src/Lib", 55);
    }
    
    [Fact]
    public void Should_calculate_knowledge_map_for_multiple_authors_for_single_file_with_first_wins_approach()
    {
        var firstAuthor = CreateAuthorWithFile("raf", "src/Lib/File.cs", 5);
        var secondAuthor = CreateAuthorWithFile("tom", "src/Lib/File.cs", 5);
        var authorsCodeLinesAdded = new List<AuthorsCodeLinesAdded> { firstAuthor, secondAuthor };
      
        var knowledgeMap = KnowledgeMapAnalyzer.CalculateKnowledgeMap(authorsCodeLinesAdded);
        
        knowledgeMap.Count.Should().Be(1);
        var firstNode = knowledgeMap.Single();
        AssertKnowledgeNode(firstNode, "raf", "src/Lib", 5);
    }
    
    [Fact]
    public void Should_calculate_knowledge_map_for_multiple_authors_for_multiple_files()
    {
        var firstAuthor = CreateAuthorWithFile("raf", "src/Lib/File.cs", 5);
        var secondAuthor = CreateAuthorWithFile("tom", "src/Lib/AnotherFile.cs", 55);
        var authorsCodeLinesAdded = new List<AuthorsCodeLinesAdded> { firstAuthor, secondAuthor };
      
        var knowledgeMap = KnowledgeMapAnalyzer.CalculateKnowledgeMap(authorsCodeLinesAdded);
        
        knowledgeMap.Count.Should().Be(1);
        var firstNode = knowledgeMap.Single();
        AssertKnowledgeNode(firstNode, "tom", "src/Lib", 55);
    }
    
    [Fact]
    public void Should_calculate_knowledge_map_for_multiple_authors_for_multiple_files_in_different_dirs()
    {
        var firstAuthor = CreateAuthorWithFile("raf", "src/Lib/File.cs", 5);
        var secondAuthor = CreateAuthorWithFile("tom", "src/Api/AnotherFile.cs", 66);
        var authorsCodeLinesAdded = new List<AuthorsCodeLinesAdded> { firstAuthor, secondAuthor };
           
        var knowledgeMap = KnowledgeMapAnalyzer.CalculateKnowledgeMap(authorsCodeLinesAdded);
             
        knowledgeMap.Count.Should().Be(2);
        var firstNode = knowledgeMap.OrderBy(km => km.NumberOfLines).Take(1).Single();
        var secondNode = knowledgeMap.OrderBy(km => km.NumberOfLines).Skip(1).Take(1).Single();
        AssertKnowledgeNode(firstNode, "raf", "src/Lib", 5);
        AssertKnowledgeNode(secondNode, "tom", "src/Api", 66);
    }
    
    private static void AssertKnowledgeNode(KnowledgeNode node, string expectedAuthor, string expectedPath, long expectedLines)
    {
        node.Should().NotBeNull();
        node.Author.Should().Be(expectedAuthor);
        node.NumberOfLines.Should().Be(expectedLines);
        node.Directory.Should().Be(expectedPath);
    }
    
    
    private static AuthorsCodeLinesAdded CreateAuthorWithFile(string author, string file, long lines)
    {
        var singleAuthor = AuthorsCodeLinesAdded.From(author);
        singleAuthor.AddLines(author, file, lines);
        return singleAuthor;
    }
}