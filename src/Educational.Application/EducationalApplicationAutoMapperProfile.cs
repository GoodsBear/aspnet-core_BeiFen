using AutoMapper;
using AutoMapper;
using AutoMapper.Internal.Mappers;
using Educational.Organization;
using System.Collections.Generic;
using System.Linq;
using Educational.Announcements;
using Educational.Classgrade;
using Educational.Courses;
using Educational.Datadictionary;
using Educational.Dto.Announcements;
using Educational.Dto.Grades;
using Educational.Dto.Positions;
using Educational.Organization;
using Educational.Positions;
using Educational.RBAC;
using Educational.RBAC.PermissionsManager;
using Educational.RBAC.RoleManager;
using Educational.SpecialSubject;
using Educational.Staffs;
using Educational.StafRoles;
using Educational.StaffTypes;
using Educational.Classgrade;
using Educational.Dto.Grades;
using Educational.RolePerssions;
using Educational.Subject;
using System.Collections.Generic;
using System.Linq;
using Educational.Dto.ClassRooms;
using Educational.StaffTypes;
using Educational.Subject;
using System.Collections.Generic;
using System.Linq;

namespace Educational;

public class EducationalApplicationAutoMapperProfile : Profile
{
    public EducationalApplicationAutoMapperProfile()
    {
        //专题一套
        CreateMap<UpdateSpecialSubjectDto, SpecialSubjectModel>().ReverseMap();
        CreateMap<SpecialSubjectModel, SpecialSubjectDto>().ReverseMap();
        CreateMap<UpdateCategoryDto, CategoryModel>().ReverseMap();
        CreateMap<CategoryModel, CategoryModelDto>().ReverseMap();
        CreateMap<CategoryModel, CategoryModelDto>().ReverseMap();



        CreateMap<CreateUpdateOrganizationLevel, OrganizationLevel>();
        CreateMap<OrganizationLevel, OrganizationLevelDto>();
        // 组织机构映射
        CreateMap<CreateUpdateOrganizationDto, OrganizationModel>();
        CreateMap<OrganizationModel, OrganizationDto>();
        CreateMap<OrganizationModel, OrganizationTreeDto>().ReverseMap();
        CreateMap<OrganizationLevel, XialaLevelDto>().ReverseMap();
        CreateMap<OrganizationModel, OrganizationSelectDto>().ReverseMap();
        CreateMap<XialaLevelDto, OrganizationLevel>().ReverseMap();
        //   CreateMap<OrganizationLevel, XialaLevelDto>() .ForMember(dest => dest.LevelNameDto, opt => opt.MapFrom(src => src.Name));
        CreateMap<AddorUpdStaffDTO, StaffInfo>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<ShowStaffDTO, StaffInfo>().ReverseMap();
        CreateMap<StaffInfo, LoginReturnDTO>().ReverseMap();
        CreateMap<StaffTypeInfo, ShowStaffTypeDTO>().ReverseMap();
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organizationaaa. */
        CreateMap<Course, CourseDto>(MemberList.Source)
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
        //科目管理 
        CreateMap<UpdateSubjectDto, Educational.Subject.SubjectModel>().ReverseMap();
        CreateMap<Educational.Subject.SubjectModel, SubjectDto>().ReverseMap();

        CreateMap<DictTypeDto, DictType>().ReverseMap();

    }
} 