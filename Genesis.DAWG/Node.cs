using System;
using System.Collections.Generic;

namespace Genesis.DAWG
{
    /// <summary>
    /// вершина дерева
    /// </summary>
    /// <typeparam name="T"> тип элемента </typeparam>
    public sealed class Node<T>
    {
        /// <summary>
        /// словарь связей
        /// </summary>
        private Dictionary<char, Node<T>> children;

        /// <summary>
        /// значение элемента
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// указывает, что значение отличается от значения по умолчанию
        /// </summary>
        public bool HasValue => _equalityComparer.Equals(Value, default);

        /// <summary>
        /// получить вершину или создать новую
        /// </summary>
        /// <param name="key"> ключ </param>
        /// <returns></returns>
        public Node<T> GetOrAddEdge(char key)
        {
            // создаем словарь, если он еще не был создан
            if (children == default) children = new Dictionary<char, Node<T>>();

            if (!children.TryGetValue(key, out var node))
            {
                children.Add(key, node = new Node<T>());
            }

            return node;
        }

        internal Node<T> GetChild(char c)
        {
            // TODO
            throw new NotImplementedException();
        }

        #region Static

        /// <summary>
        /// класс сравнения
        /// </summary>
        private static readonly EqualityComparer<T> _equalityComparer = EqualityComparer<T>.Default;

        #endregion
    }
}
