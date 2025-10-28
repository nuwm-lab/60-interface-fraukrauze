using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOP_Lab3
{
    // Інтерфейс для відображення та роботи з числовою структурою
    public interface IStructure
    {
        void Display();
        double GetMax();
    }

    // Абстрактний базовий клас
    public abstract class NumericStructure : IStructure
    {
        private int _size;
        private double[] _elements;

        public int Size => _size;  // тільки читання
        protected double[] Elements => _elements; // доступ для похідних класів

        public NumericStructure(int size)
        {
            if (size <= 0)
                throw new ArgumentException("Розмір має бути додатнім.");

            _size = size;
            _elements = new double[size];
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine($"[Створено NumericStructure на {_size} елементів]");
        }

        public void SetElements(double[] values)
        {
            if (values == null || values.Length != _size)
                throw new ArgumentException($"Розмір масиву ({values?.Length ?? 0}) не відповідає {_size}.");

            Array.Copy(values, _elements, _size);
        }

        public abstract void Display();
        public virtual double GetMax() => _elements.Length > 0 ? _elements.Max() : double.NaN;

        public double[] ElementsCopy() => (double[])_elements.Clone();
    }

    // Клас Vector4
    public class Vector4 : NumericStructure
    {
        private const int DefaultSize = 4;

        public Vector4() : base(DefaultSize)
        {
            Console.WriteLine("[Створено Vector4]");
        }

        public override void Display()
        {
            Console.WriteLine("\n--- Вектор ---");
            Console.WriteLine(string.Join(", ", Elements.Select(e => e.ToString("F2"))));
        }

        // Індексатор для доступу до елементів
        public double this[int index]
        {
            get
            {
                if (index < 0 || index >= Size)
                    throw new IndexOutOfRangeException();
                return Elements[index];
            }
            set
            {
                if (index < 0 || index >= Size)
                    throw new IndexOutOfRangeException();
                Elements[index] = value;
            }
        }
    }

    // Клас Matrix4x4
    public class Matrix4x4 : NumericStructure
    {
        private const int MatrixDimension = 4;
        public int Rows { get; } = MatrixDimension;
        public int Cols { get; } = MatrixDimension;

        public Matrix4x4() : base(MatrixDimension * MatrixDimension)
        {
            Console.WriteLine("[Створено Matrix4x4]");
        }

        public override void Display()
        {
            Console.WriteLine("\n--- Матриця 4x4 ---");
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                    Console.Write($"{Elements[i * Cols + j],8:F2}");
                Console.WriteLine();
            }
        }

        public double this[int row, int col]
        {
            get
            {
                if (row < 0 || row >= Rows || col < 0 || col >= Cols)
                    throw new IndexOutOfRangeException();
                return Elements[row * Cols + col];
            }
            set
            {
                if (row < 0 || row >= Rows || col < 0 || col >= Cols)
                    throw new IndexOutOfRangeException();
                Elements[row * Cols + col] = value;
            }
        }
    }

    public static class Program
    {
        public static void Main()
        {
            List<IStructure> shapes = new List<IStructure>();

            Vector4 vector = new Vector4();
            vector.SetElements(new double[] { 2.5, -1.3, 4.8, 0.0 });
            shapes.Add(vector);

            Matrix4x4 matrix = new Matrix4x4();
            matrix.SetElements(new double[] {
                1.1, 2.2, 3.3, 4.4,
                5.5, 6.6, 7.7, 8.8,
                9.9, 10.0, 11.1, 12.2,
                13.3, 14.4, 15.5, 16.6
            });
            shapes.Add(matrix);

            Console.WriteLine("\n--- Поліморфізм через інтерфейс ---");
            foreach (var obj in shapes)
            {
                obj.Display();
                Console.WriteLine($"Максимальний елемент: {obj.GetMax():F2}");
            }
        }
    }
}

