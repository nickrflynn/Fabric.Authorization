CREATE TABLE [dbo].[RoleUsers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[CreatedDateTimeUtc] [datetime2](7) NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
	[IdentityProvider] [nvarchar](200) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDateTimeUtc] [datetime2](7) NULL,
	[SubjectId] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_RoleUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [HCFabricAuthorizationData1],
 CONSTRAINT [AK_RoleUsers_SubjectId_IdentityProvider_GroupId] UNIQUE NONCLUSTERED 
(
	[SubjectId] ASC,
	[IdentityProvider] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [HCFabricAuthorizationData1]
) ON [HCFabricAuthorizationData1] TEXTIMAGE_ON [HCFabricAuthorizationData1]
GO

CREATE NONCLUSTERED INDEX [IX_RoleUsers_RoleId] ON [dbo].[RoleUsers]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [HCFabricAuthorizationIndex1]
GO

ALTER TABLE [dbo].[RoleUsers]  WITH CHECK ADD  CONSTRAINT [FK_RoleUsers_Roles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[RoleUsers] CHECK CONSTRAINT [FK_RoleUsers_Roles_RoleId]
GO

ALTER TABLE [dbo].[RoleUsers]  WITH CHECK ADD  CONSTRAINT [FK_RoleUsers_Users_SubjectId_IdentityProvider] FOREIGN KEY([SubjectId], [IdentityProvider])
REFERENCES [dbo].[Users] ([SubjectId], [IdentityProvider])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[RoleUsers] CHECK CONSTRAINT [FK_RoleUsers_Users_SubjectId_IdentityProvider]
GO
