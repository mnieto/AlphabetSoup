using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlphabetSoup.Core {
    internal class BoundariesManager : IBoundariesManager {
        public Soup Soup { get; set; }
        private ISoupFactory SoupFactory { get; set; }
        private ILogger<BoundariesManager> Logger { get; set; }

        public BoundariesManager(ILogger<BoundariesManager> logger) {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool Check(WordEntry entry) {
            var size = new Point(Soup.Matrix.GetUpperBound(0), Soup.Matrix.GetUpperBound(1));
            var origin = entry.AbsoluteOrigin();
            var ending = entry.AbsoluteEnding();
            bool haveHorizontalSpace = origin.X >= 0 && ending.X <= size.X;
            bool haveVerticalSpace = origin.Y >= 0 && ending.Y <= size.Y;

            return haveHorizontalSpace && haveVerticalSpace;
        }

        /// <summary>
        /// Computes the necessary delta to move the <see cref="WordEntry"/> inside the boundaries
        /// </summary>
        /// <param name="entry"><see cref="WordEntry"/> to test</param>
        /// <returns>Delta</returns>
        public Point GetDelta(WordEntry entry) {
            var size = new Point(Soup.Matrix.GetUpperBound(0), Soup.Matrix.GetUpperBound(1));
            var origin = entry.Origin;
            var ending = entry.EndingCoordinate();
            var result = new Point();
            if (origin.X < 0)
                result.X = origin.X * -1;
            if (origin.Y < 0)
                result.Y = origin.Y * -1;
            if (ending.X > size.X)
                result.X = size.X - ending.X;
            if (ending.Y > size.Y)
                result.Y = size.Y - ending.Y;

            return result;
        }

    }
}
