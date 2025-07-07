using AutoMapper;
using AutoMapper.Internal.Mappers;
using Educational.Courses;
using Educational.Organization;
using Educational.Announcements;
using Educational.Classgrade;
using Educational.ClassSchedule;
using Educational.ClassSchedule.DTO;
using Educational.ClassSchedule.Update;
using Educational.Datadictionary;
using Educational.Dto.Announcements;
using Educational.Dto.ClassRooms;
using Educational.Dto.Clbums;
using Educational.Dto.Grades;
using Educational.Dto.MaterialDtos;
using Educational.Dto.Positions;
using Educational.Materials;
using Educational.Menu;
using Educational.Organization;
using Educational.Positions;
using Educational.RBAC;
using Educational.RBAC.PermissionsManager;
using Educational.RBAC.RoleManager;
using Educational.RolePerssions;
using Educational.SalarySetting;
using Educational.SpecialSubject;
using Educational.Staffs;
using Educational.Staffs.SalarySetting;
using Educational.StaffTypes;
using Educational.StafRoles;
using Educational.Subject;
using Educational.Menu;
using Educational.StudentsAndParents.Students;
using Educational.Dto.MaterialRecordsDtos;
using Educational.StudentsAndParents.StudentFollow;
using Educational.StudentsAndParends.Students.Follow;
using Educational.StudentsAndParends.Students.Store;
using Educational.StudentsAndParents.Stores;
using Educational.StudentsAndParents.StudentFollow;
using Educational.StudentsAndParents.Students;
using Educational.StudentsAndParents.StudentStore;
using Educational.StudentsAndParends.Students;
using Educational.Subject;

namespace Educational;

