SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DeliveryPlan](
	[DeliveryPlanId] [uniqueidentifier] NOT NULL,
	[CargoStuffingId] [int] NOT NULL,
	[ContainerType] [nvarchar](20) NOT NULL,
	[ContainerReference] [nvarchar](50) NOT NULL,
	[VesselName] [nvarchar](50) NOT NULL,
	[VesselType] [nvarchar](20) NOT NULL,
	[Status] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_DeliveryPlan] PRIMARY KEY CLUSTERED 
(
	[DeliveryPlanId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DeliveryPlanLeg](
	[DeliveryPlanLegId] [uniqueidentifier] NOT NULL,
	[DeliveryPlanId] [uniqueidentifier] NOT NULL,
	[ProviderCode] [nvarchar](10) NOT NULL,
	[PickUpDate] [datetime2](7) NOT NULL,
	[DropOffDate] [datetime2](7) NOT NULL,
	[StartLocationCountryCode] [nvarchar](10) NOT NULL,
	[StartLocationSiteCode] [nvarchar](10) NOT NULL,
	[StartLocationFullCountryName] [nvarchar](25) NOT NULL,
	[StartLocationFullSiteName] [nvarchar](50) NOT NULL,
	[EndLocationCountryCode] [nvarchar](10) NOT NULL,
	[EndLocationSiteCode] [nvarchar](10) NOT NULL,
	[EndLocationFullCountryName] [nvarchar](25) NOT NULL,
	[EndLocationFullSiteName] [nvarchar](50) NOT NULL,
	[Status] [nvarchar](10) NOT NULL,
	CONSTRAINT [FK_DeliveryPlanLeg_DeliveryPlanId] FOREIGN KEY ([DeliveryPlanId]) REFERENCES [dbo].[DeliveryPlan] ([DeliveryPlanId]),
 CONSTRAINT [PK_DeliveryPlanLeg] PRIMARY KEY CLUSTERED 
(
	[DeliveryPlanLegId] ASC
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO



