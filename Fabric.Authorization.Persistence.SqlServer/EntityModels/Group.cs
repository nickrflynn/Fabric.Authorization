﻿using System;
using System.Collections.Generic;
using Fabric.Authorization.Domain.Models;

namespace Fabric.Authorization.Persistence.SqlServer.EntityModels
{
    public class Group : ITrackable, ISoftDelete
    {
        public Group()
        {
            GroupRoles = new List<GroupRole>();
            GroupUsers = new List<GroupUser>();
        }

        public int Id { get; set; }
        public Guid GroupId { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }

        public DateTime CreatedDateTimeUtc { get; set; }
        public DateTime? ModifiedDateTimeUtc { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<GroupRole> GroupRoles{ get; set; }
        public ICollection<GroupUser> GroupUsers { get; set; }
    }
}