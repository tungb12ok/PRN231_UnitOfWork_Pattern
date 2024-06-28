using AutoMapper;
using JewelryAuctionBusiness.Dto;
using JewelryAuctionData.Dto;
using JewelryAuctionData.Entity;

namespace JewelryAuctionBusiness.AutoMap
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map Customer to CustomerDTO and reverse
            CreateMap<Customer, CustomerDTO>()
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company))
                .ReverseMap();

            // Map Company to CompanyDTO and reverse
            CreateMap<Company, CompanyDTO>()
                .ReverseMap();

            // Map AuctionSectionDto to AuctionSection
            CreateMap<AuctionSectionDto, AuctionSection>()
                .ForMember(dest => dest.JewelryId, opt => opt.MapFrom(src => src.JewelryID))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Discription, opt => opt.MapFrom(src => src.Discription))
                .ForMember(dest => dest.InitialPrice, opt => opt.MapFrom(src => src.InitialPrice))
                .ForMember(dest => dest.Bidder, opt => opt.MapFrom(src => src.BidderDto));

            // Map AuctionSection to AuctionSectionDto
            CreateMap<AuctionSection, AuctionSectionDto>()
                .ForMember(dest => dest.JewelryID, opt => opt.MapFrom(src => src.JewelryId))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Discription, opt => opt.MapFrom(src => src.Discription))
                .ForMember(dest => dest.InitialPrice, opt => opt.MapFrom(src => src.InitialPrice))
                .ForMember(dest => dest.BidderDto, opt => opt.MapFrom(src => src.Bidder));

            // Map Bidder to BidderDto and reverse
            CreateMap<Bidder, BidderDto>()
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.CurrentBidPrice))
                .ReverseMap();
        }
    }
}
