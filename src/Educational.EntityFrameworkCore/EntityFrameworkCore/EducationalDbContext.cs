using Educational.Announcements;
using Educational.Organization;
using Educational.Positions;
using Educational.RBAC;
using Educational.Staffs;
using Educational.Subject;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace Educational.EntityFrameworkCore;

[ConnectionStringName("Default")]
public class EducationalDbContext :
    AbpDbContext<EducationalDbContext>
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity

    #endregion

    public EducationalDbContext(DbContextOptions<EducationalDbContext> options)
        : base(options)
    {

    }

    public DbSet<StaffInfo> staffInfos { get; set; }
    public DbSet<OrganizationModel> OrganizationModels { get; set; }
    public DbSet<OrganizationLevel> OrganizationLevels { get; set; }
    public DbSet<Position> positions { get; set; }
    public DbSet<Announcement> announcements { get; set; }
    public DbSet<Role> Role { get; set; } //角色表
    public DbSet<Educational.RBAC.Permissions> Permissions { get; set; } //权限表
    
    public DbSet<SubjectModel> SubjectModel { get; set; } //角色表

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        
        builder.Entity<StaffInfo>(b =>
        {
            b.ToTable(EducationalConsts.DbTablePrefix + "StaffInfo", EducationalConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            //...
        });


        /// <summary>
        /// 组织机构表
        /// </summary>
        builder.Entity<OrganizationModel>(b =>
        {
            b.ToTable(EducationalConsts.DbTablePrefix + "OrganizationModel", EducationalConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            //...
        });

        /// <summary>
        /// 组织机构级别表
        /// </summary>
        builder.Entity<OrganizationLevel>(b =>
        {
            b.ToTable(EducationalConsts.DbTablePrefix + "OrganizationLevel", EducationalConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            //...
        });

        /// <summary>
        ///  科目管理表
        /// </summary>   
        builder.Entity<SubjectModel>(b =>
        {
            b.ToTable(EducationalConsts.DbTablePrefix + "SubjectModels", EducationalConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            //...
        });
    }
}
