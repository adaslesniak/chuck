using System.Text;
namespace Downchuckle.Tests;

[TestFixture]
public class TestJokesFilter {

    const int lengthLimit = 200;

    [Test]
    public void ShouldPassShortStrings() =>
        Assert.True(ChuckFinder.IsGoodChuck(DumbStringOfLenght(lengthLimit)));


    [Test]
    public void ShouldNotPassTooLongString() =>
        Assert.False(ChuckFinder.IsGoodChuck(DumbStringOfLenght(lengthLimit + 1)));


    string DumbStringOfLenght(int howManyChars) {
        var lottery = new Random(13); //when using random in tests always define seed so results are repeatable
        var scribe = new StringBuilder();
        for(int i = 0; i < howManyChars; i++) {
            var nextChar = (char)lottery.Next(97, 122);
            scribe.Append(nextChar);
        }
        return scribe.ToString();
    }
}
