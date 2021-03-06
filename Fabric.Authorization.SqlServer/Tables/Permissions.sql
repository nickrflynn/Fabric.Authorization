CREATE TABLE [dbo].[Permissions](
	[PermissionId] [uniqueidentifier] NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[CreatedDateTimeUtc] [datetime] NOT NULL,
	[Grain] [nvarchar](200) NOT NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDateTimeUtc] [datetime] NULL,
	[Name] [nvarchar](200) NOT NULL,
	[SecurableItemId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Permissions] PRIMARY KEY NONCLUSTERED 
(
	[PermissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [HCFabricAuthorizationData1]
) ON [HCFabricAuthorizationData1] TEXTIMAGE_ON [HCFabricAuthorizationData1]
GO

CREATE UNIQUE CLUSTERED INDEX [IX_Permissions_Id] ON [dbo].[Permissions]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [HCFabricAuthorizationData1]
GO

CREATE NONCLUSTERED INDEX [IX_Permissions_SecurableItemId] ON [dbo].[Permissions]
(
	[SecurableItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [HCFabricAuthorizationIndex1]
GO

ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[Permissions]  WITH CHECK ADD  CONSTRAINT [FK_Permissions_SecurableItems_SecurableItemId] FOREIGN KEY([SecurableItemId])
REFERENCES [dbo].[SecurableItems] ([SecurableItemId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Permissions] CHECK CONSTRAINT [FK_Permissions_SecurableItems_SecurableItemId]
GO
