using System;

namespace NatArrays.Debug;

public class Program
{
    public static void Main(string[] args)
    {
        var matrix = new PointerLinMatrix<int>();
        
        matrix.Allocate(10000,10000);

        for (var i = 0; i < matrix.Width; i++)
        {
            for (var j = 0; j < matrix.Height; j++)
            {
                matrix[i,j] = i * j;
            }
        }

        // for (int i = 0; i < matrix.Height; i++)
        // {
        //     for (int j = 0; j < matrix.Width; j++)
        //     {
        //         Console.Write(matrix[i, j] + " ");
        //     }
        //     Console.WriteLine();
        // }

        Console.WriteLine("Filled...");
        Console.ReadKey();
        
        matrix.Deallocate();

        Console.ReadKey();
    }
}