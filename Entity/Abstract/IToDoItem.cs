namespace Entity.Abstract;

    // ITodoItem arayüzü, görev nesneleri için temel özellikleri tanımlar.
    public interface ITodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        DateTime CreatedAt { get; set; }
        int UserId { get; set; }
    }