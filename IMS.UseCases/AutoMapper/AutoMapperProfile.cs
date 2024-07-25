using AutoMapper;
using IMS.CoreBusiness;
using IMS.UseCases.ViewModels;

namespace IMS.UseCases.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<InventoryViewModel, Inventory>().ReverseMap();
        }
    }
}
