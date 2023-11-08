using Dijkstra3D.Core;

namespace Dijkstra3D.UnitTests;

public class Dijkstra3D
{
    [Fact]
    public void CalculatePerpendicularLine_ReturnsExpectedValues()
    {
        // Arrange
        double x1 = -5;
        double y1 = 10;
        double x2 = -3;
        double y2 =4;

        var calculator = new Calculations();

        // Act
        Dictionary<string, double> result = calculator.CalculatePerpendicularLine(x1, y1, x2, y2);
        double preperdicularSlope = 0.3333333333333333;
        var actualB = preperdicularSlope * x2 - y2;
        // Assert
        Assert.Equal(preperdicularSlope, result["slop"]);
        Assert.Equal(actualB, result["b"]);
    }
}