public class EducationalApplicationAutoMapperProfile : Profile
{
    public EducationalApplicationAutoMapperProfile()
    {
        //薪资
        CreateMap<Educational.Staffs.SalarySettingModel, SalarySettingDto>().ReverseMap();
        //排课  
        CreateMap<Educational.ClassSchedule.ClassSchedule, ClassScheduleDto>().ReverseMap();
        CreateMap<ConflictModel, ConflictModelDto>().ReverseMap();
        CreateMap<ConflictModel, ConflictModelDto>().ReverseMap();
        CreateMap<UpdateClassScheduleDto, Educational.ClassSchedule.ClassSchedule>().ReverseMap();
        //专题一套
        CreateMap<UpdateSpecialSubjectDto, SpecialSubjectModel>().ReverseMap();
        CreateMap<SpecialSubjectModel, SpecialSubjectDto>().ReverseMap();
        CreateMap<UpdateCategoryDto, CategoryModel>().ReverseMap();
        CreateMap<CategoryModel, CategoryModelDto>().ReverseMap();
        CreateMap<SpecialSubjectModel, XiAsepecialSubjectDto>().ReverseMap();
         
            
        CreateMap<CreateUpdateOrganizationLevel, OrganizationLevel>();
        CreateMap<OrganizationLevel, OrganizationLevelDto>();
        // 添加从 CreateUpdateOrganizationDto 到 OrganizationModel 的映射
        
        CreateMap<OrganizationModel, OrganizationTreepageDto>();
        // 组织机构映射
        CreateMap<CreateUpdateOrganizationDto, OrganizationModel>();
        CreateMap<OrganizationModel, OrganizationDto>();
        CreateMap<OrganizationModel, OrganizationTreeDto>().ReverseMap();
        CreateMap<OrganizationLevel, XialaLevelDto>().ReverseMap();
        CreateMap<OrganizationModel, OrganizationSelectDto>().ReverseMap();
        CreateMap<XialaLevelDto, OrganizationLevel>().ReverseMap();
        //   CreateMap<OrganizationLevel, XialaLevelDto>() .ForMember(dest => dest.LevelNameDto, opt => opt.MapFrom(src => src.Name));
       //成员
        CreateMap<AddorUpdStaffDTO, StaffInfo>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<ShowStaffDTO, StaffInfo>().ReverseMap();
        CreateMap<StaffInfo, LoginReturnDTO>().ReverseMap();
        CreateMap<StaffTypeInfo, ShowStaffTypeDTO>().ReverseMap(); 
        CreateMap<StaffInfo, StaffSelectDto>().ReverseMap();
        CreateMap<StaffInfo, StaffUpdateDTO>().ReverseMap();
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organizationaaa. */
        //课程映射
        CreateMap<CreateCourseDto, Course>().ReverseMap().ForMember(dest => dest.CourseName, pot => pot.MapFrom(src => src.CourseName)).ReverseMap();
        CreateMap<Course, CourseDto>(MemberList.Source)
            .ForMember(dest=>dest.CourseName,pot=>pot.MapFrom(src=>src.CourseName)).ReverseMap();
        CreateMap<Course, CourseSelectDto>(MemberList.Source)
            .ForMember(dest=>dest.CourseName,pot=>pot.MapFrom(src=>src.CourseName)).ReverseMap();
        // 职位映射
        CreateMap<Position, PositionsDto>().ReverseMap();
        CreateMap<CreateUpdatePositionDto, Position>().ReverseMap();
        CreateMap<Position, ExportPositionDto>().ReverseMap();
        // 公告映射
        CreateMap<Announcement, AnnouncementDto>().ReverseMap();
        CreateMap<CreateUpdateAnnouncement, Announcement>().ReverseMap();
        // 年级映射
        CreateMap<Grade, GradeDto>().ReverseMap();
        CreateMap<CreateUpdateGradeDto, Grade>().ReverseMap();
        CreateMap<Grade, GradeSelectDto>().ReverseMap();
        // 角色映射
        CreateMap<CreateUpdateRoleDto, Role>().ReverseMap();
        CreateMap<Role, RoleDto>().ReverseMap();
        // 权限映射
        CreateMap<CreateUpdatePermissionsDto, Permissions>().ReverseMap();
        CreateMap<Permissions, PermissionsDto>().ReverseMap();
        // 角色权限映射
        CreateMap<RolePermission, RolePermissionDto>().ReverseMap();
        // 员工角色映射
        CreateMap<StaffRole, StaffRoleDto>().ReverseMap();
        CreateMap<StaffRole,RoleStaffDto>().ReverseMap();
        // 字典类型映射
        CreateMap<DictTypeDto, DictType>().ReverseMap();
        // 教室映射
        CreateMap<ClassRoom, ClassRoomDto>().ReverseMap();
        CreateMap<ClassRoom, ClassRoomSelectDto>().ReverseMap();
        CreateMap<CreateUpdateClassRoomDto, ClassRoom>().ReverseMap();
        // 班级映射
        CreateMap<ClassInfo, ClassInfoDto>().ReverseMap();
        CreateMap<ClassInfo, CreateUpdateClassDto>().ReverseMap();
        CreateMap<ClassInfo, ClassSelectDto>().ReverseMap();
        //科目管理 
        CreateMap<UpdateSubjectDto, Educational.Subject.SubjectModel>().ReverseMap();
        CreateMap<Educational.Subject.SubjectModel, SubjectDto>().ReverseMap();
        CreateMap<Educational.Subject.SubjectModel, XialaSubjectDto>().ReverseMap();
        CreateMap<DictTypeDto, DictType>().ReverseMap();

		//物料
		CreateMap<Material, MaterialDto>().ReverseMap();
		CreateMap<CreateUpdateMaterialDto,Material>().ReverseMap();
        CreateMap<Material, MaterialSelectDto>().ReverseMap();
        CreateMap<MaterialRecords,CreateMaterialRecordDto>().ReverseMap();
        //出入库
        CreateMap<MaterialRecords, MaterialRecordsDto>().ReverseMap();
        CreateMap<CreateUpdateMaterialRecordsDto, MaterialRecords>().ReverseMap();
        
        //动态菜单
        CreateMap<CreateUpdateMenuDto,Educational.Menu.Menu>().ReverseMap();
        CreateMap<Educational.Menu.Menu, MenuDto>().ReverseMap();
        CreateMap<Educational.Menu.Menu, Educational.Shared.Models.MenuInfo>().ReverseMap();

        //学员
        CreateMap<CreateUpdateStudentDto, Educational.StudentsAndParends.Students.Student>().ReverseMap();
        CreateMap<Educational.StudentsAndParends.Students.Student, StudentsDto>().ReverseMap();
        CreateMap<Student, StudentSelectDto>().ReverseMap();
        //学员跟进
        CreateMap<CreateUpdateFollowDto,Follow>().ReverseMap();
        CreateMap<Follow, FollowDto>();
        CreateMap<MiddleCreateUpateDto, StudentFollowRelation>().ReverseMap();

        //学员积分
        CreateMap<CreateUpdateStoreDto, Store>().ReverseMap();
        CreateMap<Store, StoreDto>();
        CreateMap<MeddleStoreDto, StudentStoreRelation>().ReverseMap();

        //薪资
        CreateMap<SalarySettingModel, SalarySettingDto>().ReverseMap();
       
    }
} 