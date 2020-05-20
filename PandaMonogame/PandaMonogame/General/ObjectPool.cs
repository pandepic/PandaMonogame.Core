using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandaMonogame
{
    public class ObjectPool<T> where T : IPoolable, new()
    {
        public const int DefaultPoolSize = 100;

        protected T[] _objects;
        protected bool _disposable = false;

        public T[] Objects { get => _objects; }
        public int Size { get => _objects.Length; }

        public T this[int index]
        {
            get => _objects[index];
        }

        public ObjectPool(int size = DefaultPoolSize)
        {
            _objects = new T[0];

            if (typeof(T) is IDisposable)
                _disposable = true;

            AddObjects(size);
        } // constructor

        ~ObjectPool()
        {
            if (_disposable)
            {
                for (var i = 0; i < Size; i++)
                {
                    ((IDisposable)_objects[i])?.Dispose();
                }
            }
        }

        public void Clear()
        {
            for (var i = 0; i < _objects.Length; i++)
                Delete(i);
        }

        public void Delete(int poolIndex)
        {
            _objects[poolIndex].ObjectAlive = false;
            _objects[poolIndex].Delete();
        }

        public void Delete(T obj)
        {
            Delete(obj.PoolIndex);
        }

        public T New(bool resize = true)
        {
            for (var i = 0; i < Size; i++)
            {
                T obj = _objects[i];
                if (!obj.ObjectAlive)
                {
                    obj.ObjectAlive = true;
                    obj.New();
                    return obj;
                }
            }

            if (resize)
            {
                var currentSize = Size;
                var newSize = currentSize * 2;
                AddObjects(newSize - currentSize);
                return New();
            }

            return default;
        }

        protected void AddObjects(int count)
        {
            var newIndex = Size;

            Array.Resize(ref _objects, Size + count);

            for (var i = 0; i < count; i++)
            {
                var obj = new T();
                obj.PoolIndex = newIndex;
                obj.ObjectAlive = false;
                _objects[newIndex] = obj;

                newIndex++;
            }
        }

        public IEnumerable<T> GetAlive()
        {
            for (var i = 0; i < Size; i++)
            {
                var obj = _objects[i];
                if (obj.ObjectAlive)
                    yield return obj;
            }
        }

        public IEnumerable<T> GetAll()
        {
            for (var i = 0; i < Size; i++)
            {
                yield return _objects[i];
            }
        }

    } // ObjectPool
}
