CREATE TABLE [dbo].[DeliveryInfo] (
    [DeliveryInfoId] INT            IDENTITY (1, 1) NOT NULL,
    [FullName]          NVARCHAR (MAX) NOT NULL,
    [Postcode]          NVARCHAR (MAX) NOT NULL,
    [Country]           NVARCHAR (MAX) NOT NULL,
    [City]              NVARCHAR (MAX) NOT NULL,
    [Street]            NVARCHAR (MAX) NOT NULL,
    [Building]          NVARCHAR (MAX) NOT NULL,
    [Appartment]      NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_dbo.DeliveryInfo] PRIMARY KEY CLUSTERED ([DeliveryInfoId] ASC)
);

