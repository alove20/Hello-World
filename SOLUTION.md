# Solution for Task calc-20251117234937

**Summary**: I'll create a comprehensive Calculator API with four arithmetic operation endpoints (add, subtract, multiply, divide) using .NET 9 Web API best practices. The solution includes proper error handling, input validation, API documentation, and follows RESTful principles with JSON responses.

**Implementation**

**File: `Hello-World/Controllers/CalculatorController.cs`**
```csharp
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
```

**File: `Hello-World/Program.cs`**
```csharp
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add API Explorer for Swagger
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger/OpenAPI
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Calculator API",
        Description = "A simple calculator API providing basic arithmetic operations",
        Contact = new OpenApiContact
        {
            Name = "Calculator API Team"
        }
    });

    // Enable XML comments for better documentation
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// Add CORS policy for development
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Calculator API v1");
        options.RoutePrefix = string.Empty; // Serve Swagger UI at root
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
```

**File: `Hello-World/Hello-World.csproj`**
```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <RootNamespace>Hello_World</RootNamespace>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
    <NoWarn>$(NoWarn);1591</NoWarn>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
  </ItemGroup>

</Project>
```

**File: `Hello-World/Hello-World.http`**
```http
### Calculator API Tests

### Health Check
GET http://localhost:5000/weatherforecast

### Addition
GET http://localhost:5000/api/calculator/add?a=10&b=5

### Addition with decimals
GET http://localhost:5000/api/calculator/add?a=10.5&b=5.25

### Addition with negative numbers
GET http://localhost:5000/api/calculator/add?a=-10&b=5

### Subtraction
GET http://localhost:5000/api/calculator/subtract?a=10&b=5

### Subtraction resulting in negative
GET http://localhost:5000/api/calculator/subtract?a=5&b=10

### Multiplication
GET http://localhost:5000/api/calculator/multiply?a=10&b=5

### Multiplication with decimals
GET http://localhost:5000/api/calculator/multiply?a=2.5&b=4

### Division
GET http://localhost:5000/api/calculator/divide?a=10&b=5

### Division with decimals
GET http://localhost:5000/api/calculator/divide?a=10&b=3

### Division by zero (should return error)
GET http://localhost:5000/api/calculator/divide?a=10&b=0

### Large number addition
GET http://localhost:5000/api/calculator/add?a=999999999&b=1

### Zero operations
GET http://localhost:5000/api/calculator/multiply?a=0&b=100
```

**File: `README.md`**
```markdown
# Calculator API

A .NET 9 Web API providing basic arithmetic operations (addition, subtraction, multiplication, and division).

## Features

- ✅ RESTful API design
- ✅ Four basic arithmetic operations
- ✅ Comprehensive error handling
- ✅ Input validation
- ✅ Division by zero protection
- ✅ Overflow detection
- ✅ JSON responses
- ✅ Swagger/OpenAPI documentation
- ✅ Logging support
- ✅ XML documentation comments

## Endpoints

### Addition
```
GET /api/calculator/add?a={number}&b={number}
```
Returns the sum of two numbers.

**Example:**
```
GET /api/calculator/add?a=10&b=5
```

**Response:**
```json
{
  "operation": "addition",
  "operandA": 10,
  "operandB": 5,
  "result": 15,
  "formula": "10 + 5 = 15"
}
```

### Subtraction
```
GET /api/calculator/subtract?a={number}&b={number}
```
Subtracts the second number from the first.

**Example:**
```
GET /api/calculator/subtract?a=10&b=5
```

**Response:**
```json
{
  "operation": "subtraction",
  "operandA": 