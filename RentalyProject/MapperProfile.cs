using AutoMapper;
using RentalyProject.Models;
using RentalyProject.ViewModels.Account;

namespace RentalyProject
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<AppUser, RegisterVM>();
            CreateMap<RegisterVM, AppUser>();
        }
    }
}
