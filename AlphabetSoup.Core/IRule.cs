using System;
using System.Collections.Generic;
using System.Text;

namespace AlphabetSoup.Core {

    /// <summary>
    /// Rule to be satisfied before a <see cref="WordEntry"/> is added to a <see cref="Soup"/>
    /// </summary>
    public interface IRule {

        /// <summary>
        /// Descriptive name of the rule
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns <c>true</c> if <paramref name="entry"/> satisfy the rule; otherwise returns <c>false</c>
        /// </summary>
        /// <param name="soup"><see cref="Soup"/> object where the new <paramref name="entry"/> will be added</param>
        /// <param name="entry"><see cref="WordEntry"/> to be added to the <paramref name="soup"/> if it satisfy the conditions</param>
        /// <returns></returns>
        bool Check(Soup soup, WordEntry entry);
    }
}
