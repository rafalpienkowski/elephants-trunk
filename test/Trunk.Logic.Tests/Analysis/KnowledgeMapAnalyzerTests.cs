using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Trunk.Logic.Analysis;
using Trunk.Logic.Dimensions.Frequencies;
using Xunit;

namespace Trunk.Logic.Tests.Analysis;

public class KnowledgeMapAnalyzerTests
{
    [Fact]
    public void Should_calculate_knowledge_map_for_a_single_author_for_single_dir()
    {
        var authorsCodeLinesAdded = new List<LinesAddedByAuthor>
        {
            LinesAddedByAuthor.From("src/Lib", "raf", 5)
        };
      
        var knowledgeMap = KnowledgeMapAnalyzer.CalculateKnowledgeMap(authorsCodeLinesAdded);

        knowledgeMap.Count.Should().Be(1);
        var firstNode = knowledgeMap.Single();
        AssertKnowledgeNode(firstNode, "raf", "src/Lib", 5);
    }

    [Fact]
    public void Should_calculate_knowledge_map_for_a_single_author_for_multiple_dirs()
    {
        var authorsCodeLinesAdded = new List<LinesAddedByAuthor>
        {
            LinesAddedByAuthor.From("src/Lib", "raf", 5),
            LinesAddedByAuthor.From("src/Api", "raf", 100)
        };
      
        var knowledgeMap = KnowledgeMapAnalyzer.CalculateKnowledgeMap(authorsCodeLinesAdded);
        
        knowledgeMap.Count.Should().Be(2);
        var firstNode = knowledgeMap.OrderBy(km => km.NumberOfLines).Take(1).Single();
        var secondNode = knowledgeMap.OrderBy(km => km.NumberOfLines).Skip(1).Take(1).Single();
        AssertKnowledgeNode(firstNode, "raf", "src/Lib", 5);
        AssertKnowledgeNode(secondNode, "raf", "src/Api", 100);
    }
    

    [Fact]
    public void Should_calculate_knowledge_map_for_multiple_authors_for_single_dir()
    {
        var authorsCodeLinesAdded = new List<LinesAddedByAuthor>
        {
            LinesAddedByAuthor.From("src/Lib", "raf", 5),
            LinesAddedByAuthor.From("src/Lib", "tom", 55)
        };
      
        var knowledgeMap = KnowledgeMapAnalyzer.CalculateKnowledgeMap(authorsCodeLinesAdded);
        
        knowledgeMap.Count.Should().Be(1);
        var firstNode = knowledgeMap.Single();
        AssertKnowledgeNode(firstNode, "tom", "src/Lib", 55);
    }
    
    [Fact]
    public void Should_calculate_knowledge_map_for_multiple_authors_for_single_dir_with_first_wins_approach()
    {
        var authorsCodeLinesAdded = new List<LinesAddedByAuthor>
        {
            LinesAddedByAuthor.From("src/Lib", "raf", 5),
            LinesAddedByAuthor.From("src/Lib", "tom", 5)
        };
      
        var knowledgeMap = KnowledgeMapAnalyzer.CalculateKnowledgeMap(authorsCodeLinesAdded);
        
        knowledgeMap.Count.Should().Be(1);
        var firstNode = knowledgeMap.Single();
        AssertKnowledgeNode(firstNode, "raf", "src/Lib", 5);
    }
    
    [Fact]
    public void Should_calculate_knowledge_map_for_multiple_authors_for_multiple_dirs()
    {
        var authorsCodeLinesAdded = new List<LinesAddedByAuthor>
        {
            LinesAddedByAuthor.From("src/Lib", "raf", 5),
            LinesAddedByAuthor.From("src/Api", "tom", 66)
        };
           
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
}