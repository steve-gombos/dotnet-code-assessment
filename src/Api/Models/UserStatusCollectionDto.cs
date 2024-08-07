using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Template.Api.Models
{
    public class UserStatusCollectionDto
    {
        public IList<UserStatusDto> Users { get; set; }
    }

    public class UserStatusDto
    {
        [JsonPropertyName("id")]
        public int UserId { get; set; }

        public bool IsComplete { get; set; }
        public bool IsActive { get; set; }
    }
}
