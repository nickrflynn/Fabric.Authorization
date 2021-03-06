﻿using System;

namespace Fabric.Authorization.Domain.Models
{
    public interface ITrackable
    {
        DateTime CreatedDateTimeUtc { get; set; }
        DateTime? ModifiedDateTimeUtc { get; set; }
        string CreatedBy { get; set; }
        string ModifiedBy { get; set; }
    }
}
