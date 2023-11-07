namespace WordCount
{
    using WordCount.Scripts;

    internal class Program
    {
        static async Task Main()
        {
            /*
             * Programming exercise: 
             * Write a program in the C# programming language that reads in a file containing a list of words. 
             * The program should take a word as input and output the number of times that word appears in the file.
             */

            // using a regular text file with a list of words separated via newline
            // text file is located at relative project directory
            // @"{CurrentApplicationDirectory}\bin\Debug\net6.0\words.txt"

            IWordCounter wordCounter = new WordCounter("words.txt");

            await RunSimpleUnitTest(wordCounter);

            //await RunUnitTestWithCancellation(wordCounter);
        }

        private static async Task RunSimpleUnitTest(IWordCounter wordCounter)
        {
            // Simple Unit Test
            string[] wordsToFind = new string[]
            {
                "2",
                "AAA",
                "word",
                "make",
                "swim",
                "magic",
                "tempo",
                "storm"
            };

            foreach (string word in wordsToFind)
            {
                int numFound = await wordCounter.GetWordCount(word);
                Console.WriteLine($"word - \"{word}\" - count - {numFound}");
            }
        }

        private static async Task RunUnitTestWithCancellation(IWordCounter wordCounter)
        {
            // Unit Test With Cancellation
            Console.WriteLine(Environment.NewLine);
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            string wordToFind = "AAA";
            Task<int> checkWordTask = wordCounter.GetWordCount(wordToFind, tokenSource);
            // if task takes longer than 100 milliseconds to get through the list of words, a call to cancel will 
            // terminate the function early.
            // this will be more obvious if we are to log each line of words in the console with Console.WriteLine
            await Task.Delay(100); // waits 100 milliseconds
            tokenSource.Cancel();
            // prints out count of word at early termination or completion of the function.
            Console.WriteLine($"word - \"{wordToFind}\" - count - {checkWordTask.Result}");
        }
    }
}
