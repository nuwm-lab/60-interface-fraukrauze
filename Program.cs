using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOP_Lab3
{
    /// <summary>
    /// Абстрактний базовий клас для числових структур (векторів і матриць).
    /// Містить спільні властивості та методи.
    /// </summary>
    public abstract class NumericStructure
    {
        protected int _size;
        protected double[] _elements;

        // Конструктор
        public NumericStructure(int size)
        {
            if (size <= 0)
                throw new ArgumentException("Розмір має бути додатнім.");

            _size = size;
            _elements = new double[size];
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine($"[Конструктор базового класу] Створено NumericStructure на {_size} елементів.");
        }

        // Деструктор
        ~NumericStructure()
        {
            Console.WriteLine("[Деструктор базового класу] Об’єкт NumericStructure знищено.");
        }

        // Абстрактні методи — мають бути реалізовані в похідних класах
        public abstract void SetElements(double[] values);
        public abstract void SetElementsFromConsole();
        public abstract void Display();

        // Віртуальний метод із типовою реалізацією
        public virtual double GetMax()
        {
            return _elements.Length > 0 ? _elements.Max() : double.NaN;
        }

        // Загальне рядкове представлення
        public override string ToString()
        {
            return string.Join(", ", _elements.Select(e => e.ToString("F2")));
        }
    }

    /// <summary>
    /// Клас Vector4 — спадкоємець NumericStructure.
    /// Представляє одномірний вектор на 4 елементи.
    /// </summary>
    public class Vector4 : NumericStructure
    {
        private const int DEFAULT_SIZE = 4;

        public Vector4() : base(DEFAULT_SIZE)
        {
            Console.WriteLine("[Конструктор Vector4] Створено вектор на 4 елементи.");
        }

        ~Vector4()
        {
            Console.WriteLine("[Деструктор Vector4] Вектор знищено.");
        }

        public override void SetElements(double[] values)
        {
            if (values == null || values.Length != _size)
                throw new ArgumentException($"Розмір вхідного масиву ({values?.Length ?? 0}) не відповідає {_size}.");

            Array.Copy(values, _elements, _size);
            Console.WriteLine($"Успішно встановлено {_size} елементів вектора.");
        }

        public override void SetElementsFromConsole()
        {
            Console.WriteLine($"\nВведіть {_size} елементів вектора:");
            for (int i = 0; i < _size; i++)
            {
                Console.Write($"Елемент [{i}] = ");
                while (!double.TryParse(Console.ReadLine(), out _elements[i]))
                    Console.Write("Некоректне значення, повторіть: ");
            }
        }

        public override void Display()
        {
            Console.WriteLine("\n--- Вектор ---");
            Console.WriteLine(ToString());
        }
    }

    /// <summary>
    /// Клас Matrix4x4 — спадкоємець NumericStructure.
    /// Представляє матрицю розміром 4x4.
    /// </summary>
    public class Matrix4x4 : NumericStructure
    {
        private const int MATRIX_DIMENSION = 4;
        private int _rows;
        private int _cols;

        public Matrix4x4() : base(MATRIX_DIMENSION * MATRIX_DIMENSION)
        {
            _rows = MATRIX_DIMENSION;
            _cols = MATRIX_DIMENSION;
            Console.WriteLine("[Конструктор Matrix4x4] Створено матрицю 4x4.");
        }

        ~Matrix4x4()
        {
            Console.WriteLine("[Деструктор Matrix4x4] Матрицю знищено.");
        }

        public override void SetElements(double[] values)
        {
            if (values == null || values.Length != _size)
                throw new ArgumentException($"Розмір масиву ({values?.Length ?? 0}) не відповідає {_size} елементам матриці.");

            Array.Copy(values, _elements, _size);
            Console.WriteLine($"Успішно встановлено {_size} елементів матриці.");
        }

        public override void SetElementsFromConsole()
        {
            Console.WriteLine($"\nВведіть елементи матриці {_rows}x{_cols}:");
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    int index = i * _cols + j;
                    Console.Write($"Елемент [{i},{j}] = ");
                    while (!double.TryParse(Console.ReadLine(), out _elements[index]))
                        Console.Write("Некоректне значення, повторіть: ");
                }
            }
        }

        public override void Display()
        {
            Console.WriteLine("\n--- Матриця 4x4 ---");
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                    Console.Write($"{_elements[i * _cols + j],8:F2}");
                Console.WriteLine();
            }
        }
    }

    /// <summary>
    /// Демонстрація роботи з абстрактним класом NumericStructure.
    /// </summary>
    public static class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            List<NumericStructure> shapes = new List<NumericStructure>();

            Console.WriteLine("\n--- СТВОРЕННЯ ВЕКТОРА ---");
            Vector4 vector = new Vector4();
            vector.SetElements(new double[] { 2.5, -1.3, 4.8, 0.0 });
            shapes.Add(vector);

            Console.WriteLine("\n--- СТВОРЕННЯ МАТРИЦІ ---");
            Matrix4x4 matrix = new Matrix4x4();
            matrix.SetElements(new double[] {
                1.1, 2.2, 3.3, 4.4,
                5.5, 6.6, 7.7, 8.8,
                9.9, 10.0, 11.1, 12.2,
                13.3, 14.4, 15.5, 16.6
            });
            shapes.Add(matrix);

            Console.WriteLine("\n==========================================");
            Console.WriteLine("ВИКЛИК МЕТОДІВ ЧЕРЕЗ КОЛЕКЦІЮ (ПОЛІМОРФІЗМ)");
            Console.WriteLine("==========================================");

            foreach (var obj in shapes)
            {
                obj.Display();
                Console.WriteLine($"Максимальний елемент: {obj.GetMax():F2}");
            }

            // Неможливо створити екземпляр абстрактного класу:
            // NumericStructure test = new NumericStructure(4); // ❌ Помилка компіляції
        }
    }
}
