namespace Redis.Entities;

public class ToDo
{
    public ToDo(Guid id, string title, string description)
    {
        Id = id;
        Title = title;
        Description = description;
    }

    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}