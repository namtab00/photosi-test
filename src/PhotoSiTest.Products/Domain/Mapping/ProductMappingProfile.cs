using PhotoSiTest.Common.Mapping;
using PhotoSiTest.Contracts.Domain.Products.Dtos;

namespace PhotoSiTest.Products.Domain.Mapping;

public class ProductMappingProfile : MappingProfileBase
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductDto>();

        CreateMap<CreateProductDto, Product>().ForMember(d => d.Id, opt => opt.Ignore()).ForMember(d => d.Category, opt => opt.Ignore());

        CreateMap<UpdateProductDto, Product>().ForMember(d => d.Id, opt => opt.Ignore()).ForMember(d => d.Category, opt => opt.Ignore());

        CreateMap<ProductCategory, ProductCategoryDto>();

        CreateMap<CreateProductCategoryDto, ProductCategory>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.Products, opt => opt.Ignore());

        CreateMap<UpdateProductCategoryDto, ProductCategory>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.Products, opt => opt.Ignore());
    }
}
