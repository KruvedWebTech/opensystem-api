using AutoMapper;
using opensystem_api.Models;
using System.Collections.Generic;

namespace opensystem_api.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, Dictionary<string, string>>()
                .AfterMap((src, dest) =>
                {
                    dest["UserId"] = src.UserId.ToString();
                    dest["Email"] = src.Email ?? "";
                    dest["FirstName"] = src.FirstName ?? "";
                    dest["LastName"] = src.LastName ?? "";
                    dest["ProfilePic"] = src.ProfilePic ?? "";
                    dest["Gender"] = src.Gender ?? "";
                    //dest["RoleId"] = src.RoleId.ToString();
                    //dest["CompanyId"] = src.CompanyId.ToString();
                   // dest["CreatedAt"] = src.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss");
                    //dest["CreatedOn"] = src.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss");
                    //dest["ModifiedAt"] = src.ModifiedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "";
                    //dest["ModifiedOn"] = src.ModifiedOn?.ToString("yyyy-MM-dd HH:mm:ss") ?? "";
                   // dest["IsActive"] = src.IsActive.ToString();
                    dest["Role"] = src.Role?.RoleName ?? "";
                    dest["Company"] = src.Company?.Name ?? "";
                });
        }
    }
}
