SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DeliveryPlanView](
	[DeliveryPlanId] [uniqueidentifier] NOT NULL,
	[ContainerReference] [nvarchar](25) NOT NULL,
	[ContainerName] [nvarchar](50) NOT NULL,
	[StatusId] [int] NOT NULL,
	[StatusDescription] [nvarchar](20) NOT NULL,
	[VesselName] [nvarchar](25) NOT NULL,
	[EstimatedTimeOfArrival] [DATETIME] NOT NULL
 CONSTRAINT [PK_DeliveryPlanningView] PRIMARY KEY CLUSTERED 
(
	[DeliveryPlanId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO