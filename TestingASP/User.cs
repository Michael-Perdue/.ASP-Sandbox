public class User
{
    public int Id {get; set;}
    public string UserName { get; set; }
    public string Pass { get; set; }

    public User(int id, string user, string pass)
    {
        Id = id;
        UserName = user;
        Pass = pass;
    }

    public override string ToString()
    {
        return "ID: " + Id.ToString() + ", UserName: " + UserName + ", Password: " + Pass;
    }

    public bool match(string user, string pass)
    {
        return user.Equals(UserName) && pass.Equals(Pass);
    }
}