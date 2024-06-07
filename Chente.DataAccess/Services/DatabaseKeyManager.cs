namespace Chente.DataAccess.Services;

public static class DatabaseKeyManager
{
    public static int GetPrimaryKeyFrom(string stringValue)
    {
        string[] parts = stringValue.Split("-");
        return Int32.Parse(parts[1]);
    }
}
