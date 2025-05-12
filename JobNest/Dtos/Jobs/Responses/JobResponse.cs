namespace JobNest.Dtos.Jobs.Responses
{
    public class JobResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime PostedDate { get; set; }
        public string EmployerName { get; set; } = string.Empty;
    }
}

