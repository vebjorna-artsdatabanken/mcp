# 🔷 C# Code Style Guidelines

## Documentation

 - Every class should have a comment that explains what it represents in the domain
 - Every method should have a comment that explains what it does and what it returns
 - No comments on properties

## 📂 File Organization

- One class per file
- Filename matches class name
- Usings at top, no `using` inside namespaces
- Remove unused usings
```csharp
using System;
using System.Collections.Generic;

namespace Contoso.Mail;
```

## 🏗️ Class Structure

- Properties first
- Constructors second
- Public methods third
- Private methods last
- Always override ToString()
```csharp
public class Contact
{
	public int ID { get; set; }
	public string Name { get; set; } = string.Empty;

	public Contact()
	{
	}

	public Contact(string name)
	{
		Name = name;
	}

	public override string ToString() => $"{Name} ({ID})";

	private void ValidateName()
	{
		// Implementation
	}
}
```

## 📝 Naming

- PascalCase for types and public members
- camelCase for parameters and local variables
- No Hungarian notation
- No underscores in names except for private fields
```csharp
public class EmailMessage
{
	private readonly ILogger _logger;
	
	public void SendEmail(string recipientEmail)
	{
		var messageContent = PrepareContent();
	}
}
```

## 🎯 Properties

- Use auto-implemented when possible
- Initialize collections in declaration
- Use init-only where appropriate
```csharp
public class Message
{
	public int ID { get; set; }
	public string Subject { get; set; } = string.Empty;
	public List<string> Tags { get; set; } = new();
	public string Slug { get; init; } = string.Empty;
}
```

## 🔄 Methods

- Expression-bodied for simple methods
- Verb-first naming for actions
```csharp
public class Order
{
	public decimal CalculateTotal() => Items.Sum(i => i.Price);

	public void AddItem(Item item)
	{
		if (item == null)
			throw new ArgumentNullException(nameof(item));

		Items.Add(item);
	}
}
```

## ⚡ Performance

- Use `StringBuilder` for string concatenation in loops
- Initialize collections with capacity when size is known
- Prefer `string.IsNullOrEmpty()` over `string.Empty` comparison
```csharp
public class Logger
{
	public string BuildReport(IEnumerable<LogEntry> entries)
	{
		var builder = new StringBuilder();
		foreach (var entry in entries)
		{
			builder.AppendLine(entry.ToString());
		}
		return builder.ToString();
	}
}
```

## 🎨 Formatting

- Use tabs for indentation
- Curly braces on new lines
- One statement per line
- Line break before LINQ chain
```csharp
public class DataProcessor
{
	public IEnumerable<string> ProcessItems(List<Item> items)
	{
		return items
			.Where(i => i.IsActive)
			.OrderBy(i => i.Name)
			.Select(i => i.ToString());
	}
}
```

## 🚫 Avoid

- Public fields
- Regions
- Optional parameters (except for testing)
- Multiple return statements
- Empty catch blocks
```csharp
// Don't
public string _publicField;
#region Properties
public bool IsValid { get; set; }
#endregion
```

## ✨ Modern Features

- Use pattern matching
- Target latest C# version
- Use init-only properties
- Use file-scoped namespaces
```csharp
public class Parser
{
	public string GetDisplayText(object item) => item switch
	{
		string s => s,
		int n => n.ToString(),
		DateTime d => d.ToShortDateString(),
		_ => item.ToString() ?? string.Empty
	};
}
```

## 🧪 Error Handling

- Throw exceptions for exceptional cases
- Use custom exceptions for domain errors
- Return result types for expected failures
```csharp
public class UserService
{
	public Result<User> CreateUser(string email)
	{
		if (string.IsNullOrEmpty(email))
			return Result<User>.Error("Email is required");

		if (!IsValidEmail(email))
			return Result<User>.Error("Invalid email format");

		var user = new User(email);
		return Result<User>.Success(user);
	}
}
```

## 🔒 Immutability

- Use init-only properties for immutable data
- Make classes sealed by default
- Use record types for DTOs
```csharp
public sealed record UserDto(int Id, string Name, string Email);

public sealed class Configuration
{
	public string ApiKey { get; init; } = string.Empty;
	public string BaseUrl { get; init; } = string.Empty;
}
```
