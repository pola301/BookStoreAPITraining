using AutoMapper;
using BookInfo.API.Models;
using BookInfo.API.Entities;

public class ClassesOfInterestProfile : Profile
{
    public ClassesOfInterestProfile()
    {
        // Map from ClassOfInterest to ClassesOfInterest
        CreateMap<ClassOfInterest, ClassesOfInterest>().ReverseMap();

        // Map from ClassesOfInterestForCreationdto to ClassOfInterest
        CreateMap<ClassesOfInterestForCreationdto, ClassOfInterest>().ReverseMap();
        CreateMap<ClassesOfInterestForUpdate, ClassOfInterest>();
        CreateMap<ClassOfInterest, ClassesOfInterestForUpdate>();


    }
}
