using EFCore.NamingConventions.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata; // Npgsql 驱动自身的元数据命名空间
using System;
using System.IO;

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
            .UseMySql(configuration.GetConnectionString("Default"),ServerVersion.Parse("5.7.22-mysql")); //mysql

            //.UseNpgsql(configuration.GetConnectionString("Default"));

            //builder.UseNpgsql(configuration.GetConnectionString("Default"), options =>
            //{
            //    options.MigrationsHistoryTable("__EFMigrationsHistory");
            //})
            //.UseSnakeCaseNamingConvention();

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
