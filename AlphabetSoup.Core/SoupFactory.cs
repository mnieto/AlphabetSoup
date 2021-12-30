using System;
using System.Collections.Generic;
using System.Text;

namespace AlphabetSoup.Core {

    internal interface ISoupFactory {
        Soup Instance { get;  }
    }

    internal class SoupFactory : ISoupFactory {

        private static Soup _instance;
        private ISoupGenerator _generator;
        private object lockObject = new object();

        public Soup Instance {
            get {
                if (_instance == null) {
                    lock (lockObject) {
                        if (_instance == null) {
                            _instance =_generator.Init()
                                .Create();
                        }
                    }
                }
                return _instance;
            }
        }

        public SoupFactory(ISoupGenerator generator) {
            _generator = generator ?? throw new ArgumentNullException(nameof(generator));
        }

    }
}
