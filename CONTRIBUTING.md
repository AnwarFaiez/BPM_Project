# Contributing to Enterprise BPM System

Thank you for your interest in contributing to the Enterprise BPM System! This document provides guidelines and instructions for contributing.

## Code of Conduct

- Be respectful and inclusive
- Welcome newcomers and help them learn
- Focus on constructive feedback
- Respect different viewpoints and experiences

## How to Contribute

### Reporting Bugs

1. Check if the bug has already been reported in [Issues](https://github.com/AnwarFaiez/BPM_Project/issues)
2. If not, create a new issue with:
   - Clear, descriptive title
   - Detailed description of the bug
   - Steps to reproduce
   - Expected vs. actual behavior
   - Environment details (OS, .NET version, etc.)
   - Screenshots if applicable

### Suggesting Enhancements

1. Check if the enhancement has been suggested
2. Create a new issue with:
   - Clear, descriptive title
   - Detailed description of the proposed feature
   - Use cases and benefits
   - Any implementation ideas

### Pull Requests

1. **Fork the repository**
2. **Create a feature branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```
3. **Make your changes**
   - Follow the coding standards (see below)
   - Add tests for new functionality
   - Update documentation as needed
4. **Commit your changes**
   ```bash
   git commit -m "Add: Brief description of changes"
   ```
5. **Push to your fork**
   ```bash
   git push origin feature/your-feature-name
   ```
6. **Create a Pull Request**
   - Provide a clear title and description
   - Reference any related issues
   - Ensure all tests pass

## Coding Standards

### C# Code Style
- Follow Microsoft's C# Coding Conventions
- Use meaningful variable and method names
- Add XML documentation comments for public APIs
- Keep methods focused and small
- Use async/await for I/O operations
- Handle exceptions appropriately

### Example
```csharp
/// <summary>
/// Retrieves a process definition by its unique identifier
/// </summary>
/// <param name="id">The process definition ID</param>
/// <returns>The process definition or null if not found</returns>
public async Task<ProcessDefinition?> GetProcessDefinitionAsync(Guid id)
{
    try
    {
        return await _repository.GetByIdAsync(id);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error retrieving process definition {ProcessId}", id);
        throw;
    }
}
```

### Testing
- Write unit tests for business logic
- Write integration tests for API endpoints
- Aim for 70%+ code coverage
- Use meaningful test names

### Git Commit Messages
- Use present tense ("Add feature" not "Added feature")
- Use imperative mood ("Move cursor to..." not "Moves cursor to...")
- Start with a capital letter
- Keep first line under 50 characters
- Add detailed description if needed

**Prefixes:**
- `Add:` New features
- `Fix:` Bug fixes
- `Update:` Updates to existing functionality
- `Remove:` Code removal
- `Refactor:` Code refactoring
- `Docs:` Documentation changes
- `Test:` Adding or updating tests

## Development Setup

1. **Install Prerequisites**
   - .NET 8 SDK
   - SQL Server or PostgreSQL
   - Docker (optional)

2. **Clone and Build**
   ```bash
   git clone https://github.com/AnwarFaiez/BPM_Project.git
   cd BPM_Project
   dotnet restore
   dotnet build
   ```

3. **Run Tests**
   ```bash
   dotnet test
   ```

4. **Run Services**
   ```bash
   cd src/Services/BPM.Services.Process
   dotnet run
   ```

## Project Structure

```
BPM_Project/
├── src/
│   ├── Core/              # Business logic
│   ├── Services/          # API services
│   └── Infrastructure/    # Cross-cutting concerns
├── tests/
│   ├── BPM.Tests.Unit/
│   └── BPM.Tests.Integration/
└── docs/                  # Documentation
```

## Questions or Need Help?

- Create an issue with the "question" label
- Reach out to maintainers
- Check existing documentation

Thank you for contributing! 🎉
