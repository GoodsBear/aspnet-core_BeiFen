using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Educational.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class EducationalDbContextFactory : IDesignTimeDbContextFactory<EducationalDbContext>
{
    public EducationalDbContext CreateDbContext(string[] args)
    {
        EducationalEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<EducationalDbContext>()
            .UseMySql(configuration.GetConnectionString("Default"),ServerVersion.Parse("5.7.22-mysql"));

        return new EducationalDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
