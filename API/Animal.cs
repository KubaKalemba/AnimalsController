
public class Animal
{
    public Animal()
    {
    }

    public Animal(int id, string name, string description, string category, string area)
    {
        Id = id;
        Name = name;
        Description = description;
        Category = category;
        Area = area;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string Area { get; set; }
}