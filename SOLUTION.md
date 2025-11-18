# Solution for Task github-alove20-Hello-World-11

**Summary**: I'll create a set of creative and complex math-solving endpoints including matrix operations, polynomial root finding, numerical integration, statistical analysis, and fractal generation. These endpoints will demonstrate advanced mathematical capabilities while maintaining clean, well-documented code.

**Implementation**

**File: `Hello-World/Controllers/MathController.cs`**
```csharp
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hello_World.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MathController : ControllerBase
    {
        /// <summary>
        /// Solves a system of linear equations using Gaussian elimination
        /// </summary>
        /// <param name="request">The matrix equation Ax = b</param>
        /// <returns>Solution vector x</returns>
        [HttpPost("solve-linear-system")]
        public ActionResult<LinearSystemResult> SolveLinearSystem([FromBody] LinearSystemRequest request)
        {
            try
            {
                if (request.Matrix == null || request.Constants == null)
                {
                    return BadRequest("Matrix and constants are required");
                }

                int n = request.Constants.Length;
                if (request.Matrix.Length != n || request.Matrix.Any(row => row.Length != n))
                {
                    return BadRequest("Matrix must be square and match constants length");
                }

                var solution = GaussianElimination(request.Matrix, request.Constants);
                
                return Ok(new LinearSystemResult
                {
                    Solution = solution,
                    Steps = "Solved using Gaussian elimination with partial pivoting"
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error solving system: {ex.Message}");
            }
        }

        /// <summary>
        /// Finds roots of a polynomial using the Newton-Raphson method
        /// </summary>
        /// <param name="request">Polynomial coefficients (highest degree first)</param>
        /// <returns>Approximate roots of the polynomial</returns>
        [HttpPost("polynomial-roots")]
        public ActionResult<PolynomialRootsResult> FindPolynomialRoots([FromBody] PolynomialRequest request)
        {
            try
            {
                if (request.Coefficients == null || request.Coefficients.Length < 2)
                {
                    return BadRequest("At least 2 coefficients required");
                }

                var roots = FindRoots(request.Coefficients, request.MaxIterations ?? 1000, request.Tolerance ?? 1e-10);
                
                return Ok(new PolynomialRootsResult
                {
                    Coefficients = request.Coefficients,
                    Roots = roots,
                    Degree = request.Coefficients.Length - 1
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error finding roots: {ex.Message}");
            }
        }

        /// <summary>
        /// Performs numerical integration using Simpson's rule
        /// </summary>
        /// <param name="request">Integration parameters</param>
        /// <returns>Approximate integral value</returns>
        [HttpPost("integrate")]
        public ActionResult<IntegrationResult> NumericalIntegration([FromBody] IntegrationRequest request)
        {
            try
            {
                if (request.Lower >= request.Upper)
                {
                    return BadRequest("Lower bound must be less than upper bound");
                }

                int n = request.Intervals ?? 1000;
                if (n % 2 != 0) n++; // Simpson's rule requires even number of intervals

                var result = SimpsonsRule(request.FunctionType, request.Lower, request.Upper, n);
                
                return Ok(new IntegrationResult
                {
                    FunctionType = request.FunctionType,
                    Lower = request.Lower,
                    Upper = request.Upper,
                    Result = result,
                    Method = "Simpson's Rule",
                    Intervals = n
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error computing integral: {ex.Message}");
            }
        }

        /// <summary>
        /// Calculates advanced statistical measures for a dataset
        /// </summary>
        /// <param name="request">Dataset to analyze</param>
        /// <returns>Comprehensive statistical analysis</returns>
        [HttpPost("statistics")]
        public ActionResult<StatisticsResult> AdvancedStatistics([FromBody] StatisticsRequest request)
        {
            try
            {
                if (request.Data == null || request.Data.Length == 0)
                {
                    return BadRequest("Data array cannot be empty");
                }

                var sorted = request.Data.OrderBy(x => x).ToArray();
                var mean = sorted.Average();
                var variance = sorted.Select(x => Math.Pow(x - mean, 2)).Average();
                var stdDev = Math.Sqrt(variance);
                var skewness = CalculateSkewness(sorted, mean, stdDev);
                var kurtosis = CalculateKurtosis(sorted, mean, stdDev);
                
                return Ok(new StatisticsResult
                {
                    Count = sorted.Length,
                    Mean = mean,
                    Median = CalculateMedian(sorted),
                    Mode = CalculateMode(sorted),
                    Variance = variance,
                    StandardDeviation = stdDev,
                    Skewness = skewness,
                    Kurtosis = kurtosis,
                    Range = sorted.Last() - sorted.First(),
                    Min = sorted.First(),
                    Max = sorted.Last(),
                    Q1 = CalculatePercentile(sorted, 25),
                    Q3 = CalculatePercentile(sorted, 75),
                    IQR = CalculatePercentile(sorted, 75) - CalculatePercentile(sorted, 25)
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error calculating statistics: {ex.Message}");
            }
        }

        /// <summary>
        /// Generates points for the Mandelbrot set
        /// </summary>
        /// <param name="request">Mandelbrot generation parameters</param>
        /// <returns>Grid of iteration counts</returns>
        [HttpPost("mandelbrot")]
        public ActionResult<MandelbrotResult> GenerateMandelbrot([FromBody] MandelbrotRequest request)
        {
            try
            {
                int width = request.Width ?? 100;
                int height = request.Height ?? 100;
                int maxIterations = request.MaxIterations ?? 100;
                double xMin = request.XMin ?? -2.5;
                double xMax = request.XMax ?? 1.0;
                double yMin = request.YMin ?? -1.0;
                double yMax = request.YMax ?? 1.0;

                var grid = new int[height][];
                for (int i = 0; i < height; i++)
                {
                    grid[i] = new int[width];
                    for (int j = 0; j < width; j++)
                    {
                        double x0 = xMin + (xMax - xMin) * j / width;
                        double y0 = yMin + (yMax - yMin) * i / height;
                        grid[i][j] = MandelbrotIterations(x0, y0, maxIterations);
                    }
                }

                return Ok(new MandelbrotResult
                {
                    Grid = grid,
                    Width = width,
                    Height = height,
                    MaxIterations = maxIterations,
                    Bounds = new { xMin, xMax, yMin, yMax }
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error generating Mandelbrot set: {ex.Message}");
            }
        }

        /// <summary>
        /// Calculates eigenvalues and eigenvectors of a matrix
        /// </summary>
        /// <param name="request">Square matrix</param>
        /// <returns>Dominant eigenvalue and eigenvector</returns>
        [HttpPost("eigenvalues")]
        public ActionResult<EigenResult> CalculateEigenvalues([FromBody] MatrixRequest request)
        {
            try
            {
                if (request.Matrix == null || request.Matrix.Length == 0)
                {
                    return BadRequest("Matrix cannot be empty");
                }

                int n = request.Matrix.Length;
                if (request.Matrix.Any(row => row.Length != n))
                {
                    return BadRequest("Matrix must be square");
                }

                var (eigenvalue, eigenvector) = PowerMethod(request.Matrix, request.MaxIterations ?? 1000, request.Tolerance ?? 1e-10);

                return Ok(new EigenResult
                {
                    DominantEigenvalue = eigenvalue,
                    DominantEigenvector = eigenvector,
                    Method = "Power Method"
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error calculating eigenvalues: {ex.Message}");
            }
        }

        // Helper methods

        private double[] GaussianElimination(double[][] A, double[] b)
        {
            int n = b.Length;
            double[][] augmented = new double[n][];
            
            for (int i = 0; i < n; i++)
            {
                augmented[i] = new double[n + 1];
                Array.Copy(A[i], augmented[i], n);
                augmented[i][n] = b[i];
            }

            // Forward elimination with partial pivoting
            for (int i = 0; i < n; i++)
            {
                int maxRow = i;
                for (int k = i + 1; k < n; k++)
                {
                    if (Math.Abs(augmented[k][i]) > Math.Abs(augmented[maxRow][i]))
                        maxRow = k;
                }

                var temp = augmented[i];
                augmented[i] = augmented[maxRow];
                augmented[maxRow] = temp;

                for (int k = i + 1; k < n; k++)
                {
                    double factor = augmented[k][i] / augmented[i][i];
                    for (int j = i; j <= n; j++)
                    {
                        augmented[k][j] -= factor * augmented[i][j];
                    }
                }
            }

            // Back substitution
            double[] x = new double[n];
            for (int i = n - 1; i >= 0; i--)
            {
                x[i] = augmented[i][n];
                for (int j = i + 1; j < n; j++)
                {
                    x[i] -= augmented[i][j] * x[j];
                }
                x[i] /= augmented[i][i];
            }

            return x;
        }

        private List<double> FindRoots(double[] coefficients, int maxIterations, double tolerance)
        {
            var roots = new List<double>();
            int degree = coefficients.Length - 1;

            for (int attempt = 0; attempt < degree; attempt++)
            {
                double x = attempt * 0.5 - 1.0; // Different starting points
                
                for (int iter = 0; iter < maxIterations; iter++)
                {
                    double fx = EvaluatePolynomial(coefficients, x);
                    double fpx = EvaluatePolynomialDerivative(coefficients, x);
                    
                    if (Math.Abs(fpx) < tolerance) break;
                    
                    double xNew = x - fx / fpx;
                    
                    if (Math.Abs(xNew - x) < tolerance)
                    {
                        if (!roots.Any(r => Math.Abs(r - xNew) < tolerance * 10))
                        {
                            roots.Add(Math.Round(xNew, 10));
                        }
                        break;
                    }
                    x = xNew;
                }
            }

            return roots.Distinct().OrderBy(x => x).ToList();
        }

        private double EvaluatePolynomial(double[] coefficients, double x)
        {
            double result = 0;
            double power = 1;
            for (int i = coefficients.Length - 1; i >= 0; i--)
            {
                result += coefficients[i] * power;
                power *= x;
            }
            return result;
        }

        private double EvaluatePolynomialDerivative(double[] coefficients, double x)
        {
            double result = 0;
            double power = 1;
            for (int i = coefficients.Length - 1; i > 0; i--)
            {
                result += coefficients[i - 1] * i * power;
                power *= x;
            }
            return result;
        }

        private double SimpsonsRule(string functionType, double lower, double upper, int n)
        {
            double h = (upper - lower) / n;
            double sum = EvaluateFunction(functionType, lower) + EvaluateFunction(functionType, upper);

            for (int i = 1; i < n; i++)
            {
                double x = lower + i * h;
                sum += EvaluateFunction(functionType, x) * (i % 2 == 0 ? 2 : 4);
            }

            return sum * h / 3;
        }

        private double EvaluateFunction(string functionType, double x)
        {
            return functionType.ToLower() switch
            {
                "sin" => Math.Sin(x),
                "cos" => Math.Cos(x),
                "exp" => Math.Exp(x),
                "sqrt" => Math.Sqrt(Math.Abs(x)),
                "x^2" => x * x,
                "x^3" => x * x * x,
                "1/x" => x != 0 ? 1 / x : 0,
                "ln" => x > 0 ? Math.Log(x) : 0,
                _ => x * x
            };
        }

        private double CalculateMedian(double[] sorted)
        {
            int n = sorted.Length;
            return n % 2 == 0 ? (sorted[n / 2 - 1] + sorted[n / 2]) / 2 : sorted[n / 2];
        }

        private List<double> CalculateMode(double[] sorted)
        {
            var frequency = sorted.GroupBy(x => x)
                                 .Select(g => new { Value = g.Key, Count = g.Count() })
                                 .OrderByDescending(x => x.Count)
                                 .ToList();
            
            int maxCount = frequency.First().Count;
            if (maxCount == 1) return new List<double>();
            
            return frequency.Where(x => x.Count == maxCount).Select(x => x.Value).ToList();
        }

        private double CalculatePercentile(double[] sorted, double percentile)
        {
            double index = (percentile / 100.0) * (sorted.Length - 1);
            int lower = (int)Math.Floor(index);
            int upper = (int)Math.Ceiling(index);
            double weight = index - lower;
            return sorted[lower] * (1 - weight) + sorted[upper] * weight;
        }

        private double CalculateSkewness(double[] data, double mean, double stdDev)
        {
            if (stdDev == 0) return 0;
            return data.Average(x => Math.Pow((x - mean) / stdDev, 3));
        }

        private double CalculateKurtosis(double[] data, double mean, double stdDev)
        {
            if (stdDev == 0) return 0;
            return data.Average(x => Math.Pow((x - mean) / stdDev, 4)) - 3;
        }

        private int MandelbrotIterations(double x0