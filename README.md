# Introduction 
TODO: Give a short introduction of your project. Let this section explain the objectives or the motivation behind this project. 

#Coding task
Develop a simple RESTful Web API using C# that integrates with the mock API provided at https://restful-api.dev. Your API should extend the functionality of the mock API by adding additional processing or features.

Technical Requirements:
Framework .NET 8.0
API Methods: Implement at least three API methods:
To retrieve data from the mock API with the ability to filter by name(substring) and paging
To add data to the mock API
To remove data via the mock API
Validation:
Implement proper validation for the model.
Ensure all required fields are provided and valid when creating or updating a product.
Error Handling:
Implement proper error handling.

#Project Overview
This is a .NET 8  application
Uses Clean Architecture principles
Implements CQRS pattern with MediatR
Has integration with an external REST API service
#2 Architecture Layers
Domain Layer (TrueStory.Domain)
Contains core business entities
Defines interfaces for repositories
Includes value objects and domain events
Application Layer (TrueStory.Application)
Contains business logic
Implements CQRS pattern
Handles commands and queries
Manages DTOs and mapping
Infrastructure Layer (TrueStory.Infrastructure)
Implements external service integrations
Contains REST client implementations
Handles data persistence
Manages external API communications
API Layer (TrueStory.Web.API)
Provides REST endpoints
Handles HTTP requests/responses
Manages authentication/authorization
#3 Key Features
Product Management
Create products
Delete products
Get products with pagination
Product filtering and search
External Service Integration
REST API client implementation
Service abstraction layer
CQRS Implementation
Separate commands and queries
MediatR for handling requests
#4 Testing Structure
Unit tests for application layer
Integration tests for API endpoints
Test coverage for critical paths
Mock implementations for external services
#5 Technical Stack
.NET 8
Entity Framework Core
MediatR for CQRS
FluentValidation
xUnit for testing
Moq for test mocking
#6 Integration Points
External REST API service
Database persistence
HTTP client factory for external calls
#7 Security Features
Authentication/Authorization
Input validation
Error handling
Secure communication with external services
#8 Project Organization
Clear separation of concerns
Modular design
Consistent naming conventions
Well-defined interfaces
#9 Dependencies
Internal dependencies between layers
External service dependencies
Third-party library dependencies
#10 Development Practices
Clean Architecture principles
SOLID principles
Dependency Injection
Async/await patterns
Error handling strategies
