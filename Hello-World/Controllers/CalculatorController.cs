using Microsoft.AspNetCore.Mvc;

namespace Hello_World.Controllers
{
    /// <summary>
    /// Calculator API controller providing basic arithmetic operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
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
        /// <response code="200">Returns the sum</response>
        /// <response code="400">If the input parameters are invalid</response>
        [HttpGet("add")]
        [ProducesResponseType(typeof(CalculationResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public ActionResult<CalculationResult> Add([FromQuery] double a, [FromQuery] double b)
        {
            try
            {
                if (double.IsNaN(a) || double.IsNaN(b) || double.IsInfinity(a) || double.IsInfinity(b))
                {
                    return BadRequest(new ErrorResponse 
                    { 
                        Error = "Invalid input parameters",
                        Message = "Parameters must be valid numbers"
                    });
                }

                double result = a + b;

                if (double.IsInfinity(result))
                {
                    return BadRequest(new ErrorResponse 
                    { 
                        Error = "Calculation overflow",
                        Message = "The result exceeds the maximum representable value"
                    });
                }

                _logger.LogInformation("Addition: {A} + {B} = {Result}", a, b, result);

                return Ok(new CalculationResult
                {
                    Operation = "addition",
                    OperandA = a,
                    OperandB = b,
                    Result = result,
                    Formula = $"{a} + {b} = {result}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error performing addition");
                return BadRequest(new ErrorResponse 
                { 
                    Error = "Calculation error",
                    Message = ex.Message
                });
            }
        }

        /// <summary>
        /// Subtracts second number from first number
        /// </summary>
        /// <param name="a">First number (minuend)</param>
        /// <param name="b">Second number (subtrahend)</param>
        /// <returns>Difference of a and b</returns>
        /// <response code="200">Returns the difference</response>
        /// <response code="400">If the input parameters are invalid</response>
        [HttpGet("subtract")]
        [ProducesResponseType(typeof(CalculationResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public ActionResult<CalculationResult> Subtract([FromQuery] double a, [FromQuery] double b)
        {
            try
            {
                if (double.IsNaN(a) || double.IsNaN(b) || double.IsInfinity(a) || double.IsInfinity(b))
                {
                    return BadRequest(new ErrorResponse 
                    { 
                        Error = "Invalid input parameters",
                        Message = "Parameters must be valid numbers"
                    });
                }

                double result = a - b;

                if (double.IsInfinity(result))
                {
                    return BadRequest(new ErrorResponse 
                    { 
                        Error = "Calculation overflow",
                        Message = "The result exceeds the maximum representable value"
                    });
                }

                _logger.LogInformation("Subtraction: {A} - {B} = {Result}", a, b, result);

                return Ok(new CalculationResult
                {
                    Operation = "subtraction",
                    OperandA = a,
                    OperandB = b,
                    Result = result,
                    Formula = $"{a} - {b} = {result}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error performing subtraction");
                return BadRequest(new ErrorResponse 
                { 
                    Error = "Calculation error",
                    Message = ex.Message
                });
            }
        }

        /// <summary>
        /// Multiplies two numbers
        /// </summary>
        /// <param name="a">First number (multiplicand)</param>
        /// <param name="b">Second number (multiplier)</param>
        /// <returns>Product of a and b</returns>
        /// <response code="200">Returns the product</response>
        /// <response code="400">If the input parameters are invalid</response>
        [HttpGet("multiply")]
        [ProducesResponseType(typeof(CalculationResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public ActionResult<CalculationResult> Multiply([FromQuery] double a, [FromQuery] double b)
        {
            try
            {
                if (double.IsNaN(a) || double.IsNaN(b) || double.IsInfinity(a) || double.IsInfinity(b))
                {
                    return BadRequest(new ErrorResponse 
                    { 
                        Error = "Invalid input parameters",
                        Message = "Parameters must be valid numbers"
                    });
                }

                double result = a * b;

                if (double.IsInfinity(result))
                {
                    return BadRequest(new ErrorResponse 
                    { 
                        Error = "Calculation overflow",
                        Message = "The result exceeds the maximum representable value"
                    });
                }

                _logger.LogInformation("Multiplication: {A} * {B} = {Result}", a, b, result);

                return Ok(new CalculationResult
                {
                    Operation = "multiplication",
                    OperandA = a,
                    OperandB = b,
                    Result = result,
                    Formula = $"{a} * {b} = {result}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error performing multiplication");
                return BadRequest(new ErrorResponse 
                { 
                    Error = "Calculation error",
                    Message = ex.Message
                });
            }
        }

        /// <summary>
        /// Divides first number by second number
        /// </summary>
        /// <param name="a">Dividend</param>
        /// <param name="b">Divisor</param>
        /// <returns>Quotient of a divided by b</returns>
        /// <response code="200">Returns the quotient</response>
        /// <response code="400">If the input parameters are invalid or division by zero</response>
        [HttpGet("divide")]
        [ProducesResponseType(typeof(CalculationResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public ActionResult<CalculationResult> Divide([FromQuery] double a, [FromQuery] double b)
        {
            try
            {
                if (double.IsNaN(a) || double.IsNaN(b) || double.IsInfinity(a) || double.IsInfinity(b))
                {
                    return BadRequest(new ErrorResponse 
                    { 
                        Error = "Invalid input parameters",
                        Message = "Parameters must be valid numbers"
                    });
                }

                if (b == 0)
                {
                    return BadRequest(new ErrorResponse 
                    { 
                        Error = "Division by zero",
                        Message = "Cannot divide by zero"
                    });
                }

                double result = a / b;

                if (double.IsInfinity(result) || double.IsNaN(result))
                {
                    return BadRequest(new ErrorResponse 
                    { 
                        Error = "Calculation error",
                        Message = "The result is undefined or infinite"
                    });
                }

                _logger.LogInformation("Division: {A} / {B} = {Result}", a, b, result);

                return Ok(new CalculationResult
                {
                    Operation = "division",
                    OperandA = a,
                    OperandB = b,
                    Result = result,
                    Formula = $"{a} / {b} = {result}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error performing division");
                return BadRequest(new ErrorResponse 
                { 
                    Error = "Calculation error",
                    Message = ex.Message
                });
            }
        }
    }

    /// <summary>
    /// Represents the result of a calculation
    /// </summary>
    public class CalculationResult
    {
        /// <summary>
        /// The arithmetic operation performed
        /// </summary>
        public string Operation { get; set; } = string.Empty;

        /// <summary>
        /// First operand
        /// </summary>
        public double OperandA { get; set; }

        /// <summary>
        /// Second operand
        /// </summary>
        public double OperandB { get; set; }

        /// <summary>
        /// Result of the calculation
        /// </summary>
        public double Result { get; set; }

        /// <summary>
        /// Formula representation of the calculation
        /// </summary>
        public string Formula { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents an error response
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Error type
        /// </summary>
        public string Error { get; set; } = string.Empty;

        /// <summary>
        /// Detailed error message
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}