﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Fabric.Authorization.Domain.Exceptions;
using Fabric.Authorization.Domain.Models;
using Fabric.Authorization.Domain.Stores;

namespace Fabric.Authorization.Persistence.InMemory.Stores
{
    public class InMemoryGroupStore : InMemoryFormattableIdentifierStore<Group>, IGroupStore
    {
        private readonly IRoleStore _roleStore;
        private readonly IUserStore _userStore;

        [Obsolete]
        public InMemoryGroupStore(IIdentifierFormatter identifierFormatter, 
            IRoleStore roleStore,
            IUserStore userStore) 
            : base(identifierFormatter)
        {
            _roleStore = roleStore;
            _userStore = userStore;
            var group1 = new Group
            {
                Id = Guid.NewGuid().ToString(),
                Name = @"FABRIC\Health Catalyst Viewer",
            };

            var group2 = new Group
            {
                Id = Guid.NewGuid().ToString(),
                Name = @"FABRIC\Health Catalyst Editor",
            };

            this.Add(group1).Wait();
            this.Add(group2).Wait();
        }

        public override async Task<Group> Add(Group group)
        {
            group.Track();

            var formattedId = FormatId(group.Identifier);

            if (Dictionary.ContainsKey(formattedId))
            {
                var existingGroup = Dictionary[formattedId];
                if (existingGroup == null)
                {
                    throw new CouldNotCompleteOperationException();
                }
                if (!existingGroup.IsDeleted)
                {
                    throw new AlreadyExistsException<Group>(group, formattedId);
                }

                existingGroup.IsDeleted = false;
                await Update(existingGroup).ConfigureAwait(false);
                return existingGroup;
            }

            Dictionary.TryAdd(formattedId, group);
            return group;
        }

        public override async Task Delete(Group group)
        {
            group.IsDeleted = true;

            var formattedId = FormatId(group.Id);

            // use base class Exists so IsDeleted is ignored since it's already been updated at this point
            if (await base.Exists(formattedId).ConfigureAwait(false))
            {
                if (!Dictionary.TryUpdate(formattedId, group, Dictionary[formattedId]))
                {
                    throw new CouldNotCompleteOperationException();
                }
            }
            else
            {
                throw new NotFoundException<Group>(group, group.Identifier);
            }
        }

        public override async Task<bool> Exists(string id)
        {
            var formattedId = FormatId(id);
            return await base.Exists(formattedId) && !Dictionary[formattedId].IsDeleted;
        }

        public async Task<Group> AddRoleToGroup(Group group, Role role)
        {
            group.Roles.Add(role);
            role.Groups.Add(group.Name);

            await _roleStore.Update(role);
            await Update(group);

            return group;
        }

        public async Task<Group> DeleteRoleFromGroup(Group group, Role role)
        {
            if (role.Groups.Any(g => string.Equals(g, group.Name, StringComparison.OrdinalIgnoreCase)))
            {
                role.Groups.Remove(group.Name);
            }

            await _roleStore.Update(role);
            await Update(group);
            return group;
        }

        public async Task<Group> AddUserToGroup(Group group, User user)
        {
            if (user.Groups.All(g => !string.Equals(g, group.Name, StringComparison.OrdinalIgnoreCase)))
            {
                user.Groups.Add(group.Name);
            }

            await _userStore.Update(user);
            await Update(group);
            return group;
        }

        public async Task<Group> DeleteUserFromGroup(Group group, User user)
        {
            if (user.Groups.Any(g => string.Equals(g, group.Name, StringComparison.OrdinalIgnoreCase)))
            {
                user.Groups.Remove(group.Name);
            }

            await _userStore.Update(user);
            await Update(group);
            return group;
        }
    }
}