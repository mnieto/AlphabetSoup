using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Abstractions;

namespace AlphabetSoup.Core {

    /// <summary>
    /// Responsible of read configuration files dependent of the selected language
    /// </summary>
    public class ConfigurationManager {

        private const string DataDirectory = "Data";
        private IFileSystem FileSystem { get; set; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileSystem"><see cref="IFileSystem"/> implementation to access to files</param>
        public ConfigurationManager(IFileSystem fileSystem) {
            FileSystem = fileSystem;
        }

        /// <summary>
        /// Select the corresponding data files, load them and returns a initialized <see cref="LanguageData"/>
        /// </summary>
        /// <param name="cultureCode">Language as specified in <see cref="System.Globalization.CultureInfo"/></param>
        /// <param name="useLemmata"><c>true</c> if the lemmata (words) should be read from data file. <c>false</c> if a list of words is provided from <see cref="Options.Words"/></param>
        /// <returns></returns>
        public LanguageData ReadLanguageData(string cultureCode, bool useLemmata) {

            (string code, string path) info = FindFile(cultureCode);
            IEnumerable<string> lines = FileSystem.File.ReadLines(info.path);
            LanguageData data = new LanguageData();
            data.Code = info.code;
            data.Letters = lines.First().ToCharArray();
            if (useLemmata) {
                data.Lemmata = lines.Skip(1).ToList();
            }
            return data;
        }

        /// <summary>
        /// Find a file in form en-us or if not found, in form of en
        /// </summary>
        /// <param name="cultureCode"></param>
        /// <returns>tuple with real used code and file path to be used</returns>
        /// <exception cref="FileNotFound">If a data file corresponding with the language is not found</exception>
        protected (string, string) FindFile(string cultureCode) {
            string code = cultureCode;
            do {
                string path = $"{DataDirectory}{FileSystem.Path.DirectorySeparatorChar}Dictionary.{code}.txt";
                if (FileSystem.File.Exists(path)) {
                    return (code, path);
                } else if (code.Length > 2) {
                    code = cultureCode.Substring(0, 2);
                } else {
                    throw new System.IO.FileNotFoundException($"Data configuration file corresponding to {cultureCode} not found");
                }
            } while (true);
        }

    }
}
