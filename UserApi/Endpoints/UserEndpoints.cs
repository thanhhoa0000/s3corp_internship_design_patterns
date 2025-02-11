using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace UserManagementApp.UserApi.Endpoints
{
    public class UserEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/users");

            group.MapGet("/", GetAllUsers);
            group.MapGet("/{userId:guid}", GetUser);
            group.MapPost("/", CreateUser);
            group.MapPut("/", UpdateUser);
            group.MapDelete("/{userId:guid}", DeleteUser);
        }

        public async Task<Results<Ok<IEnumerable<AppUserDto>>, BadRequest<string>>> 
            GetAllUsers(
                IUserRepository repository,
                HttpContext httpContext,
                IMapper mapper, 
                ILogger logger,
                int pageSize = 0,
                int pageNumber = 1)
        {
            try
            {
                logger.LogInformation("Getting the users...");

                IEnumerable<AppUser> usersList 
                    = await repository.GetUsersAsync(tracked: false, pageSize: pageSize, pageNumber: pageNumber);

                Pagination pagination = new Pagination()
                {
                    PageSize = pageSize,
                    PageNumber = pageNumber
                };

                httpContext.Response.Headers["X-Pagination"] = JsonSerializer.Serialize(pagination);

                return TypedResults.Ok(mapper.Map<IEnumerable<AppUserDto>>(usersList));
            }
            catch (Exception ex)
            {
                logger.LogError("Error(s) occured: \n{error}", ex);
                
                return TypedResults.BadRequest("Error(s) occured when getting the users!");
            }
        }

        public async Task<Results<Ok<AppUserDto>, BadRequest<string>>> 
            GetUser(
                IUserRepository repository,
                Guid userId,
                IMapper mapper,
                ILogger logger)
        {
            try
            {
                logger.LogInformation("Getting the user...");

                var user = await repository.GetUserAsync(u => u.Id == userId, tracked: false);

                if (user is null)
                {
                    logger.LogError("User not found!");
                    return TypedResults.BadRequest($"Cannot find the user with ID \"{userId}\"");
                }

                return TypedResults.Ok(mapper.Map<AppUserDto>(user));
            }
            catch (Exception ex)
            {
                logger.LogError("Error(s) occured: \n{error}", ex);

                return TypedResults.BadRequest("Error(s) occured when getting the user!");
            }
        }

        public async Task<Results<Created, BadRequest<string>>> 
            CreateUser(
                [FromBody] AppUserDto userDto,
                IUserRepository repository,
                IMapper mapper,
                ILogger logger)
        {
            try
            {
                if (userDto is null)
                {
                    logger.LogError("Input User is null");
                    return TypedResults.BadRequest("No Input User was found!");
                }

                logger.LogInformation("Creating user {userId}", userDto.Id);

                if (await repository.GetUserAsync(u => u.Id == userDto.Id, tracked: false) is not null)
                {
                    logger.LogError("User already existed");
                    return TypedResults.BadRequest("User already existed!");
                }

                AppUser user = mapper.Map<AppUser>(userDto);
                await repository.CreateUserAsync(user);

                return TypedResults.Created($"/api/users/{userDto.Id}");
            }
            catch (Exception ex)
            {
                logger.LogError("Error(s) occured: \n{error}", ex);

                return TypedResults.BadRequest("Error(s) occured when creating the user!");
            }
        }

        public async Task<Results<NoContent, NotFound<string>, BadRequest<string>>>
            UpdateUser(
                [FromBody] AppUserDto userDto,
                IUserRepository repository,
                IMapper mapper,
                ILogger logger)
        {
            try
            {
                if (userDto is null)
                {
                    logger.LogError("Input User is null");
                    return TypedResults.BadRequest("No Input User was found!");
                }

                logger.LogInformation("Updating user {userId}", userDto.Id);

                if (await repository.GetUserAsync(u => u.Id == userDto.Id, tracked: false) is null)
                {
                    logger.LogError("User does not exist");
                    return TypedResults.NotFound("User does not exist!");
                }

                AppUser user = mapper.Map<AppUser>(userDto);
                await repository.UpdateUserAsync(user);

                return TypedResults.NoContent();


            }
            catch (Exception ex)
            {
                logger.LogError("Error(s) occured: \n{error}", ex);

                return TypedResults.BadRequest("Error(s) occured when updating the user!");
            }
        }

        public async Task<Results<NoContent, NotFound<string>, BadRequest<string>>>
            DeleteUser(
                Guid userId,
                IUserRepository repository,
                ILogger logger)
        {
            try
            {
                if (userId.ToString().IsNullOrEmpty())
                {
                    logger.LogError("Input User ID is null");
                    return TypedResults.BadRequest("No Input User ID was found!");
                }

                logger.LogInformation("Updating user {userId}", userId);

                AppUser user = await repository.GetUserAsync(u => u.Id == userId, tracked: false);

                if (user is null)
                {
                    logger.LogError("User does not exist");
                    return TypedResults.NotFound("User does not exist!");
                }

                await repository.RemoveUserAsync(user);

                return TypedResults.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError("Error(s) occured: \n{error}", ex);

                return TypedResults.BadRequest("Error(s) occured when deleting the user!");
            }
        }
    }
}
