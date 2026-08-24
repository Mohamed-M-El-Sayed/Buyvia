using AutoMapper;
using OnlineStore.Application.Features.Reviews.Commands.CreateReview;
using OnlineStore.Application.Features.Reviews.Commands.UpdateReview;
using OnlineStore.Application.Features.Reviews.Dtos;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Application.Features.Reviews.Mappings
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<CreateReviewCommand, Review>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
            CreateMap<UpdateReviewCommand, Review>();

            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.PurchasedVariantName,
                     opt => opt.MapFrom(src => src.PurchasedVariant != null
                           ? src.PurchasedVariant.GetVariantName() : null))
                .ForMember(dest => dest.ReviewerName,
                     opt => opt.MapFrom(src => src.User.FullName));

        }
    }
}
