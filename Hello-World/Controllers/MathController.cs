using Microsoft.AspNetCore.Mvc;
using System;
using System.Numerics;
using HelloWorld.Models;
using Microsoft.AspNetCore.Http;

namespace HelloWorld.Controllers;

/// <summary>
/// Provides endpoints for performing complex mathematical calculations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MathController : ControllerBase
{
    /// <summary>
    /// Solves a quadratic equation of the form ax^2 + bx + c = 0.
    /// </summary>
    /// <param name="a">The coefficient of the quadratic term (x^2).</param>
    /// <param name="b">The coefficient of the linear term (x).</param>
    /// <param name="c">The constant term.</param>
    /// <returns>An object containing the type of solution and the roots of the equation.</returns>
    /// <response code="200">Returns the solution to the quadratic equation.</response>
    /// <response code="400">If the coefficient 'a' is zero.</response>
    [HttpGet("quadratic")]
    [ProducesResponseType(typeof(QuadraticSolution), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<QuadraticSolution> SolveQuadratic(double a, double b, double c)
    {
        if (a == 0)
        {
            return BadRequest("The coefficient 'a' cannot be zero for a quadratic equation.");
        }

        var solution = new QuadraticSolution();
        var discriminant = b * b - 4 * a * c;

        if (discriminant > 0)
        {
            solution.SolutionType = "Two distinct real roots";
            var root1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
            var root2 = (-b - Math.Sqrt(discriminant)) / (2 * a);
            solution.Roots.Add(new ComplexNumber { Real = root1, Imaginary = 0 });
            solution.Roots.Add(new ComplexNumber { Real = root2, Imaginary = 0 });
        }
        else if (discriminant == 0)
        {
            solution.SolutionType = "One real root";
            var root = -b / (2 * a);
            solution.Roots.Add(new ComplexNumber { Real = root, Imaginary = 0 });
        }
        else // discriminant < 0
        {
            solution.SolutionType = "Two complex roots";
            var realPart = -b / (2 * a);
            var imaginaryPart = Math.Sqrt(-discriminant) / (2 * a);
            solution.Roots.Add(new ComplexNumber { Real = realPart, Imaginary = imaginaryPart });
            solution.Roots.Add(new ComplexNumber { Real = realPart, Imaginary = -imaginaryPart });
        }

        return Ok(solution);
    }

    /// <summary>
    /// Calculates the nth number in the Fibonacci sequence.
    /// </summary>
    /// <param name="n">The zero-based index of the Fibonacci number to calculate.</param>
    /// <returns>The nth Fibonacci number as a string to support large numbers.</returns>
    /// <response code="200">Returns the calculated Fibonacci number.</response>
    /// <response code="400">If 'n' is negative or too large.</response>
    [HttpGet("fibonacci")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<string> GetFibonacciNumber(int n)
    {
        if (n < 0)
        {
            return BadRequest("Input 'n' must be a non-negative integer.");
        }
        if (n > 1000)
        {
            return BadRequest("Input 'n' is too large. Please provide a number less than or equal to 1000 to prevent excessive resource usage.");
        }

        if (n == 0) return Ok("0");

        BigInteger a = 0;
        BigInteger b = 1;
        for (int i = 1; i < n; i++)
        {
            BigInteger temp = a + b;
            a = b;
            b = temp;
        }

        return Ok(b.ToString());
    }

    /// <summary>
    /// Checks if a given number is a prime number.
    /// </summary>
    /// <param name="number">The integer to check.</param>
    /// <returns>True if the number is prime, otherwise false.</returns>
    /// <response code="200">Returns boolean indicating if the number is prime.</response>
    [HttpGet("isprime/{number}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public ActionResult<bool> IsPrime(long number)
    {
        if (number <= 1) return Ok(false);
        if (number == 2) return Ok(true);
        if (number % 2 == 0) return Ok(false);

        var boundary = (long)Math.Floor(Math.Sqrt(number));

        for (long i = 3; i <= boundary; i += 2)
        {
            if (number % i == 0)
            {
                return Ok(false);
            }
        }

        return Ok(true);
    }
}