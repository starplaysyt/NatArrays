# NatLib — idk what exactly it is for C#

That is a benchmark test for struct operations. Only for Unit2 and composers of Unit2. Why I placed it here? 'Cos I want to see on what i wasted all day!

Oh, you just need to look on `Mean` column and divide it on 1.000.000+- to get time for one operation.
| Method                      | Mean       | Error   | StdDev  | Allocated |
|---------------------------- |-----------:|--------:|--------:|----------:|
| Unit2_ReadProperty          |   216.5 us | 0.27 us | 0.21 us |         - |
| Unit2_WriteProperty         |   216.4 us | 0.16 us | 0.15 us |         - |
| ComposedUnit2_ReadProperty  |   216.3 us | 0.07 us | 0.06 us |         - |
| ComposedUnit2_WriteProperty |   216.4 us | 0.09 us | 0.08 us |         - |
| Unit2_Create                | 1,297.5 us | 0.30 us | 0.28 us |         - |
| Composed_Create             | 1,297.3 us | 0.34 us | 0.32 us |         - |
| Unit2ToComposed             | 1,081.1 us | 0.32 us | 0.27 us |         - |
| ComposedToUnit2             | 1,081.2 us | 0.25 us | 0.23 us |         - |
| Unit2_ObjectArithmetic      | 1,081.3 us | 0.39 us | 0.34 us |         - |
| Composed_ObjectArithmetic   | 1,081.3 us | 0.43 us | 0.41 us |         - |
| Unit2_TArithmetic           | 1,303.0 us | 6.53 us | 6.11 us |         - |
| Composed_TArithmetic        | 1,302.4 us | 3.95 us | 3.50 us |         - |
| Unit2_IEquatable            |   542.1 us | 0.82 us | 0.64 us |         - |
| Composed_IEquatable         |   541.7 us | 0.37 us | 0.33 us |         - |
| Unit2_Equals                |   657.0 us | 3.65 us | 3.41 us |         - |
| Composed_Equals             |   651.1 us | 1.28 us | 1.14 us |         - |
| Unit2_Existing              | 2,591.8 us | 0.78 us | 0.65 us |         - |
| Composed_Existing           | 2,591.6 us | 0.39 us | 0.37 us |         - |

Currently unused stuff:

A **high-performance, unmanaged array** implementation for C# that allows direct control over memory allocation, reallocation, and deallocation using `NativeMemory`.  
Designed for systems programming, game engines, and any low-level environment where performance and control matter more than safety.

---

## Features

- **Manual memory management** — full control over allocation and freeing  
-  **True unmanaged array** — stored outside the CLR heap  
-  **Maximum performance** — no GC overhead, ideal for tight loops  
-  **Optional initialization** — choose whether to run constructors for your structs  
-  **Safe API boundaries** — explicit `Allocate()`, `Reallocate()`, and `Deallocate()` calls  
-  **Span view support** — lightweight view with pointer access

---

## Concept

Unlike regular C# arrays, `NatArrays` does **not** live on the managed heap.  
You control exactly when and how memory is allocated and released — similar to `malloc`, `realloc`, and `free` in C/C++.

> This makes it perfect for performance-critical code, native interop, graphics, ECS systems, or custom memory pools.

---

## Initialization Control

You decide whether constructors for your structs are called.

```csharp
// Allocates memory only (uninitialized)
array.Allocate(256);

// Allocates memory and calls 'new T()' for each element
array.AllocateDefault(256);
```

You can also reallocate an existing array while preserving old values:

```csharp
array.Reallocate(512);         // Resizes raw memory
array.ReallocateDefault(512);  // Resizes and calls constructors for new slots
```

---

## Safety Notes

- These arrays are unsafe — you are responsible for correct usage.
- Never use spans or pointers after a Reallocate() or Deallocate() call.
- **Always call Dispose()** or use a using block to avoid memory leaks.

---

## Requirements

- **.NET 8.0** or newer (currently built for .NET 9)
- unsafe context enabled in your project

**How to?**
```xml
<PropertyGroup>
  <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
</PropertyGroup>
```
(when you look for such lowlevel lib, you probably know already how to enable unsafe context in project. Ask LLM if not)) )

---

## Graph
Here is a simple graph that shows how it all works.

(idk how that might help, but i'll just leave it here, it looks cool)

```mermaid
graph LR;
  A[Allocate] --> B[Use Pointer/Span]
  B --> C[Reallocate]
  B --> D[Deallocate]
  C --> B
  D --> X[Disposed]
```

---

## License

```
MIT License with Attribution Requirement

Copyright (c) 2025 starplaysyt
https://github.com/starplaysyt/NatArrays

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the “Software”), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, subject to the following conditions:

1. Attribution is required.
   Any redistributions of this software, in source or binary forms, must
   include a prominent attribution to the original author and repository URL
   (for example, in documentation, “About” dialogs, or credits).

2. The above copyright notice and this permission notice shall be included in
   all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
```
