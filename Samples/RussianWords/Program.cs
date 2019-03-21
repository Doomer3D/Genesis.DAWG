using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml;

namespace RussianWords
{
    class Program
    {
        static void Main(string[] args)
        {
            // конвертируем словарь OpenCorpora в список слов
            string inputFileName = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "dict.opcorpora.xml"));
            string outputFileName = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "words.gz"));

            if (!File.Exists(outputFileName))
            {
                if (!File.Exists(inputFileName))
                {
                    Console.WriteLine("Ошибка: отсутствует файл со словарем OpenCorpora");
                    Console.WriteLine("Скачайте его по ссылке: http://opencorpora.org/?page=downloads (файл dict.opcorpora.xml)");
                    return;
                }
                ConvertOpenCorporaDictionary(inputFileName, outputFileName);
            }

            // загружаем список слов
            var words = new List<string>();
            using (var stream = File.Open(outputFileName, FileMode.Open, FileAccess.Read))
            {
                using (var gzipStream = new GZipStream(stream, CompressionMode.Decompress))
                {
                    using (var reader = new StreamReader(gzipStream, Encoding.UTF8))
                    {
#if DEBUG
                        int count = 0;
#endif
                        for (string line; !reader.EndOfStream;)
                        {
                            line = reader.ReadLine();
                            if (line.Length != 0)
                            {
                                words.Add(line);
#if DEBUG
                                if (++count >= 10) break;
#endif
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// конвертировать словарь OpenCorpora в текстовый вид
        /// </summary>
        /// <param name="inputFileName"> имя входного файла </param>
        /// <param name="outputFileName"> имя выходного файла </param>
        private static void ConvertOpenCorporaDictionary(string inputFileName, string outputFileName)
        {
            var words = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            // читаем слова из XML
            using (var stream = File.Open(inputFileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = XmlReader.Create(stream))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.HasAttributes)
                        {
                            while (reader.MoveToNextAttribute())
                            {
                                if (reader.Name == "t")
                                {
                                    words.Add(reader.Value);
                                }
                            }
                        }
                    }
                }
            }

            // записываем слова в выходной файл
            using (var stream = File.Open(outputFileName, FileMode.Create, FileAccess.Write))
            {
                using (var gzipStream = new GZipStream(stream, CompressionLevel.Optimal))
                {
                    using (var writer = new StreamWriter(gzipStream, Encoding.UTF8))
                    {
                        foreach (var word in words.OrderBy(e => e))
                        {
                            writer.WriteLine(word);
                        }
                    }
                }
            }
        }
    }
}
