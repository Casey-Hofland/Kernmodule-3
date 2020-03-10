using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robocode;

namespace CaseyDeCoder.BehaviorTree
{
    public class Blackboard
    {
        private Dictionary<string, object> values = new Dictionary<string, object>();

        public T GetValue<T>(string key) => (values.ContainsKey(key) ? (T)values[key] : default);

        // Used for evaluating if a value exists for deciding what needs to happen in case it's missing.
        public bool TryGetValue<T>(string key, out T value)
        {
            value = GetValue<T>(key);
            return values.ContainsKey(key);
        }

        public void SetValue<T>(string key, T value)
        {
            if(values.ContainsKey(key))
                values[key] = value;
            else
                values.Add(key, value);
        }

        public bool RemoveValue(string key) => values.Remove(key);

        //public bool UpdateValue<T>(string key, Func<T> predicate)
    }
}
