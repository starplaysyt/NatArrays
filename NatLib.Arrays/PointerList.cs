using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NatLib.Arrays;

public sealed class PointerList<T> : IDisposable where T : unmanaged
{
    internal unsafe T* Pointer = null;

    public int Length { get; private set; } = 0;

    public int Capacity { get; private set; } = 0;

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
        if (Pointer == null)
        {
            Pointer = (T*)NativeMemory.Alloc((nuint)resultCapacity * (nuint)sizeof(T));
            Capacity = resultCapacity;
            return;
        }

        Pointer = (T*)NativeMemory.Realloc(Pointer, (nuint)resultCapacity * (nuint)sizeof(T));

        Capacity = resultCapacity;
    }

    public PointerList() { }

    public PointerList(int capacity)
    {
        Capacity = capacity;
        Reallocate(capacity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe T UnsafeGet(int index) => Pointer[index];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void UnsafeSet(int index, T value) => Pointer[index] = value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe ref T UnsafeRef(int index) => ref Pointer[index];

    public unsafe Span<T> AsSpan() => new(Pointer, Length);

    public void Reserve(int capacity)
    {
        if (capacity < Capacity)
            throw new InvalidOperationException("Given capacity is less than current one.");

        Reallocate(capacity);
    }

    public void Add(T value)
    {
        if (Length == Capacity)
            Reallocate(Capacity + 20);

        UnsafeSet(Length, value);
        Length++;
    }

    public void Delete(int index)
    {
        if (index < 0 || index >= Length)
            throw new IndexOutOfRangeException();

        var elementsToCopy = Length - index - 1;

        unsafe
        {
            if (elementsToCopy > 0)
            {
                Buffer.MemoryCopy(
                    &Pointer[index + 1],
                    &Pointer[index],
                    (long)elementsToCopy * sizeof(T),
                    (long)elementsToCopy * sizeof(T)
                );
            }
        }

        Length--;

        if (Length == Capacity - 20)
        {
            Console.WriteLine($"REALLOCATING FROM {Capacity} to {Capacity - 20}");
            Reallocate(Capacity - 20);
        }
    }

    public void Clear()
    {
        unsafe
        {
            NativeMemory.Free(Pointer);
            Pointer = null;
        }

        Length = 0;
        Capacity = 0;
    }

    public void Dispose()
    {
        Clear();
    }
}