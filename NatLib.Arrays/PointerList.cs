using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NatLib.Arrays;

public sealed class PointerList<T> : IDisposable where T : unmanaged
{
    private unsafe T* _pointer = null;

    public int Length { get; private set; }

    public int Capacity { get; private set; }

    public T this[int index]
    { get
      { if (index < 0 || index >= Length)
            throw new IndexOutOfRangeException();
          return UnsafeGet(index); }
      set
      { if (index < 0 || index >= Length)
            throw new IndexOutOfRangeException();
          UnsafeSet(index, value); } }

    private unsafe void Reallocate(int resultCapacity)
    {
        if (_pointer == null)
        {
            _pointer = (T*)NativeMemory.Alloc((nuint)resultCapacity * (nuint)sizeof(T));
            Capacity = resultCapacity;
            return;
        }

        _pointer = (T*)NativeMemory.Realloc(_pointer, (nuint)resultCapacity * (nuint)sizeof(T));

        Capacity = resultCapacity;
    }

    public PointerList() { }

    public PointerList(int capacity)
    {
        Reallocate(capacity);
    }
    
    public void TrimExcess()
    {
        if (Length == 0)
        {
            Clear();
            return;
        }

        if (Length < Capacity)
            Reallocate(Length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe T UnsafeGet(int index) => _pointer[index];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void UnsafeSet(int index, T value) => _pointer[index] = value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe ref T UnsafeRef(int index) => ref _pointer[index];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe Span<T> AsSpan() => new(_pointer, Length);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe T* AsPointer() => _pointer;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe T* AsPointer(int index) => &_pointer[index];

    public void Reserve(int capacity)
    {
        if (capacity < Capacity)
            throw new InvalidOperationException("Given capacity is less than current one.");

        Reallocate(capacity);
    }

    public void AddSeveral(Span<T> elements)
    {
        var currentLength = Length;
        if (Length + elements.Length > Capacity)
            Reallocate(Length + elements.Length);

        unsafe
        {
            var span = new Span<T>(&_pointer[Length], elements.Length);
            elements.CopyTo(span);
            
            Length += elements.Length;
        }
    }

    public void Add(T value)
    {
        if (Length == Capacity)
            Reallocate(Math.Max(Capacity * 2, Capacity + 20));

        UnsafeSet(Length, value);
        Length++;
    }

    public void Delete(int index)
    {
        if (index < 0 || index >= Length)
            throw new IndexOutOfRangeException();
        unsafe
        {
            var elementsToCopy = Length - index - 1;
            long elementsToCopySize = elementsToCopy * sizeof(T);
            
            if (elementsToCopy > 0)
            {
                Buffer.MemoryCopy(
                    &_pointer[index + 1],
                    &_pointer[index],
                    elementsToCopySize,
                    elementsToCopySize
                );
            }
        }

        Length--;
    }

    public void Clear()
    {
        unsafe
        {
            NativeMemory.Free(_pointer);
            _pointer = null;
        }

        Length = 0;
        Capacity = 0;
    }

    ~PointerList()
    {
        Clear();
    }

    public void Dispose()
    {
        Clear();
        GC.SuppressFinalize(this);
    }
}