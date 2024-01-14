namespace TestingASP;

public class RandomData
{
    public int Id { get; }
    public string Message { get; set; }
    public string? Value2 { get; }

    public RandomData(string message, int id, string? value2 = null)
    {
        Message = message;
        Id = id;
        Value2 = value2;
    }

    public override string ToString()
    {
        return "ID: " + Id.ToString() + ", Message: " + Message + ", Value2: " + Value2;
    }
}