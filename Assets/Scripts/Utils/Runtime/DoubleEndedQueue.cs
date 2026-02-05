using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

[BurstCompile, Serializable]
public class DoubleEndedQueue<T> : IEnumerable<int>
{
    [SerializeField] T[] _queue;
    [SerializeField] int _offset;
    [SerializeField] int _size;
    [SerializeField] int _count;
    
    private Strategy _strategy; 
    public enum Strategy
    {
        FrontToBack,
        BackToFront
    }

    

    public DoubleEndedQueue(int size = 4)
    {
        _size = size;
        _queue = new T[_size];
        _offset = -1;
        _count = 0;
        _strategy = Strategy.FrontToBack;
    }
 
    public int Count => _count;
    
    public void EnqueueFront(T obj)
    {
        if(_count == _size) IncreaseSize();

        if (_count == 0)
        {
            EnqueueFirst(obj);
            return;
        }
        
        _offset = (_offset - 1 + _size) % _size;
        _queue[_offset] = obj;
        _count++;
    }

    public void EnqueueBack(T obj)
    {
        if(_count == _size) IncreaseSize();

        if (_count == 0)
        {
            EnqueueFirst(obj);
            return;
        }
        
        _queue[(_offset + _count) % _size] = obj;
        _count++;
    }
    
    public T DequeueFront()
    {
        if (_count == 0) throw new IndexOutOfRangeException("Queue is empty");

        var obj = _queue[_offset];
        _queue[_offset] = default;
        _offset = (_offset + 1) % _size;
        _count--;
        return obj;
    }
    
    public T DequeueBack()
    {
        if (_count == 0) throw new IndexOutOfRangeException("Queue is empty");
        int index = (_offset + _count - 1) % _size;
        var obj = _queue[index];
        _queue[index] = default;
        _count--;
        return obj;
    }

    public T PeekFront()
    {
        if (_count == 0) throw new IndexOutOfRangeException("Queue is empty");
        return _queue[_offset];
    }

    public T PeekBack()
    {
        if (_count == 0) throw new IndexOutOfRangeException("Queue is empty");
        return _queue[(_offset + _count - 1) % _size];
    }

    public T PeekAt(int index)
    {
        if (index >= _count || index < 0) throw new IndexOutOfRangeException("Index out of range");
        
        return _queue[(_offset + index) % _size];
    }

    public void Insert(int index, T obj)
    {
        if (index < 0 || index > _count) throw new IndexOutOfRangeException("Index out of range");
        if (_count == _size) IncreaseSize();
        
        if (index == 0)
        {
            EnqueueFront(obj);
            return;
        }
        
        if (index == _count)
        {
            EnqueueBack(obj);
            return;
        }
        
        if (index <= _count / 2)
        {
            for(int i = 0; i <= index; i++)
            {
                _queue[(_offset + i -1 + _size) % _size] = _queue[(_offset + i) % _size];
            }
            _offset = (_offset - 1 + _size) % _size;
            _queue[(_offset + index) % _size] = obj;
        }
        else
        {
            for(int i = _count - 1; i >= index; i--)
            {
                _queue[(_offset + i + 1) % _size] = _queue[(_offset + i) % _size];
            }
            _queue[(_offset + index) % _size] = obj;
        }

        _count++;
    }

    public void DeleteAt(int index)
    {
        if (index < 0 || index >= _count) throw new IndexOutOfRangeException("Index out of range");
        if (index == 0)
        {
            DequeueFront();
            return;
        }
        
        if (index == _count - 1)
        {
            DequeueBack();
            return;
        }
        
        if (index < _count / 2)
        {
            for(int i = 0; i < index; i++)
            {
                _queue[(_offset + i + 1) % _size] = _queue[(_offset + i + _size) % _size];
            }
            _queue[_offset % _size] = default;
            _offset = (_offset + 1) % _size;
        }
        else
        {
            for(int i = _count - 2; i >= index; i--)
            {
                _queue[(_offset + i) % _size] = _queue[(_offset + i + 1) % _size];
            }
            _queue[(_offset + _count - 1) % _size] = default;
        }
        
        _count--;
    }
    
    public void Delete(T obj)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_queue[(_offset + i) % _size].Equals(obj))
            {
                DeleteAt(i);
                return;
            }
        }
    }
    
    public void DeleteRange(int start, int end)
    {
        if (start < 0 || start >= _count || end < 0 || end >= _count) throw new IndexOutOfRangeException("Index out of range");
        if (start > end) throw new IndexOutOfRangeException("Start index is greater than end index");

        int rangeLength = end - start + 1;

        if (start == 0 && end == _count - 1)
        {
            Clear();
            return;
        }

        if (start == 0)
        {
            _offset = (_offset + rangeLength) % _size;
            _count -= rangeLength;
            return;
        }

        if (end == _count - 1)
        {
            _count -= rangeLength;
            return;
        }

        if (start < _count - end - 1)
        {
            for (int i = start - 1; i >= 0; i--)
            {
                _queue[(_offset + i + rangeLength) % _size] = _queue[(_offset + i) % _size];
            }
            _offset = (_offset + rangeLength) % _size;
        }
        else
        {
            for (int i = end + 1; i < _count; i++)
            {
                _queue[(_offset + i - rangeLength) % _size] = _queue[(_offset + i) % _size];
            }
        }

        _count -= rangeLength;
    }
    

    public void Clear()
    {
        _queue = new T[_size];
        _offset = -1;
        _count = 0;
    }

    
    public override string ToString()
    {
        string str = "";
        for (int i = 0; i < _count; i++)
        {
            str += _queue[(_offset + i) % _size] + " ";
        }
        return str;
    }
    
    public T this[int index] => PeekAt(index);

    private void IncreaseSize()
    {
        T[] newQueue = new T[_size * 2];
        for (int i = 0; i < _count; i++)
        {
            newQueue[i] = _queue[(_offset + i) % _size];
        }

        _queue = newQueue;
        _offset = 0;
        _size *= 2;
    }

    private void EnqueueFirst(T obj)
    {
        _offset = 0;
        _queue[_offset] = obj;
        _count++;
    }
    
    public void Update(int index, T obj)
    {
        if (index < 0 || index >= _count) throw new IndexOutOfRangeException("Index out of range");
        _queue[(_offset + index) % _size] = obj;
    }
    
    public void SetStrategy(Strategy strategy)
    {
        _strategy = strategy;
    }
    
    IEnumerator<int> IEnumerable<int>.GetEnumerator()
    {
        if (_strategy == Strategy.FrontToBack)
        {
            for (int i = 0; i < _count; i++)
            {
                yield return i;
            }
        }
        else
        {
            for (int i = _count - 1; i >= 0; i--)
            {
                yield return i;
            }
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        if (_strategy == Strategy.FrontToBack)
        {
            for (int i = 0; i < _count; i++)
            {
                yield return _queue[(_offset + i) % _size];
            }
        }
        else
        {
            for (int i = _count - 1; i >= 0; i--)
            {
                yield return _queue[(_offset + i) % _size];
            }
        }
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void UpdateFront(T obj)
    {
        _queue[_offset] = obj;
    }
}