using System.ComponentModel.DataAnnotations;

namespace efsane_oyun.Models
{
    public class Games
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? TagLine { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public string? CoverImage { get; set; }
        public string? EmbedSource { get; set; }
    }
}
