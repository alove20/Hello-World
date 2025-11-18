# Calculator API Solution

## Overview
This solution implements a RESTful Calculator API with four basic arithmetic operations using .NET 9 Web API.

## Implementation Details

### Controller: CalculatorController
Located at `Hello-World/Controllers/CalculatorController.cs`

### Endpoints

#### 1. Addition
- **URL**: `/api/calculator/add?a={number}&b={number}`
- **Method**: GET
- **Example**: `/api/calculator/add?a=10&b=5`
- **Response**: