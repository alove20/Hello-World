using Microsoft.AspNetCore.Mvc;

namespace Hello_World.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CalculatorController : ControllerBase
{
    private readonly ILogger<CalculatorController> _logger;

    public CalculatorController(ILogger<CalculatorController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Adds two numbers
    /// </summary>
    /// <param name="a">First number</param>
    /// <param name="b">Second number</param>
    /// <returns>Sum of a and b</returns>
    [HttpGet("add")]
    [ProducesResponseType(typeof(CalculatorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public ActionResult<CalculatorResponse> Add([FromQuery] double a, [FromQuery] double b)
    {
        try
        {
            var result = a + b;
            _logger.LogInformation("Addition: {A} + {B} = {Result}", a, b, result);
            
            return Ok(new CalculatorResponse
            {
                Operation = "addition",
                A = a,
                B = b,
                Result = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error performing addition");
            return BadRequest(new ErrorResponse { Error = "An error occurred during addition" });
        }
    }

    /// <summary>
    /// Subtracts two numbers
    /// </summary>
    /// <param name="a">First number</param>
    /// <param name="b">Second number</param>
    /// <returns>Difference of a and b</returns>
    [HttpGet("subtract")]
    [ProducesResponseType(typeof(CalculatorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public ActionResult<CalculatorResponse> Subtract([FromQuery] double a, [FromQuery] double b)
    {
        try
        {
            var result = a - b;
            _logger.LogInformation("Subtraction: {A} - {B} = {Result}", a, b, result);
            
            return Ok(new CalculatorResponse
            {
                Operation = "subtraction",
                A = a,
                B = b,
                Result = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error performing subtraction");
            return BadRequest(new ErrorResponse { Error = "An error occurred during subtraction" });
        }
    }

    /// <summary>
    /// Multiplies two numbers
    /// </summary>
    /// <param name="a">First number</param>
    /// <param name="b">Second number</param>
    /// <returns>Product of a and b</returns>
    [HttpGet("multiply")]
    [ProducesResponseType(typeof(CalculatorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public ActionResult<CalculatorResponse> Multiply([FromQuery] double a, [FromQuery] double b)
    {
        try
        {
            var result = a * b;
            _logger.LogInformation("Multiplication: {A} * {B} = {Result}", a, b, result);
            
            return Ok(new CalculatorResponse
            {
                Operation = "multiplication",
                A = a,
                B = b,
                Result = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error performing multiplication");
            return BadRequest(new ErrorResponse { Error = "An error occurred during multiplication" });
        }
    }

    /// <summary>
    /// Divides two numbers
    /// </summary>
    /// <param name="a">Dividend</param>
    /// <param name="b">Divisor</param>
    /// <returns>Quotient of a and b</returns>
    [HttpGet("divide")]
    [ProducesResponseType(typeof(CalculatorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public ActionResult<CalculatorResponse> Divide([FromQuery] double a, [FromQuery] double b)
    {
        try
        {
            if (b == 0)
            {
                _logger.LogWarning("Division by zero attempted: {A} / {B}", a, b);
                return BadRequest(new ErrorResponse { Error = "Cannot divide by zero" });
            }

            var result = a / b;
            _logger.LogInformation("Division: {A} / {B} = {Result}", a, b, result);
            
            return Ok(new CalculatorResponse
            {
                Operation = "division",
                A = a,
                B = b,
                Result = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error performing division");
            return BadRequest(new ErrorResponse { Error = "An error occurred during division" });
        }
    }
}

/// <summary>
/// Response model for calculator operations
/// </summary>
public class CalculatorResponse
{
    public string Operation { get; set; } = string.Empty;
    public double A { get; set; }
    public double B { get; set; }
    public double Result { get; set; }
}

/// <summary>
/// Error response model
/// </summary>
public class ErrorResponse
{
    public string Error { get; set; } = string.Empty;
}