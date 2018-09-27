using AlphabetSoup.Core;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace AlpabetSoup.Core.Test {
    public class SoupGeneratorTest {
        [Fact]
        public void MembersAreInitializedAfterConstructorInvoke() {
            var words = new List<string> { "alpha", "beta", "gamma", "delta", "epsilon", "zeta", "eta", "theta", "iota", "kappa", "lambda", "mu", "nu", "xi", "omicron", "pi", "rho", "tau", "upsilon", "phi", "chi", "psi", "omega" };
            var sut = new SoupGenerator(new Options {
                CultureCode = "es-es",
                NumWords = 10,
                Size = 30,
                Words = words
            });
            Assert.Equal(words.Count, sut.Words.Count);
            Assert.Equal(27, sut.Letters.Length);
            Assert.Equal(4, sut.AllowedDirections.Length);
        }


        protected string ReadTestFile(string path) {
            var fullPath = Path.Combine(AppContext.BaseDirectory, "Data", path);
            return File.ReadAllText(fullPath);
        }

    }
}
