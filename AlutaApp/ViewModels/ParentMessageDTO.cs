namespace AlutaApp.ViewModels
{
    public class ParentMessageDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string MediaLink { get; set; }
        public int? PostId { get; set; }
        public string UserName { get; set; }
        public bool Deleted { get; set; }
    }
}