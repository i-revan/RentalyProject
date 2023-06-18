using AutoMapper;
using RentalyProject.Models;
using RentalyProject.ViewModels.Account;
using RentalyProject.ViewModels.Categories;
using RentalyProject.ViewModels.Models;

namespace RentalyProject
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<AppUser, RegisterVM>();
            CreateMap<RegisterVM, AppUser>();

            CreateMap<Model, ModelVM>();
            CreateMap<ModelVM,Model>();

            CreateMap<Category, UpdateCategoryVM>();
            CreateMap<UpdateCategoryVM, Category>();
        }
    }
}
