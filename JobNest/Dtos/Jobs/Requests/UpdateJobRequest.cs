﻿namespace JobNest.Dtos.Jobs.Requests
{
    public class UpdateJobRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }
}

