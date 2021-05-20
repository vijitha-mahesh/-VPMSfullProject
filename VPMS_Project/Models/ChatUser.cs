using VPMS_Project.Models;


namespace VPMS_Project.Models
{

    public class ChatUser
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set;}
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
        public UserRole Role { get; set; }
    }
}