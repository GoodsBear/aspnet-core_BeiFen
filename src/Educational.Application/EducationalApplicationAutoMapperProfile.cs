using AutoMapper;
using Educational.RBAC;
using Educational.RBAC.PermissionsManager;
using Educational.RBAC.RoleManager;

namespace Educational;

public class EducationalApplicationAutoMapperProfile : Profile
{
    public EducationalApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organizationaaa. */

        CreateMap<CreateUpdateRoleDto, Role>().ReverseMap();
        CreateMap<Role, RoleDto>().ReverseMap();


        CreateMap<CreateUpdatePermissionsDto, Permissions>().ReverseMap();
        CreateMap<Permissions, PermissionsDto>().ReverseMap();

    }
}
