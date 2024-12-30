using FinanceBlazor.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<RecurringExpense> RecurringExpenses { get; set; }
    public DbSet<Income> Incomes { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure Category entity
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id); // Primary key
            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(c => c.Description)
                .HasMaxLength(255);
        });

        // Configure RecurringExpense entity
        modelBuilder.Entity<RecurringExpense>(entity =>
        {
            entity.HasKey(e => e.Id); // Primary key
            entity.Property(e => e.Name)
                .HasMaxLength(100);
            entity.Property(e => e.Description)
                .HasMaxLength(255);
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18,2)") // Precise decimal type for money
                .IsRequired();
            entity.Property(e => e.PaymentFrequency)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.PaymentDate)
                .IsRequired();

            // Configure relationship with Category
            entity.HasOne(e => e.Category)
                .WithMany(c => c.Expenses)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Cascade); // Delete expenses if the category is deleted
        });

        // Configure Income entity
        modelBuilder.Entity<Income>(entity =>
        {
            entity.HasKey(i => i.Id); // Primary key
            entity.Property(i => i.TotalAmount)
                .HasColumnType("decimal(18,2)") // Precise decimal type for money
                .IsRequired();
            entity.Property(i => i.PayDate)
                .IsRequired();
            entity.Property(i => i.Spending)
                .HasColumnType("decimal(18,2)");
            entity.Property(i => i.Savings)
                .HasColumnType("decimal(18,2)");
        });

        // Global settings for soft delete (optional)
        // For all entities, set IsDeleted to false by default
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var isDeletedProperty = entityType.FindProperty("IsDeleted");
            if (isDeletedProperty != null && isDeletedProperty.ClrType == typeof(bool))
            {
                modelBuilder.Entity(entityType.ClrType).Property("IsDeleted").HasDefaultValue(false);
            }
        }
    }

    // Optional: Add a timestamp tracking functionality
    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is BaseEntity baseEntity)
            {
                if (entry.State == EntityState.Added)
                {
                    baseEntity.CreatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    baseEntity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }
        return base.SaveChanges();
    }
}

// Optional BaseEntity to add created/updated timestamps
public abstract class BaseEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}