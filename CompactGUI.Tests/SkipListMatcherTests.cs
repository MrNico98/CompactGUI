using CompactGUI.Core;

namespace CompactGUI.Tests;

public class SkipListMatcherTests
{
    [Fact]
    public void ExcludesFilesByExtensionPattern()
    {
        string root = @"C:\Games\Doom";
        string[] allFiles =
        [
            @"C:\Games\Doom\video.mp4",
            @"C:\Games\Doom\sub\audio.mp3",
            @"C:\Games\Doom\sub\trailer.MP4",
            @"C:\Games\Doom\game.exe"
        ];
        string[] patterns = [".mp4", "*.mp3"];

        var excluded = SkipListMatcher.GetExcludedFiles(root, allFiles, patterns);

        Assert.Contains(@"C:\Games\Doom\video.mp4", excluded);
        Assert.Contains(@"C:\Games\Doom\sub\trailer.MP4", excluded);
        Assert.Contains(@"C:\Games\Doom\sub\audio.mp3", excluded);
        Assert.DoesNotContain(@"C:\Games\Doom\game.exe", excluded);
    }

    [Fact]
    public void ExcludesExactFileNameAtAnyDepth()
    {
        string root = @"C:\Photos";
        string[] allFiles =
        [
            @"C:\Photos\Thumbs.db",
            @"C:\Photos\Album1\thumbs.db",
            @"C:\Photos\photo.jpg"
        ];
        string[] patterns = ["Thumbs.db"];

        var excluded = SkipListMatcher.GetExcludedFiles(root, allFiles, patterns);

        Assert.Contains(@"C:\Photos\Thumbs.db", excluded);
        Assert.Contains(@"C:\Photos\Album1\thumbs.db", excluded);
        Assert.DoesNotContain(@"C:\Photos\photo.jpg", excluded);
    }

    [Fact]
    public void ReturnsEmptyWhenNoPatternsProvided()
    {
        string root = @"C:\Test";
        string[] allFiles = [@"C:\Test\a.txt", @"C:\Test\b.bin"];

        var excluded = SkipListMatcher.GetExcludedFiles(root, allFiles, []);

        Assert.Empty(excluded);
    }

    [Fact]
    public void RelativizesAbsolutePathPatternsInsideRoot()
    {
        string root = @"C:\Test";
        string[] allFiles =
        [
            @"C:\Test\skipme.dat",
            @"C:\Test\keepme.dat"
        ];
        string[] patterns = [@"C:\Test\skipme.dat"];

        var excluded = SkipListMatcher.GetExcludedFiles(root, allFiles, patterns);

        Assert.Contains(@"C:\Test\skipme.dat", excluded);
        Assert.DoesNotContain(@"C:\Test\keepme.dat", excluded);
    }
}
