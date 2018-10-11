using System;
using System.Collections.Generic;
using System.Text;

namespace AlphabetSoup.Core.Test {
    public static class TestDataGenerator {
        internal static List<string> Words = new List<string> { "alpha", "beta", "gamma", "delta", "epsilon", "zeta", "eta", "theta", "iota", "kappa", "lambda", "mu", "nu", "xi", "omicron", "pi", "rho", "tau", "upsilon", "phi", "chi", "psi", "omega" };

        internal static SoupGenerator InitGenerator(Directions allowedDirections = Directions.E, int numWords = 10, int size = 30, List<string> words = null, List<IRule> rules = null) {

            if (numWords > (words ?? Words).Count) {
                throw new ArgumentException($"{nameof(numWords)} must be equal or lower than the number of words in the list");
            }
            return new SoupGenerator(new Options {
                CultureCode = "es-es",
                NumWords = numWords,
                Size = size,
                Words = words ?? Words,
                AllowedDirections = allowedDirections,
                Rules = rules
            });
        }
    }
}
