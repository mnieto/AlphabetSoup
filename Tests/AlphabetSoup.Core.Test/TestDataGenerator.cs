using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AlphabetSoup.Core.Test {

    class InLengthRangeWords : IEnumerable<object[]> {

        public static List<string> Words = new List<string> { "alpha", "beta", "gamma", "delta", "epsilon", "zeta", "eta", "theta", "iota", "kappa", "lambda", "mu", "nu", "xi", "omicron", "pi", "rho", "tau", "upsilon", "phi", "chi", "psi", "omega" };
        public const int min = 4;
        public const int max = 6;
        public IEnumerator<object[]> GetEnumerator() {
            foreach (var item in Words) {
                yield return new object[] { item.Length >= min && item.Length <= max, item };
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    class TestDataGenerator {

        public static List<string> Words = new List<string> { "alpha", "beta", "gamma", "delta", "epsilon", "zeta", "eta", "theta", "iota", "kappa", "lambda", "mu", "nu", "xi", "omicron", "pi", "rho", "tau", "upsilon", "phi", "chi", "psi", "omega" };
        private IBoundariesManager _boundaries;
        private IIntersectionManager _intersections;

        public TestDataGenerator(IBoundariesManager boundaries, IIntersectionManager intersections) {
            _boundaries = boundaries ?? throw new ArgumentNullException(nameof(boundaries));
            _intersections = intersections ?? throw new ArgumentNullException(nameof(boundaries));
        }

        internal SoupGenerator InitGenerator(Directions allowedDirections = Directions.E, int numWords = 10, int size = 30, List<string> words = null, List<IRule> rules = null) {

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
                },
                _intersections,
                _boundaries,
                NullLogger<SoupGenerator>.Instance
            );
        }
    }
}
