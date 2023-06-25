﻿using AutoMapper;
using RentalyProject.Models;
using RentalyProject.Utilities.Extensions;
using RentalyProject.ViewModels.Account;
using RentalyProject.ViewModels.Categories;
using RentalyProject.ViewModels.DynamicSections;
using RentalyProject.ViewModels.Faqs;
using RentalyProject.ViewModels.Models;
using RentalyProject.ViewModels.Newss;
using RentalyProject.ViewModels.Services;
using RentalyProject.ViewModels.Tags;

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

            CreateMap<Service, ServiceVM>().ReverseMap();

            CreateMap<Tag, TagVM>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Capitalize()))
           .ReverseMap()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Capitalize()));

            CreateMap<News, CreateNewsVM>().ReverseMap();
            CreateMap<News, UpdateNewsVM>().ReverseMap();

            CreateMap<DynamicSection,CreateDynamicSectionVM>().ReverseMap();
            CreateMap<UpdateDynamicSectionVM,DynamicSection>().ReverseMap();

            CreateMap<Faq, FaqVM>().ReverseMap();
        }
    }
}
