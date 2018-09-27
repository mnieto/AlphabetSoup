using System;
using System.Collections.Generic;
using System.Text;

namespace AlphabetSoup.Core
{

    /// <summary>
    /// Language dependent settings
    /// </summary>
    public class LanguageData
    {

        /// <summary>
        /// Language identifier. Should be a <see cref="System.Globalization.CultureInfo" /> 2 letters or 4 letters code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// List of words definition for this language
        /// </summary>
        public List<string> Lemmata{ get; set; }

        /// <summary>
        /// Valid letters for this language
        /// </summary>
        public char[] Letters { get; set; }

    }
}
