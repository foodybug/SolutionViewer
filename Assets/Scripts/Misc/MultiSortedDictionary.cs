using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MultiSortedDictionary<Key, Value>
{
    [SerializeField] SortedDictionary<Key, List<Value>> dic_ = null;
 
    public MultiSortedDictionary() 
    {
        dic_ = new SortedDictionary<Key, List<Value>>();
    }
 
    public void Add(Key key, Value value)
    {
        List<Value> list = null;
 
        if(dic_.TryGetValue(key, out list))
        {
            list.Add(value);
        }
        else
        {
            list = new List<Value>();
            list.Add(value);
            dic_.Add(key, list);
        }
    }
 
    public bool ContainsKey(Key key)
    {
        return dic_.ContainsKey(key);
    }
 
    public List<Value> this[Key key]
    {
        get
        {
            List<Value> list = null;
            if (!dic_.TryGetValue(key, out list))
            {
                list = new List<Value>();
                dic_.Add(key, list);
            }
 
            return list;
        }
    }
 
    public IEnumerable keys
    {
        get
        {
            return dic_.Keys;
        }
    }
}