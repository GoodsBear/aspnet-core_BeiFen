using Educational.Classgrade;
using Educational.Courses;
using Educational.Students;
using Educational.Organization;
using Educational.Announcements;
using Educational.Positions;
using Educational.RBAC;
using Educational.Staffs;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Educational.StaffTypes;

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
   
	public DbSet<Student> Student { get; set; } //学生
	public DbSet<Class> Class { get; set; }  //班级
    public DbSet<Grade> Grade { get; set; }   //年级
    public DbSet<Parent> Parent { get; set; }//家长
    public DbSet<Course> Course { get; set; } //课程信息表
    public DbSet<StaffInfo> staffInfos { get; set; }//员工信息表
    public DbSet<OrganizationModel> OrganizationModels { get; set; }//组织信息表
    public DbSet<OrganizationLevel> OrganizationLevels { get; set; }//组织级别表
    public DbSet<Position> positions { get; set; }//职位信息表
    public DbSet<Announcement> announcements { get; set; }//公告信息表
    public DbSet<Role> Role { get; set; } //角色信息表
    public DbSet<Educational.RBAC.Permissions> Permissions { get; set; } //权限信息表
    public DbSet<StaffTypeInfo> StaffTypeInfos { get; set; }//人员类型信息表
    public DbSet<Educational.SubjectModel.SubjectModel> SubjectModel { get; set; }//科目表
    public DbSet<Educational.Materials.Material> Material { get; set; }//物料表
    public DbSet<Educational.Materials.MaterialRecords> MaterialRecords { get; set; }//物料出入库记录表
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
        /// 人员类型信息表
        /// </summary>
        builder.Entity<StaffTypeInfo>(b =>
        {
            b.ToTable(EducationalConsts.DbTablePrefix + "StaffTypeInfo", EducationalConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            //...
        });
        /* Configure your own tables/entities inside here */

        builder.Entity<Student>(b =>
        {
            b.ToTable(EducationalConsts.DbTablePrefix + "Student", EducationalConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Name).IsRequired().HasMaxLength(50);
        });
        builder.Entity<Class>(b =>
        {
            b.ToTable(EducationalConsts.DbTablePrefix + "Class", EducationalConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.ClassName).IsRequired().HasMaxLength(50);
        });
        builder.Entity<Grade>(b =>
        {
            b.ToTable(EducationalConsts.DbTablePrefix + "Grade", EducationalConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.GradeName).IsRequired().HasMaxLength(50);
        });
		builder.Entity<Parent>(b =>
		{
			b.ToTable(EducationalConsts.DbTablePrefix + "Parent", EducationalConsts.DbSchema);
			b.ConfigureByConvention(); //auto configure for the base class props
			b.Property(x => x.PardentName).IsRequired().HasMaxLength(50);
		});
		builder.Entity<Course>(b =>
		{
			b.ToTable(EducationalConsts.DbTablePrefix + "Course", EducationalConsts.DbSchema);
			b.ConfigureByConvention(); //auto configure for the base class props
			b.Property(x => x.CourseName).IsRequired().HasMaxLength(50);
		});
	}
}
