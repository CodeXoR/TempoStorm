namespace WordCount.Scripts
{
    /// <summary>
    /// The interface for any instance of a word counter
    /// </summary>
    internal interface IWordCounter
    {
        Task<int> GetWordCount(string word, CancellationTokenSource? cancellationTokenSource = null);
    }
}
