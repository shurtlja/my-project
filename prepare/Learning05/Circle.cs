public class Circle : Shape
{
    private double _radius;

    public Circle(double radius, string color)
    {
        _radius = radius;
        _color = color;
    }

    public override double GetArea()
    {
        return Math.PI * _radius * _radius;
    }
}