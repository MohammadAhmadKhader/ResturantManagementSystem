using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ResturantManagementSystem.Models;
using ResturantManagementSystem.ViewModels;

namespace ResturantManagementSystem.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<IdentityUser, RmsUserVM>().ReverseMap();
			CreateMap<IdentityUser, RmsUserUpdateRolesVM>()
                .ForMember(dest => dest.Roles,(opt => opt.Ignore()))
				.ForMember(dest => dest.SelectedRoles, (opt => opt.Ignore()))
				.ReverseMap();

			CreateMap<Chef, ChefVM>()
                .ForMember(dest => dest.ChefName, (opt => opt.MapFrom(src => src.User == null ? null : src.User.UserName)))
				.ForMember(dest => dest.Email, (opt => opt.MapFrom(src => src.User == null ? null : src.User.Email)));
            CreateMap<Chef, ChefCreateVM>()
				.ForMember(dest => dest.Email, (opt => opt.MapFrom(src => src.User == null ? null : src.User.Email)))
                .ReverseMap();

            CreateMap<Ingredient, IngredientVM>()
                .ReverseMap()
                .ForMember(dest => dest.FoodIngredients,(opt => opt.Ignore()));

            CreateMap<Food, FoodVM>()
                .ForMember(dest => dest.Ingredients, (opt => opt.MapFrom(src => src.FoodIngredients.Select(fi => fi.Ingredient))))
                .ReverseMap();

            CreateMap<Food, FoodCreateVM>()
                .ForMember(dest => dest.ImageUrl, (opt => opt.MapFrom(src => src.ImageUrl == null ? null : src.ImageUrl)))
                .ForMember(dest => dest.Ingredients, (opt => opt.MapFrom(src => src.FoodIngredients.Select(fi => fi.Ingredient))))
                .ForMember(dest => dest.SelectedIngredients, (opt => opt.MapFrom(src =>
                    src.FoodIngredients.Select(fi => fi.Id)
                )))
                .ReverseMap();

            CreateMap<Food, FoodUpdateVM>()
                .ForMember(dest => dest.ImageUrl, (opt => opt.MapFrom(src => src.ImageUrl == null ? null : src.ImageUrl)))
                .ForMember(dest => dest.Ingredients, (opt => opt.MapFrom(src => src.FoodIngredients.Select(fi => fi.Ingredient))))
                .ForMember(dest => dest.SelectedIngredients, (opt => opt.MapFrom(src =>
                    src.FoodIngredients.Select(fi => fi.Id)
                )))
                .ReverseMap();

            CreateMap<Order, OrderVM>()
                .ForMember(dest => dest.Chef, (opt => opt.MapFrom(src => src.Chef == null ? null : src.Chef)))
                .ForMember(dest => dest.OrdersFoodsList, (opt => opt.MapFrom(src => src.OrderFoods == null ? null : src.OrderFoods.ToList())))
                .ReverseMap();
            CreateMap<Order, OrderCreateStep1VM>().ReverseMap();
            CreateMap<Order, OrderUpdateVM>().ReverseMap();
        }

    }
}
