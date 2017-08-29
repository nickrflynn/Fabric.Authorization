﻿using System;
using System.Collections.Generic;

namespace Fabric.Authorization.Domain.Models
{
    public class Group : ITrackable, IIdentifiable, ISoftDelete
    {
        public Group()
        {
            Roles = new List<Role>();
            Users = new List<User>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public ICollection<Role> Roles { get; set; }

        public ICollection<User> Users { get; set; }

        public string Source { get; set; }

        public string Identifier => Id;

        public bool IsDeleted { get; set; }

        public DateTime CreatedDateTimeUtc { get; set; }

        public DateTime? ModifiedDateTimeUtc { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }
    }
}