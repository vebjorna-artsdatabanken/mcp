# 🎯 Entity Framework Code Style Guidelines

## General

All EF assets should go into the `/EF` directory in the root.

## 📝 Naming Conventions
- DbContext classes: Suffix with `Context`
- Entity classes: Singular form
- DbSet properties: Plural form
- 
```csharp
public class MailContext : DbContext
{
  public DbSet<Message> Messages { get; set; }
  public DbSet<Contact> Contacts { get; set; }
}
```

## 🏗️ Entity Structure
```csharp
public class Message
{
  public int ID { get; set; }
  public string Subject { get; set; }
  public string Body { get; set; }
  public DateTime CreatedAt { get; set; }

  // Navigation properties
  public virtual ICollection<Contact> Recipients { get; set; }

  //override ToString() with something that makes sense

  //Create a ToJson() here as well
}
```

## 🔄 Relationship Conventions
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
  modelBuilder.Entity<Message>()
    .HasMany(m => m.Recipients)
    .WithMany(c => c.Messages);
}
```

## 🔍 Query Patterns
```csharp
// Prefer
var messages = await context.Messages
  .Include(m => m.Recipients)
  .Where(m => m.CreatedAt > startDate)
  .ToListAsync();

// Avoid
var messages = context.Messages
  .Where(m => m.CreatedAt > startDate)
  .ToList();
```

## 🚫 Common Anti-patterns
- Avoid raw SQL queries when EF Core can handle it
- Don't use `AsNoTracking()` by default
- Avoid loading entire tables without filters

## 🔒 Transaction Management
```csharp
using var transaction = await context.Database.BeginTransactionAsync();
try
{
  // Operations
  await context.SaveChangesAsync();
  await transaction.CommitAsync();
}
catch
{
  await transaction.RollbackAsync();
  throw;
}
```