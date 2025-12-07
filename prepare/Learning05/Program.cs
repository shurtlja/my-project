using System;
using System.Globalization;
public class Program
{
    public static void Main()
    {
        List<Shape> shapes = new List<Shape>();
        shapes.Add(new Square(5, "Red"));
        shapes.Add(new Rectangle(4, 8, "Blue"));
        shapes.Add(new Circle(3, "Green"));

        foreach (var shape in shapes)
        {
            Console.WriteLine($"{shape.GetColor()}, {shape.GetArea()}");
        }
    }
}