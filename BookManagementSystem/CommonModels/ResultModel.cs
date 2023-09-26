namespace BookManagementSystem.Common
{
    public class ResultModel<T> where T : class
    {
        public string status { get; set; } = "99";
        public string message { get; set; } = "Operation Failed";
        public bool success { get; set; } = false;
        public DateTime timeStamp { get; set; } = DateTime.Now;
        public List<T> data { get; set; } = new List<T>();
    }
}
