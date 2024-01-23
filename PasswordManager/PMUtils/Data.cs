namespace PMUtils
{
    public class Data
    {
        public string Website { get; set; }
        public string Name { get; set; }
        public byte[] Password { get; set; }

        public Data(string website, string name, byte[] password)
        {
            Website = website;
            Name = name;
            Password = password;
        }
    }
}
