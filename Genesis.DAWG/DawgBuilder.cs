using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using KEY = System.Collections.Generic.IEnumerable<char>;

namespace Genesis.DAWG
{
    /// <summary>
    /// построитель DAWG
    /// </summary>
    /// <typeparam name="T"> тип элемента </typeparam>
    public class DawgBuilder<T> //: IDictionary<KEY, T>
    {
        /// <summary>
        /// корневой элемент
        /// </summary>
        protected readonly Node<T> root = new Node<T>();

        private readonly List<Node<T>> lastPath = new List<Node<T>>();
        private string lastKey = "";

        /// <summary>
        /// добавить элемент
        /// </summary>
        /// <param name="key"> ключ </param>
        /// <param name="value"> значение </param>
        public virtual void Add(KEY key, T value)
        {
            if (key is string stringKey)
            {
                AddInner(stringKey, value);
            }
            else
            {
                AddInner(root, key, value);
            }
        }

        /// <summary>
        /// попытаться получить значение
        /// </summary>
        /// <param name="key"> ключ </param>
        /// <param name="value"> значение </param>
        /// <returns></returns>
        public bool TryGetValue(KEY key, out T value)
        {
            // начинаем поиск с корневой вершины
            var node = root;

            foreach (char c in key)
            {
                if ((node = node.GetChild(c)) == null)
                {
                    // последовательность не найдена
                    value = default;
                    return false;
                }
            }

            value = node.Value;
            return true;
        }

        /// <summary>
        /// добавить элемент
        /// </summary>
        /// <param name="key"> ключ </param>
        /// <param name="value"> значение </param>
        protected virtual void AddInner(string key, T value)
        {
            int i = 0, len = key.Length;

            for (; i < len && i < lastKey.Length; i++)
            {
                if (key[i] != lastKey[i]) break;
            }

            // обновляем последний ключ и последовательность
            lastPath.RemoveRange(i, lastPath.Count - i);
            lastKey = key;

            var node = i == 0 ? root : lastPath[i - 1];

            for (; i < len; i++)
            {
                node = node.GetOrAddEdge(key[i]);
                lastPath.Add(node);
            }

            node.Value = value;
        }

        /// <summary>
        /// добавить элемент
        /// </summary>
        /// <param name="node"> начальная вершина </param>
        /// <param name="key"> ключ </param>
        /// <param name="value"> значение </param>
        protected virtual void AddInner(Node<T> node, KEY key, T value)
        {
            foreach (char c in key)
            {
                node = node.GetOrAddEdge(c);
            }

            node.Value = value;
        }
    }
}
