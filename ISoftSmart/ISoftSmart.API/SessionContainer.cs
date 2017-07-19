using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace ISoftSmart.API
{
    public class SessionContainer : IDictionary<string, SessionState>
    {

        private static volatile Dictionary<string, SessionState> _sessionContainer = new Dictionary<string, SessionState>();

        public static Timer Tm { get; } = new Timer(sessionContainer =>
        {
            var sessions = (Dictionary<string, SessionState>)sessionContainer;
            var abandon = (from item in sessions where item.Value.CreateTime.AddMilliseconds(item.Value.TimeOut) < DateTime.Now select item.Key).ToList();
            lock (abandon)
            {
                abandon.ForEach(a => { sessions.Remove(a); });
            }
        }, _sessionContainer, 0, 1000 * 60 * 20);

        public static int TimeOut { get; set; } = 1000 * 60 * 20;

        public SessionState this[string key]
        {
            get
            {
                return ((IDictionary<string, SessionState>)_sessionContainer)[key];
            }

            set
            {
                ((IDictionary<string, SessionState>)_sessionContainer)[key] = value;
            }
        }

        public int Count => ((IDictionary<string, SessionState>)_sessionContainer).Count;

        public bool IsReadOnly => ((IDictionary<string, SessionState>)_sessionContainer).IsReadOnly;

        public ICollection<string> Keys => ((IDictionary<string, SessionState>)_sessionContainer).Keys;

        public ICollection<SessionState> Values => ((IDictionary<string, SessionState>)_sessionContainer).Values;

        public void Add(KeyValuePair<string, SessionState> item)
        {
            ((IDictionary<string, SessionState>)_sessionContainer).Add(item);
        }

        public void Add(string key, SessionState value)
        {
            ((IDictionary<string, SessionState>)_sessionContainer).Add(key, value);
        }

        public void Clear()
        {
            ((IDictionary<string, SessionState>)_sessionContainer).Clear();
        }

        public bool Contains(KeyValuePair<string, SessionState> item)
        {
            return ((IDictionary<string, SessionState>)_sessionContainer).Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return ((IDictionary<string, SessionState>)_sessionContainer).ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, SessionState>[] array, int arrayIndex)
        {
            ((IDictionary<string, SessionState>)_sessionContainer).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, SessionState>> GetEnumerator()
        {
            return ((IDictionary<string, SessionState>)_sessionContainer).GetEnumerator();
        }

        public bool Remove(KeyValuePair<string, SessionState> item)
        {
            return ((IDictionary<string, SessionState>)_sessionContainer).Remove(item);
        }

        public bool Remove(string key)
        {
            return ((IDictionary<string, SessionState>)_sessionContainer).Remove(key);
        }

        public bool TryGetValue(string key, out SessionState value)
        {
            return ((IDictionary<string, SessionState>)_sessionContainer).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<string, SessionState>)_sessionContainer).GetEnumerator();
        }
    }
}