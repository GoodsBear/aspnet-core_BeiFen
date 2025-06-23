using AutoMapper;
using Educational.Courses;

namespace Educational;

public class EducationalApplicationAutoMapperProfile : Profile
{
    public EducationalApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organizationaaa. */
        CreateMap<Course, CourseDto>(MemberList.Source)
            .ForMember(dest=>dest.CourseName,pot=>pot.MapFrom(src=>src.CourseName)).ReverseMap();
    }
}
