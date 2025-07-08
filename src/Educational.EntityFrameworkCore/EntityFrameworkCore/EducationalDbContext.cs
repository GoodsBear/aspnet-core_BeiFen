using Educational.Announcements;
using Educational.Classgrade;
using Educational.ClassSchedule;
using Educational.Courses;
using Educational.Datadictionary;
using Educational.Menu;
using Educational.Organization;
using Educational.Positions;
using Educational.RBAC;
using Educational.SalarySetting;
using Educational.SpecialSubject;
using Educational.Staffs;
using Educational.StaffTypes;
using Educational.StudentsAndParends.Parents;
using Educational.StudentsAndParends.Students;
using Educational.StudentsAndParends.Students.EnrollmentRecords;
using Educational.StudentsAndParends.Students.Follow;
using Educational.StudentsAndParends.Students.Store;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
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
    public DbSet<Educational.Menu.Menu>Menu { get; set; } //动态菜单表

    //#region 学生和家长相关表
    public DbSet<Student> Student { get; set; } //学生
    public DbSet<EnrollmentRecord> EnrollmentRecord { get; set; } //学生报名记录表
    public DbSet<Follow> Follow { get; set; } //学生跟进记录表
    public DbSet<StudentFollowRelation> StudentFollowRelation { get; set; } //学生跟进记录表中间表
    public DbSet<Store>Store { get; set; } //学生积分变动记录表
    public DbSet<StudentStoreRelation> StudentStoreRelations { get; set; } //学生积分变动记录表中间表
    public DbSet<Parent> Parent { get; set; }//家长
    public DbSet<ClassInfo> ClassInfo { get; set; }  //班级
    public DbSet<ClassRoom> ClassRoom { get; set; } //教室信息表
    public DbSet<Grade> Grade { get; set; }   //年级
    public DbSet<Course> Course { get; set; } //课程信息表

    // #endregion




    public DbSet<StaffTypeInfo> StaffTypeInfos { get; set; }//人员类型信息表
    public DbSet<DictType> DictTypes { get; set; }//数据字典类型表
    public DbSet<DictItem> DictItems { get; set; }//数据字典数据表
    public DbSet<Educational.Subject.SubjectModel> SubjectModel { get; set; }//科目表
    public DbSet<Educational.Materials.Material> Material { get; set; }//物料表
    public DbSet<Educational.Materials.MaterialRecords> MaterialRecords { get; set; }//物料出入库记录表
    public DbSet<SpecialSubjectModel> SpecialSubjectModel { get; set; }//专题名称
    public DbSet<CategoryModel> CategoryModel { get; set; }//专题级别名称
    public DbSet<Educational.ClassSchedule.ClassSchedule> ClassSchedule { get; set; }//排课表
    public DbSet<ScheduleTime> ScheduleTime { get; set; }//排课子表--上课时间表
    public DbSet<ConflictModel> ConflictModel { get; set; }//排课子表--冲突表

   //#region 组织管理
    public DbSet<OrganizationModel> OrganizationModels { get; set; }//组织信息表
    public DbSet<OrganizationLevel> OrganizationLevels { get; set; }//组织级别表
    public DbSet<Position> positions { get; set; }//职位信息表

    //#endregion
    //#region 系统管理

    public DbSet<StaffInfo> staffInfos { get; set; }//员工信息表
    public DbSet<StaffRole> StaffRole { get; set; } //员工角色中间表
    public DbSet<Role> Role { get; set; } //角色信息表

    //public DbSet<MenuPermissionsRelation> MenuPermissionsRelation { get; set; } //权限菜单中间表
    public DbSet<Educational.RBAC.Permissions> Permissions { get; set; } //权限信息表
    public DbSet<RolePermission> RolePermision { get; set; } //角色权限中间表
    public DbSet<Announcement> announcements { get; set; } //公告信息表

    public DbSet<SalarySettingModel> SalarySettingModel { get; set; } //薪资表

    //#endregion

    public DbSet<ClassHourFeeSetting> ClassHourFeeSetting { get; set; }//上课时间表

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
        //薪资表
        // 配置SalarySetting聚合根
        builder.Entity<ClassHourFeeSetting>(b =>
        {
            b.ToTable(EducationalConsts.DbTablePrefix + "ClassHourFeeSetting", EducationalConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
        });

        /// <summary>
        /// 排课子表--冲突表
        /// </summary>
        builder.Entity<ConflictModel>(b =>
        {
            b.ToTable(EducationalConsts.DbTablePrefix + "ConflictModel", EducationalConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            //...
        });

        /// <summary>
        /// 排课表
        /// </summary>
        builder.Entity<Educational.ClassSchedule.ClassSchedule>(b =>
        {
            b.ToTable(EducationalConsts.DbTablePrefix + "ClassSchedule", EducationalConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            //...
        });


        /// <summary>
        ///排课子表--上课时间表
        /// </summary>
        builder.Entity<ScheduleTime>(b =>
        {
            b.ToTable(EducationalConsts.DbTablePrefix + "ScheduleTime", EducationalConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            //...
        });


        /// <summary>
        /// 专题列表
        /// </summary>
        builder.Entity<SpecialSubjectModel>(b =>
        {
            b.ToTable(EducationalConsts.DbTablePrefix + "SpecialSubjectModel", EducationalConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            //...
        });

        /// <summary>
        /// 专题级别
        /// </summary>
        builder.Entity<CategoryModel>(b =>
        {
            b.ToTable(EducationalConsts.DbTablePrefix + "CategoryModel", EducationalConsts.DbSchema);
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
        builder.Entity<Educational.Subject.SubjectModel>(b =>
        {
            b.ToTable(EducationalConsts.DbTablePrefix + "SubjectModel", EducationalConsts.DbSchema);
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

        /// <summary>
        /// 数据字典类型表
        /// </summary>
        builder.Entity<DictType>(b =>
        {
            b.ToTable("dict_type");
            b.Property(x => x.Code).IsRequired().HasMaxLength(64);
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.Property(x => x.Description).HasMaxLength(255);
            b.Property(x => x.IsEnabled).HasDefaultValue(true);
        });

        /// <summary>
        /// 数据字典数据表
        /// </summary>
        builder.Entity<DictItem>(b =>
        {
            b.ToTable("dict_item");
            b.Property(x => x.Code).IsRequired().HasMaxLength(64);
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.Property(x => x.SortOrder).HasDefaultValue(0);
            b.Property(x => x.IsEnabled).HasDefaultValue(true);

            b.HasOne(x => x.DictType)
             .WithMany()
             .HasForeignKey(x => x.DictTypeId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        /* Configure your own tables/entities inside here */

        builder.Entity<Student>(b =>
        {
            b.ToTable(EducationalConsts.DbTablePrefix + "Student", EducationalConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Name).IsRequired().HasMaxLength(50);
        });
        builder.Entity<ClassInfo>(b =>
        {
            b.ToTable(EducationalConsts.DbTablePrefix + "ClassInfo", EducationalConsts.DbSchema);
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
