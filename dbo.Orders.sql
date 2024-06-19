CREATE TABLE [dbo].[Orders] (
    [OrderId]       INT           IDENTITY (1, 1) NOT NULL,
    [BookId]            INT           NOT NULL,
    [Quantity]          INT           NOT NULL,
    [DeliveryDetailsId] INT           NOT NULL,
    [UserId]            INT           NOT NULL,
    [DeliveryStatus]    NVARCHAR (25) NOT NULL,
    CONSTRAINT [PK_dbo.Orders] PRIMARY KEY CLUSTERED ([OrderId] ASC)
);

