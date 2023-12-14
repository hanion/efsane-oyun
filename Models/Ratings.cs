namespace efsane_oyun.Models
{
    public class Ratings
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string UserId { get; set; }
        public int Score { get; set; }
    }
}
