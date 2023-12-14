using System.ComponentModel.DataAnnotations;

namespace efsane_oyun.Models
{
    public class Comments
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string UserId { get; set; }
        public string? Content { get; set; }
    }
}
