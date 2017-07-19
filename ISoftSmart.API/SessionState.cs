using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISoftSmart.API
{
    public class SessionState : IDictionary<string, object>
    {
        private DateTime _createTime;
        private Dictionary<string, object> _innerSession;

        private string _sessionId;
        private int _timeOut = 20 * 60 * 1000;

        public int TimeOut { get { return _timeOut; } set { _timeOut = value; } }
        public string SessionId { get { return _sessionId; } }
        public DateTime CreateTime { get { return _createTime; } }
        public SessionState()
        {
            _createTime = DateTime.Now;
            _innerSession = new Dictionary<string, object>();
        }
        public SessionState(string sessionId) : this()
        {
            _sessionId = sessionId;
            _timeOut = 20 * 60 * 1000;//默认20分钟
        }
        public SessionState(string sessionId, int timeOut) : this()
        {
            _sessionId = sessionId;
            _timeOut = timeOut > 0 ? timeOut : 20 * 60 * 1000;
        }

        public object this[string key]
        {
            get
            {
                //edit by jxl 20170424 Session[key] 异常 临时解决方案
                if (_innerSession != null && _innerSession.Count > 0)
                    return ((IDictionary<string, object>)_innerSession)[key];
                return null;
            }

            set
            {
                ((IDictionary<string, object>)_innerSession)[key] = value;
            }
        }

        public int Count
        {
            get
            {
                return ((IDictionary<string, object>)_innerSession).Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((IDictionary<string, object>)_innerSession).IsReadOnly;
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                return ((IDictionary<string, object>)_innerSession).Keys;
            }
        }

        public ICollection<object> Values
        {
            get
            {
                return ((IDictionary<string, object>)_innerSession).Values;
            }
        }

        public void Add(KeyValuePair<string, object> item)
        {
            ((IDictionary<string, object>)_innerSession).Add(item);
        }

        public void Add(string key, object value)
        {
            ((IDictionary<string, object>)_innerSession).Add(key, value);
        }

        public void Clear()
        {
            ((IDictionary<string, object>)_innerSession).Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return ((IDictionary<string, object>)_innerSession).Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return ((IDictionary<string, object>)_innerSession).ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            ((IDictionary<string, object>)_innerSession).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return ((IDictionary<string, object>)_innerSession).GetEnumerator();
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return ((IDictionary<string, object>)_innerSession).Remove(item);
        }

        public bool Remove(string key)
        {
            return ((IDictionary<string, object>)_innerSession).Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return ((IDictionary<string, object>)_innerSession).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<string, object>)_innerSession).GetEnumerator();
        }
    }
}