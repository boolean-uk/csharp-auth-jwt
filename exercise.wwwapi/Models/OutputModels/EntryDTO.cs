﻿using exercise.wwwapi.Models.PureModels;

namespace exercise.wwwapi.Models.OutputModels
{
    /// <summary>
    /// Data transfer object for transfering information about a entry out.
    /// </summary>
    /// <param name="entry"> The entry to be provided</param>
    /// <param name="user"> The user that wrote the entry, should be included within Entry.Author </param>
    public class EntryDTO(Entry entry, ApplicationUser user) 
    {
        public int Id { get; set; } = entry.Id;

        public string Title { get; set; } = entry.Title;

        public string Description { get; set; } = entry.Content;

        public string LastUpdated { get; set; } = entry.UpdatedAt.ToString("yyyy-MM-dd hh:mm:ss");

        public UserDTO Author { get; set; } = new UserDTO(user);
    }
}
