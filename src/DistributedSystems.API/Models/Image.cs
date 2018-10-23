﻿using System;

namespace DistributedSystems.API.Models
{
    public class Image
    {
        public Image()
        {
            Id = Guid.NewGuid();
            UploadedDate = DateTime.UtcNow;;
        }

        public Guid Id { get; }
        public string Location { get; set; }
        public DateTime UploadedDate { get; set; }
        public ImageStatus Status { get; set; } = ImageStatus.UploadComplete;
    }
}
