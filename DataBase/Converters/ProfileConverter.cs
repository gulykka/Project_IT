using Domain;
using DataBase.Models;

namespace DataBase.Converters;

public  static class ProfileConverter
{
   public static ProfileModel ToModel(this Profile domainProfile)
   {
      return new ProfileModel
      {
         Id = domainProfile.Id,
         Name = domainProfile.Name
      };
   }

   public static Profile ToDomain(this ProfileModel prof)
   {
      return new Profile(
         prof.Id,
         prof.Name
      );
   }
}