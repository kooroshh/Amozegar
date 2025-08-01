namespace Amozegar.Areas.Admin.Models
{
    public class UsersViewModel
    {
        public string UserId { get; set; }
        public string Image { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string CreatedAt { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public string Status { get; set; }
        public bool IsBan { get; set; }
    }
}
