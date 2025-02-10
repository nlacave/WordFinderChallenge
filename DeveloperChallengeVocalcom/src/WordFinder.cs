namespace DeveloperChallengeVocalcom.src
{
    public class WordFinder
    {
        private readonly IEnumerable<string> matrix;
        private readonly int lengthOfRow;

        //Define the constructor method with the correct matrix arguments.
        public WordFinder(IEnumerable<string> matrix)
        {
            if (matrix.Count() > 64)
            {
                throw new ArgumentException("The matrix cannot have more than 64 rows");
            }

            lengthOfRow = matrix.First().Length;

            if (lengthOfRow > 64)
            {
                throw new ArgumentException("The matrix cannot have more than 64 columns.");
            }

            foreach (var row in matrix)
            {
                if (row.Length != lengthOfRow)
                {
                    throw new ArgumentException("The length of the rows cannot vary between them.");
                }
            }

            this.matrix = matrix;

        }

        public IEnumerable<string> Find(IEnumerable<string> wordstream)
        {
            var wsWithoutRepetition = new HashSet<string>(wordstream);
            Dictionary<string, int> wordsCount = [];

            //Search words by rows
            foreach (var row in matrix)
            {
                foreach (var word in wsWithoutRepetition)
                {
                    if (row.Contains(word, StringComparison.OrdinalIgnoreCase))
                    {
                        wordsCount[word] = wordsCount.TryGetValue(word, out int value) ? value + 1 : 1;
                    }
                }
            }

            //Search words by columns
            for (int col = 0; col < lengthOfRow; col++)
            {
                string wordXColumn = "";

                for (int row = 0; row < matrix.Count(); row++)
                {
                    wordXColumn += matrix.ElementAt(row)[col];
                }

                foreach (var word in wsWithoutRepetition)
                {
                    if (wordXColumn.Contains(word, StringComparison.OrdinalIgnoreCase))
                    {
                        wordsCount[word] = wordsCount.TryGetValue(word, out int value) ? value + 1 : 1;
                    }
                }
            }

            var tenMostRepeatedWords = new HashSet<string>();

            //Word repetitions count
            /*
            Console.WriteLine("Word repetitions count:");
            foreach (var word in wordsCount)
            {
                Console.WriteLine($"{word.Key}: {word.Value}");
            }
            */

            //Filter by the most 10 repeated words
            if (wordsCount.Count > 0)
            {
                tenMostRepeatedWords = wordsCount
                 .OrderByDescending(word => word.Value)
                 .Take(10)
                 .Select(word => word.Key)
                 .ToHashSet();
            }

            return tenMostRepeatedWords;
        }

    }
}