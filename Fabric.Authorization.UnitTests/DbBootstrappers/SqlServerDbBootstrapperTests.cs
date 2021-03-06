﻿using System.Linq;
using Fabric.Authorization.Persistence.SqlServer.Configuration;
using Fabric.Authorization.Persistence.SqlServer.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fabric.Authorization.UnitTests.DbBootstrappers
{
    public class SqlServerDbBootstrapperTests
    {
        [Fact]
        public void Setup_CreatesDefaults_Success()
        {
            var dbContext = CreateDbContext();
            Bootstrap(dbContext, new Domain.Defaults.Authorization());
            AssertDefaults(dbContext);
        }

        [Fact]
        public void Setup_UpdatesDefaults_Success()
        {
            var dbContext = CreateDbContext();
            var authorizationDefaults = new Domain.Defaults.Authorization();
            Bootstrap(dbContext, authorizationDefaults);
            AssertDefaults(dbContext);

            var dosGrain = authorizationDefaults.Grains.First(g => g.Name == "dos");
            dosGrain.RequiredWriteScopes.Add("fabric/authorization.write");

            Bootstrap(dbContext, authorizationDefaults);

            AssertDefaults(dbContext);

            var updatedDosGrain = dbContext.Grains.First(g => g.Name == "dos");
            Assert.Equal("fabric/authorization.dos.write;fabric/authorization.write", updatedDosGrain.RequiredWriteScopes);
        }

        private void AssertDefaults(IAuthorizationDbContext dbContext)
        {
            var grains = dbContext.Grains.ToList();
            Assert.Equal(2, grains.Count);
            Assert.Equal(1, dbContext.SecurableItems.Count());
            Assert.Equal(1, dbContext.Roles.Count());
            Assert.Equal(1, dbContext.Permissions.Count());

            var securableItem = dbContext.SecurableItems.FirstOrDefault(si => si.Name == "datamarts");
            Assert.NotNull(securableItem);
            Assert.Equal("dos-metadata-service", securableItem.ClientOwner);
            Assert.Equal("datamarts", securableItem.Name);

            var roles = dbContext.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .Where(r => r.Name == "dosadmin")
                .ToList();

            var rolePermissions = roles.SelectMany(r => r.RolePermissions).ToList();
            Assert.Single(rolePermissions);

            var permissions = rolePermissions.Select(rp => rp.Permission).ToList();
            Assert.Single(permissions);
            Assert.Equal("manageauthorization", permissions.First().Name);
        }

        private void Bootstrap(IAuthorizationDbContext dbContext, Domain.Defaults.Authorization authorizationDefaults)
        {
            var sqlServerBootStrapper = new SqlServerDbBootstrapper(dbContext, authorizationDefaults);
            sqlServerBootStrapper.Setup();
        }

        private InMemoryAuthorizationDbContext CreateDbContext()
        {
            return new InMemoryAuthorizationDbContext(new NoOpEventContextResolverService(), new ConnectionStrings { AuthorizationDatabase = "somestring" });
        }
    }
}
