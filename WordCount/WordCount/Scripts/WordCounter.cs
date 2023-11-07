namespace WordCount.Scripts
{
    /// <summary>
    /// An implementation of a word counter using StreamReader and async/await with cancellation
    /// </summary>
    internal class WordCounter : IWordCounter
    {
        private string _wordListPath;

        public WordCounter(string wordListPath) 
        {
            _wordListPath = wordListPath;
        }

        /// <summary>
        /// Cancellable asynchronous word count task for checking how many times a word is found in a list of words
        /// </summary>
        /// <param name="word">The word to look for</param>
        /// <param name="cancellationTokenSource">optional token source that can be passed to support cancellation</param>
        /// <returns></returns>
        public async Task<int> GetWordCount(string word, CancellationTokenSource? cancellationTokenSource = null)
        {
            /*
             * opted to use a StreamReader instead of reading all the lines into an array (File.ReadLines) 
             * in case the program needs to process a large bank of words
             * with a Stream Reader we are able to save space as it will only hold 1 word's worth of memory at a time
             * in Big O, the space complexity would be O(n) where n = length of the current word
             * this would be better than O(n^2) if we are to cache each unique word and their counts
             * 
             * as for the time complexity, since we aren't caching any of our previously looked up words
             * the program will look into the words list line by line each call to the function.
             * in Big O, the time complexity would be O(n + k) where n = number of words and k = length of word to look for
             * 
             * also as a design choice, I opted to make this function asynchronous for flexibility of integration
             * if for example, the program is to run as a part of another application it can run without 
             * causing a hitch to the main application in case the program needs to look for a word in a rather large bank of words
             * 
             * all of these assumptions are just looking out for the worst case scenarios of course.
             * technically, a large portion of these system architecture questions would be answered in a pre-planning of some sort
             * with a more specific idea of what the data inputs would be or how it would be stored.
             * in most cases, if a program really needs a rather large amount of data to process, a remote database can be deployed 
             * to host such large amounts of data and then it'll be just up to the client to retrieve this data from the server in
             * a way where the data map doesn't need to be loaded into the clients memory at all.
             * 
             * just to point out we could have easily went for a map here with O(1) time complexity for checking 
             * how many times a word was found but since the assumption is in terms of space the entire word bank would be
             * loaded into memory within the client itself, we can save a lot of space with this approach since we will only ever
             * need to look for a single word at a time as part of the requirement.
             */
            int wordCount = 0;

            try
            {
                using (StreamReader reader = new StreamReader(_wordListPath))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        if (string.Equals(word, line, StringComparison.Ordinal))
                        {
                            wordCount++;
                        }

                        // added cancellation in any case this asynchronous function needs to be terminated early before it completes
                        if (cancellationTokenSource != null && cancellationTokenSource.IsCancellationRequested)
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // this will also catch exceptions where the supplied path is invalid
                // opted to not throw an exception here to let function return value
                // gracefully without short-circuiting the application
            }

            return wordCount;
        }
    }
}
