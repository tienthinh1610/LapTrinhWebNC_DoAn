BEGIN TRANSACTION;
GO

CREATE TABLE [Orders] (
    [OrderID] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Line1] nvarchar(max) NOT NULL,
    [Line2] nvarchar(max) NULL,
    [Line3] nvarchar(max) NULL,
    [City] nvarchar(max) NOT NULL,
    [State] nvarchar(max) NOT NULL,
    [Zip] nvarchar(max) NULL,
    [Country] nvarchar(max) NOT NULL,
    [GiftWrap] bit NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([OrderID])
);
GO

CREATE TABLE [CartLine] (
    [CartLineID] int NOT NULL IDENTITY,
    [ProductID] bigint NOT NULL,
    [Quantity] int NOT NULL,
    [OrderID] int NULL,
    CONSTRAINT [PK_CartLine] PRIMARY KEY ([CartLineID]),
    CONSTRAINT [FK_CartLine_Orders_OrderID] FOREIGN KEY ([OrderID]) REFERENCES [Orders] ([OrderID]),
    CONSTRAINT [FK_CartLine_Products_ProductID] FOREIGN KEY ([ProductID]) REFERENCES [Products] ([ProductID]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_CartLine_OrderID] ON [CartLine] ([OrderID]);
GO

CREATE INDEX [IX_CartLine_ProductID] ON [CartLine] ([ProductID]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251117114151_Orders', N'8.0.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Orders] ADD [Shipped] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251117120606_ShippedOrders', N'8.0.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [ProductImage] (
    [ProductImageID] int NOT NULL IDENTITY,
    [ImageUrl] nvarchar(max) NOT NULL,
    [IsMainImage] bit NOT NULL,
    [DisplayOrder] int NOT NULL,
    [ProductID] bigint NULL,
    CONSTRAINT [PK_ProductImage] PRIMARY KEY ([ProductImageID]),
    CONSTRAINT [FK_ProductImage_Products_ProductID] FOREIGN KEY ([ProductID]) REFERENCES [Products] ([ProductID])
);
GO

CREATE TABLE [ProductVariant] (
    [ProductVariantID] int NOT NULL IDENTITY,
    [Size] nvarchar(max) NOT NULL,
    [Color] nvarchar(max) NOT NULL,
    [Quantity] int NOT NULL,
    [ImageUrl] nvarchar(max) NOT NULL,
    [ProductID] bigint NULL,
    CONSTRAINT [PK_ProductVariant] PRIMARY KEY ([ProductVariantID]),
    CONSTRAINT [FK_ProductVariant_Products_ProductID] FOREIGN KEY ([ProductID]) REFERENCES [Products] ([ProductID])
);
GO

CREATE INDEX [IX_ProductImage_ProductID] ON [ProductImage] ([ProductID]);
GO

CREATE INDEX [IX_ProductVariant_ProductID] ON [ProductVariant] ([ProductID]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251117182526_AddMultipleImagesToProduct', N'8.0.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251117193640_UpdateSchemaForVariantsAndImages', N'8.0.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DROP TABLE [CartLine];
GO

ALTER TABLE [Orders] ADD [OrderPlaced] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
GO

CREATE TABLE [OrderLine] (
    [OrderLineID] int NOT NULL IDENTITY,
    [ProductID] bigint NOT NULL,
    [ProductVariantID] int NULL,
    [ProductName] nvarchar(max) NOT NULL,
    [ProductSize] nvarchar(max) NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [Quantity] int NOT NULL,
    [OrderID] int NOT NULL,
    CONSTRAINT [PK_OrderLine] PRIMARY KEY ([OrderLineID]),
    CONSTRAINT [FK_OrderLine_Orders_OrderID] FOREIGN KEY ([OrderID]) REFERENCES [Orders] ([OrderID]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_OrderLine_OrderID] ON [OrderLine] ([OrderID]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251117222648_AddOrderPlaced', N'8.0.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [OrderLine] DROP CONSTRAINT [FK_OrderLine_Orders_OrderID];
GO

ALTER TABLE [OrderLine] DROP CONSTRAINT [PK_OrderLine];
GO

EXEC sp_rename N'[OrderLine]', N'OrderLines';
GO

EXEC sp_rename N'[OrderLines].[IX_OrderLine_OrderID]', N'IX_OrderLines_OrderID', N'INDEX';
GO

ALTER TABLE [OrderLines] ADD CONSTRAINT [PK_OrderLines] PRIMARY KEY ([OrderLineID]);
GO

ALTER TABLE [OrderLines] ADD CONSTRAINT [FK_OrderLines_Orders_OrderID] FOREIGN KEY ([OrderID]) REFERENCES [Orders] ([OrderID]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251117223153_CreateOrderLinesTable', N'8.0.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Products]') AND [c].[name] = N'Price');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Products] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Products] ALTER COLUMN [Price] decimal(18,2) NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251117234615_IncreasePricePrecision', N'8.0.2');
GO

COMMIT;
GO

