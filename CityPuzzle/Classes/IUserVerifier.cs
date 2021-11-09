namespace CityPuzzle.Classes
{
    public interface IUserVerifier
    {
        bool CPass(string name, string pass);
        bool CUser(string name);
        string PToH(string pass);
        bool PassVer(string pass, string passwordHash);
    }
}