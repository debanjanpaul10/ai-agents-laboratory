using AIAgents.Laboratory.Domain.DomainEntities.FeedbackEntities;
using Microsoft.EntityFrameworkCore;

namespace AIAgents.Laboratory.Persistence.SQLDatabase.Context;

/// <summary>
/// The SQL database context class.
/// </summary>
/// <seealso cref="DbContext"/>
public partial class SqlDbContext : DbContext
{
    /// <summary>
	/// Initializes a new instance of the <see cref="SqlDbContext"/> class.
	/// </summary>
	/// <remarks>
	/// See <see href="https://aka.ms/efcore-docs-dbcontext">DbContext lifetime, configuration, and initialization</see>
	/// for more information and examples.
	/// </remarks>
    public SqlDbContext()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlDbContext"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    public SqlDbContext(DbContextOptions<SqlDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the Bug report details.
    /// </summary>
    /// <value>
    /// The bug report details.
    /// </value>
    public virtual DbSet<BugReportData> BugReportDetails { get; set; }

    /// <summary>
    /// Gets or sets the New feature requests.
    /// </summary>
    /// <value>
    /// The new feature requests.
    /// </value>
    public virtual DbSet<NewFeatureRequestData> NewFeatureRequests { get; set; }

    /// <summary>
	/// Override this method to configure the database (and other options) to be used for this context.
	/// This method is called for each instance of the context that is created.
	/// The base implementation does nothing.
	/// </summary>
	/// <param name="optionsBuilder">A builder used to create or modify options for this context. Databases (and other extensions)
	/// typically define extension methods on this object that allow you to configure the context.</param>
	/// <remarks>
	/// <para>
	/// In situations where an instance of <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions" /> may or may not have been passed
	/// to the constructor, you can use <see cref="P:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.IsConfigured" /> to determine if
	/// the options have already been set, and skip some or all of the logic in
	/// <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)" />.
	/// </para>
	/// <para>
	/// See <see href="https://aka.ms/efcore-docs-dbcontext">DbContext lifetime, configuration, and initialization</see>
	/// for more information and examples.
	/// </para>
	/// </remarks>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    /// <summary>
    /// Override this method to further configure the model that was discovered by convention from the entity types
    /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
    /// and re-used for subsequent instances of your derived context.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
    /// define extension methods on this object that allow you to configure aspects of the model that are specific
    /// to a given database.</param>
    /// <remarks>
    /// <para>
    /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
    /// then this method will not be run. However, it will still run when creating a compiled model.
    /// </para>
    /// <para>
    /// See <see href="https://aka.ms/efcore-docs-modeling">Modeling entity types and relationships</see> for more information and
    /// examples.
    /// </para>
    /// </remarks>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BugReportData>().HasKey(brd => brd.Id);
    }
}
