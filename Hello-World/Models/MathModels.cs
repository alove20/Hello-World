namespace HelloWorld.Models;

/// <summary>
/// Represents the solution of a quadratic equation.
/// </summary>
public class QuadraticSolution
{
    /// <summary>
    /// A description of the type of roots found (e.g., "Two distinct real roots").
    /// </summary>
    public string SolutionType { get; set; }

    /// <summary>
    /// A list of the roots of the equation.
    /// </summary>
    public List<ComplexNumber> Roots { get; set; }

    public QuadraticSolution()
    {
        Roots = new List<ComplexNumber>();
        SolutionType = string.Empty;
    }
}

/// <summary>
/// Represents a complex number with real and imaginary parts.
/// </summary>
public class ComplexNumber
{
    /// <summary>
    /// The real part of the complex number.
    /// </summary>
    public double Real { get; set; }

    /// <summary>
    /// The imaginary part of the complex number.
    /// </summary>
    public double Imaginary { get; set; }
}