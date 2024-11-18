using AutoMapper;
namespace BookInfo.API.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile() {
            CreateMap<Entities.Book,Models.BooksWithoutClassesOfInterest>();
            CreateMap<Entities.Book,Models.BooksDto>();
        }
    }
}
