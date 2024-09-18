﻿using exercise.wwwapi.Models;
using exercise.wwwapi.Models.InputModels;
using exercise.wwwapi.Models.OutputModels;
using exercise.wwwapi.Models.PureModels;
using exercise.wwwapi.Repository;
using exercise.wwwapi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class EntryEndpoint
    {
        public static void ConfigureBlogEndpoint(this WebApplication app) 
        {
            var BlogGroup = app.MapGroup("/blog/");

            BlogGroup.MapGet("", GetPosts);
            BlogGroup.MapGet("{id}", GetPost);
            BlogGroup.MapPost("", CreatePost);
            BlogGroup.MapPut("{id}", UpdatePost);
            BlogGroup.MapDelete("{id}", DeletePost);
            BlogGroup.MapGet("MyEntries/", GetAllEntriesForCurrentUser);
            BlogGroup.MapGet("UserEntries/{email}", GetAllEntriesForProvided);
        }

        [Authorize(Roles = "User, Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> GetPosts(IRepository<Entry> repo) 
        {
            IEnumerable<Entry> entries = await repo.GetAll();
            if (entries.Count() == 0) 
            {
                return TypedResults.NotFound($"No entries could be found in the database.");
            }

            IEnumerable<EntryDTO> entriesOut = entries.Select(s => new EntryDTO(s, s.Author));

            Payload<IEnumerable<EntryDTO>> payload = new Payload<IEnumerable<EntryDTO>>(entriesOut);
            return TypedResults.Ok(payload);
        }

        [Authorize(Roles = "User, Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> GetPost(IRepository<Entry> repo, int id) 
        {
            Entry? entry = await repo.Get(id);
            if (entry == null) 
            {
                return TypedResults.NotFound($"No entry with provided ID ({id}) found.");
            }

            EntryDTO entryOut = new EntryDTO(entry, entry.Author);
            Payload<EntryDTO> payload = new Payload<EntryDTO>(entryOut);
            return TypedResults.Ok(payload);
        }

        [Authorize(Roles = "User, Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> CreatePost(IRepository<Entry> repo, UserRepository userRepo, ClaimsPrincipal apiUser, EntryPost entryPost) 
        {
            string? posterId = HttpContextHelper.UserId(apiUser);

            if (posterId == null) 
            {
                return TypedResults.BadRequest("Error retriving ID of submitting user.");
            }

            Entry inputEntry = new Entry()
            {
                Title = entryPost.Title,
                Content = entryPost.Text,
                CreatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                AuthorId = posterId,
            };

            Entry entry = await repo.Insert(inputEntry);
            ApplicationUser user = await userRepo.Get(entry.AuthorId);

            EntryDTO entryOut = new EntryDTO(entry, user);
            Payload<EntryDTO> payload = new Payload<EntryDTO>(entryOut);
            return TypedResults.Created($"/{entry.Id}", payload);
        }

        [Authorize(Roles = "User, Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> UpdatePost(IRepository<Entry> repo, UserRepository userRepo, ClaimsPrincipal apiUser, int id, EntryPut entryPost) 
        {
            string? userClaimedId = HttpContextHelper.UserId(apiUser);
            if (userClaimedId == null) 
            {
                return TypedResults.BadRequest("Could not identify the request sender.");
            }

            Entry? dbEntry = await repo.Get(id);
            if (dbEntry == null)
            {
                return TypedResults.NotFound($"Could not find the entry with provided ID ({id}), no changes performed to database data.");
            }

            bool UserIsAdmin = await userRepo.UserIsAdmin(userClaimedId);
            if ((dbEntry.AuthorId != userClaimedId) && !(UserIsAdmin))
            {
                return TypedResults.Unauthorized();
            }

            Entry inputEntry = new Entry()
            {
                Id = dbEntry.Id,
                Title = entryPost.Title ?? dbEntry.Title,
                Content = entryPost.Text ?? dbEntry.Content,
                CreatedAt = dbEntry.CreatedAt,
                UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                AuthorId = dbEntry.AuthorId,
            };

            Entry entry = await repo.Update(id, inputEntry);

            EntryDTO entryOut = new EntryDTO(entry, dbEntry.Author);
            Payload<EntryDTO> payload = new Payload<EntryDTO>(entryOut);
            return TypedResults.Created($"/{entry.Id}", payload);
        }

        [Authorize(Roles = "User, Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> DeletePost(IRepository<Entry> repo, UserRepository userRepo, ClaimsPrincipal apiUser, int id) 
        {
            string? posterId = HttpContextHelper.UserId(apiUser);

            if (posterId == null)
            {
                return TypedResults.BadRequest("Could not identify the request sender.");
            }

            Entry? entry = await repo.Get(id);
            if (entry == null) 
            {
                return TypedResults.NotFound($"Could not find any entry with the provided ID ({id}).");
            }

            if ((entry.AuthorId != posterId) && !(await userRepo.UserIsAdmin(posterId))) 
            {
                return TypedResults.Unauthorized();
            }

            entry = await repo.Delete(entry);

            EntryDTO entryOut = new EntryDTO(entry, entry.Author);
            Payload<EntryDTO> payload = new Payload<EntryDTO>(entryOut);
            return TypedResults.Ok(payload);
        }

        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> GetAllEntriesForCurrentUser(IRepository<Entry> repo, ClaimsPrincipal apiUser) 
        {
            string? posterId = HttpContextHelper.UserId(apiUser);
            
            if (posterId == null)
            {
                return TypedResults.BadRequest("Error retriving ID of submitting apiUser.");
            }

            IEnumerable<Entry> userEntries = await repo.GetAllWithFieldValue("AuthorId", posterId);
            IEnumerable<EntryDTO> entriesOut = userEntries.Select(s => new EntryDTO(s, s.Author));

            Payload<IEnumerable<EntryDTO>> payload = new Payload<IEnumerable<EntryDTO>>(entriesOut);
            return TypedResults.Ok(payload);
        }

        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> GetAllEntriesForProvided(
            IRepository<Entry> repo, UserRepository userRepo, ClaimsPrincipal apiUser, string email)
        {
            string? posterId = HttpContextHelper.UserId(apiUser);

            if (posterId == null)
            {
                return TypedResults.BadRequest("Error retriving ID of submitting apiUser.");
            }

            if (!(await userRepo.UserIsAdmin(posterId))) 
            {
                return TypedResults.Unauthorized();
            }

            IEnumerable<ApplicationUser> users = await userRepo.GetAllWithFieldValue("Email", email);
            ApplicationUser user = users.FirstOrDefault();

            IEnumerable<Entry> userEntries = await repo.GetAllWithFieldValue("AuthorId", user.Id);
            IEnumerable<EntryDTO> entriesOut = userEntries.Select(s => new EntryDTO(s, s.Author));

            Payload<IEnumerable<EntryDTO>> payload = new Payload<IEnumerable<EntryDTO>>(entriesOut);
            return TypedResults.Ok(payload);
        }
    }
}
