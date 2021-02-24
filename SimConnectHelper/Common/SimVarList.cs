using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimConnectHelper.Common
{
    public class SimVarList : IDictionary<string, string>
    {
        public string this[string key]
        {
            get
            {
                return SimVarUnits.DefaultUnits[key].DefaultUnit;
            }
            set { }
        }

        private KeyValuePair<string, string>[] values = SimVarUnits.DefaultUnits.Select(x => new KeyValuePair<string, string>(x.Key, x.Value.DefaultUnit)).OrderBy(x => x.Key).ToArray();

        public ICollection<string> Keys => (ICollection<string>)values.Select(x=> x.Key);

        public ICollection<string> Values => (ICollection<string>)values.Select(x => x.Value);

        public int Count => values.Length;

        public bool IsReadOnly => true;

        public void Add(string key, string value)
        {
            throw new NotImplementedException();
        }

        public void Add(KeyValuePair<string, string> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            return values.Any(x => x.Key == item.Key && x.Value == item.Value);
        }

        public bool ContainsKey(string key)
        {
            return values.Any(x => x.Key == key);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            for (var i = 0; i < array.Length; i++)
                array[i] = values[arrayIndex + i];
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return (IEnumerator<KeyValuePair<string, string>>)values.GetEnumerator();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out string value)
        {
            if (values.Any(x => x.Key == key))
            {
                value = values.First(x => x.Key == key).Value;
                return true;
            }
            value = null;
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return values.GetEnumerator();
        }
    }
}
