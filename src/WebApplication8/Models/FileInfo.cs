namespace ProofJob.Models
{
    public class FileInfo
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsFolder { get; set; }
        public int[] FilesSizes { get; set; }
        public FileInfo[] InnerDir { get; set; } 
    }
}
