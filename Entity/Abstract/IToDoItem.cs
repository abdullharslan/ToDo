namespace Entity.Abstract
{
    public interface ITodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        DateTime CreatedDate { get; set; }
        int UserId { get; set; }
    }
}