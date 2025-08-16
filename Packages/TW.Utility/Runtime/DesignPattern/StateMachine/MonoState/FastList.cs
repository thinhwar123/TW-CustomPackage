using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DesignPattern.StateMachine.MonoState
{
    [StructLayout(LayoutKind.Auto)]
    public struct FastList<T>
    {
        const int InitialCapacity = 8;

        public static readonly FastList<T> Empty = default;

        private T[] array;
        private int tailIndex;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T element)
        {
            if (array == null)
            {
                array = new T[InitialCapacity];
            }
            else if (array.Length == tailIndex)
            {
                Array.Resize(ref array, tailIndex * 2);
            }

            array[tailIndex] = element;
            tailIndex++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveAtSwapBack(int index)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            CheckIndex(index);

            array[index] = array[tailIndex - 1];
            array[tailIndex - 1] = default;
            tailIndex--;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear(bool removeArray = false)
        {
            if (array == null) return;

            array.AsSpan().Clear();
            tailIndex = 0;
            if (removeArray) array = null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnsureCapacity(int capacity)
        {
            if (array == null)
            {
                array = new T[InitialCapacity];
            }

            while (array.Length < capacity)
            {
                Array.Resize(ref array, array.Length * 2);
            }
        }

        public readonly T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => array[index];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => array[index] = value;
        }

        public readonly int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => tailIndex;
        }

        public readonly Span<T> AsSpan() => array == null ? Span<T>.Empty : array.AsSpan(0, tailIndex);
        public readonly T[] AsArray() => array;

        readonly void CheckIndex(int index)
        {
            if (index < 0 || index > tailIndex) throw new IndexOutOfRangeException();
        }

        public bool Contains(T element)
        {
            if (array == null) return false;

            for (int i = 0; i < tailIndex; i++)
            {
                if (EqualityComparer<T>.Default.Equals(array[i], element))
                {
                    return true;
                }
            }

            return false;
        }

        public void Remove(T element)
        {
            if (array == null) return;

            for (int i = 0; i < tailIndex; i++)
            {
                if (!EqualityComparer<T>.Default.Equals(array[i], element)) continue;
                RemoveAtSwapBack(i);
                return;
            }
        }

        public void Insert(int i, T element)
        {
            if (array == null)
            {
                array = new T[InitialCapacity];
            }
            else if (array.Length == tailIndex)
            {
                Array.Resize(ref array, tailIndex * 2);
            }

            CheckIndex(i);

            for (int j = tailIndex; j > i; j--)
            {
                array[j] = array[j - 1];
            }

            array[i] = element;
            tailIndex++;
        }
    }
}