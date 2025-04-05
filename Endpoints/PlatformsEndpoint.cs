using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Endpoints
{
    public static class PlatformEndpoint
    {
        const string BaseUrl = "api/platforms";
        const string GetPlatforms = "GetPlatforms";
        const string GetPlatformById = "GetPlatformById";
        const string PostPlatform = "PostPlatform";

        
        public static void MapPlatformEndpoint(this WebApplication app)
        {   
            var group = app.MapGroup(BaseUrl);
            //platforms
            group.MapGet("/", (IPlatformRepo repo,IMapper mapper) =>
            {
                return Results.Ok(mapper.Map<IEnumerable<PlatformReadDto>>(repo.GetAllPlatforms()));
            })
            .WithName(GetPlatforms);

            //platforms/{id}
            group.MapGet("/{id}", ( IPlatformRepo repo,IMapper mapper, int id) =>
            {
                return repo.GetPlatformById(id) is null ? 
                    Results.NotFound() : 
                    Results.Ok(mapper.Map<PlatformReadDto>(repo.GetPlatformById(id)));
            })
            .WithName(GetPlatformById);

            //platforms/
            group.MapPost("/", (IPlatformRepo repo,IMapper mapper, PlatformCreateDto platformCreateDto) =>
            {
                var platformModel = mapper.Map<Platform>(platformCreateDto);
                repo.CreatePlatform(platformModel);
                repo.SaveChanges();
                var platformReadDto = mapper.Map<PlatformReadDto>(platformModel);
                return Results.CreatedAtRoute(GetPlatformById, new { Id = platformReadDto.Id }, platformReadDto);
            })
            .WithName(PostPlatform);
        }
    }
}