using System;

namespace Genesis.DAWG.SimpleUsage
{
    class Element
    {
        /// <summary>
        /// имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// фамилия
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="name"> имя </param>
        /// <param name="surname"> фамилия </param>
        public Element(string name, string surname)
        {
            this.Name = name;
            this.Surname = surname;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // данные
            var data = new (string, Element)[]
            {
                ("aaa", new Element("Петя", "Морковкин")),
                ("aba", new Element("Вася", "Пупкин")),
                ("abb", new Element("Егор", "Васильев")),
            };

            // строим граф
            var builder = new DawgBuilder<Element>();
            foreach (var (key, value) in data)
            {
                builder.Add(key, value);
            }
        }
    }
}
