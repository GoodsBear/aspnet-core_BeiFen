using AutoMapper;
using Educational.Courses;
using AutoMapper;
using AutoMapper.Internal.Mappers;
using Educational.Announcements;
using Educational.Dto.Announcements;
using Educational.Dto.Positions;
using Educational.Organization;
using Educational.Positions;
using Educational.RBAC;
using Educational.RBAC.PermissionsManager;
using Educational.RBAC.RoleManager;
using Educational.Staffs;
using Educational.Subject;
using System.Collections.Generic;
using System.Linq;

namespace Educational;

public class EducationalApplicationAutoMapperProfile : Profile
{
    public EducationalApplicationAutoMapperProfile()
    {

        CreateMap<CreateUpdateOrganizationLevel, OrganizationLevel>();
        CreateMap<OrganizationLevel, OrganizationLevelDto>();
        // 添加从 CreateUpdateOrganizationDto 到 OrganizationModel 的映射
        CreateMap<CreateUpdateOrganizationDto, OrganizationModel>();
        CreateMap<OrganizationModel, OrganizationDto>();
        CreateMap<OrganizationModel, OrganizationTreeDto>().ReverseMap();
        CreateMap<OrganizationLevel, XialaLevelDto>().ReverseMap();

        CreateMap<XialaLevelDto, OrganizationLevel>().ReverseMap();
        //   CreateMap<OrganizationLevel, XialaLevelDto>() .ForMember(dest => dest.LevelNameDto, opt => opt.MapFrom(src => src.Name));
        CreateMap<AddorUpdStaffDTO, StaffInfo>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<ShowStaffDTO, StaffInfo>().ReverseMap();
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

        CreateMap<CreateUpdateRoleDto, Role>().ReverseMap();
        CreateMap<Role, RoleDto>().ReverseMap();
         
        CreateMap<CreateUpdatePermissionsDto, Permissions>().ReverseMap();
        CreateMap<Permissions, PermissionsDto>().ReverseMap();
        //科目管理 
        CreateMap<UpdateSubjectDto, SubjectModel>().ReverseMap();
        CreateMap<SubjectModel, SubjectDto>().ReverseMap();
        CreateMap<SubjectModel, XialaSubjectDto>().ReverseMap();
    }
}

