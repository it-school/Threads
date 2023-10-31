using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

/// <summary>Вычисление произведения двух матриц с использованием класса System.Diagnostics.Stopwatch для сравнения производительности параллельного цикла и непараллельного цикла. 
/// Т.к. данный пример может создавать большое количество выходных данных, он позволяет перенаправлять выходные данные в файл
/// </summary>
internal class MultiplyMatrices
{
    #region Demo
    public static void Demo()
    {
        // Set up matrices. Use small values to better view result matrix.
        // Increase the counts to see greater speedup in the parallel loop vs. the sequential loop.
        int colCount = 1000;
        int rowCount = 2000;
        int colCount2 = 1200;
        double[,] m1 = InitializeMatrix(rowCount, colCount);
        double[,] m2 = InitializeMatrix(colCount, colCount2);
        double[,] result = new double[rowCount, colCount2];

        // First do the sequential version
        Console.Error.WriteLine("Executing sequential loop...");
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        MultiplyMatricesSequential(m1, m2, result);
        stopwatch.Stop();
        Console.Error.WriteLine("Sequential loop time in milliseconds: {0}", stopwatch.ElapsedMilliseconds);

        OfferToPrint(rowCount, colCount2, result);

        // Reset timer and results matrix
        stopwatch.Reset();
        result = new double[rowCount, colCount2];

        // Do the parallel loop.
        Console.Error.WriteLine("Executing parallel loop...");
        stopwatch.Start();
        MultiplyMatricesParallel(m1, m2, result);
        stopwatch.Stop();
        Console.Error.WriteLine("Parallel loop time in milliseconds: {0}", stopwatch.ElapsedMilliseconds);

        OfferToPrint(rowCount, colCount2, result);

        Console.Error.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }
    #endregion

    #region Helper_Methods
    static double[,] InitializeMatrix(int rows, int cols)
    {
        double[,] matrix = new double[rows, cols];

        Random r = new Random();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                matrix[i, j] = r.Next(100);
            }
        }
        return matrix;
    }

    private static void OfferToPrint(int rowCount, int colCount, double[,] matrix)
    {
        Console.Error.Write("Computation complete. Print results (y/n)? ");
        char c = Console.ReadKey(true).KeyChar;
        Console.Error.WriteLine(c);
        if (char.ToUpperInvariant(c) == 'Y')
        {
            Console.WriteLine();
            for (int x = 0; x < rowCount; x++)
            {
                Console.WriteLine("ROW {0}: ", x);
                for (int y = 0; y < colCount; y++)
                {
                    Console.Write("{0:#.##} ", matrix[x, y]);
                }
                Console.WriteLine();
            }
        }
        else
        { 
            // Save data to file
        }
    }
    #endregion

    #region Sequential_Loop
    static void MultiplyMatricesSequential(double[,] matrixA, double[,] matrixB, double[,] result)
    {
        int matrixACols = matrixA.GetLength(1);
        int matrixBCols = matrixB.GetLength(1);
        int matrixARows = matrixA.GetLength(0);

        for (int i = 0; i < matrixARows; i++)
        {
            for (int j = 0; j < matrixBCols; j++)
            {
                double temp = 0;
                for (int k = 0; k < matrixACols; k++)
                {
                    temp += matrixA[i, k] * matrixB[k, j];
                }
                result[i, j] += temp;
            }
        }
    }
    #endregion

    #region Parallel_Loop
    static void MultiplyMatricesParallel(double[,] matrixA, double[,] matrixB, double[,] result)
    {
        int matrixACols = matrixA.GetLength(1);
        int matrixBCols = matrixB.GetLength(1);
        int matrixARows = matrixA.GetLength(0);

        // A basic matrix multiplication.
        // Parallelize the outer loop to partition the source array by rows.
        Parallel.For(0, matrixARows, i =>
        {
            for (int j = 0; j < matrixBCols; j++)
            {
                double temp = 0;
                for (int k = 0; k < matrixACols; k++)
                {
                    temp += matrixA[i, k] * matrixB[k, j];
                }
                result[i, j] = temp;
            }
        }); // Parallel.For
    }
    #endregion
}