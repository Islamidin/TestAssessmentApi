using Api.Models;
using AutoMapper;
using Database.Entities;

namespace Api.Profiles;

public class PeopleProfile : Profile
{
    public PeopleProfile()
    {
        CreateMap<Person, PersonDetails>();
        CreateMap<NewPersonDetails, Person>();
        CreateMap<UpdatePersonDetails, Person>();
    }
}