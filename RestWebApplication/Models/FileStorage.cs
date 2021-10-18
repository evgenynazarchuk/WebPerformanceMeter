namespace RestWebApplication.Models
{
    public class FileStorage
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public byte[] Content { get; set; }
    }
}
