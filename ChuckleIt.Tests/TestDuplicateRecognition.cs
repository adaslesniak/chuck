namespace Downchuckle.Tests;

[TestFixture]
public class TestDuplicateRecognition
{
    DuplicatesReferee testedFunction = ChuckFinder.IsBasicallyTheSame;

    [TestCase("asia", "asia")]
    [TestCase("asia", "Asia")]
    [TestCase("asia", "ASIA")]
    [TestCase("asia", " asia ")]
    [TestCase("Tomasz   Lipski", " tomasz lipski ")]
    public void ShouldFigureOutSimilarStrings(string a, string b) =>
        Assert.True(testedFunction(a, b));


    [TestCase("Roberto", "Rupert")]
    [TestCase("The lazy brown dog jumped over quick fox", "The quick brown fox jumps over the lazy dog.")]
    public void ShouldDiferentiateStrings(string a, string b) =>
        Assert.False(testedFunction(a, b));

}