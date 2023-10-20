--this script should be executetd to create table, stored procedure and user defined table type needed for the application

USE [IceCreamCompanySync]
GO
/****** Object:  UserDefinedTableType [dbo].[WorkflowArray]    Script Date: 2023-10-19 14:32:06 ******/
CREATE TYPE [dbo].[WorkflowArray] AS TABLE(
	[WorkflowId] [int] NULL,
	[WorkflowName] [varchar](150) NULL,
	[IsActive] [bit] NULL,
	[IsRunning] [bit] NULL,
	[MultiExecBehavior] [varchar](150) NULL
)
GO
/****** Object:  Table [dbo].[Workflows]    Script Date: 2023-10-19 14:32:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Workflows](
	[WorkflowId] [int] NOT NULL,
	[WorkflowName] [varchar](150) NOT NULL,
	[IsActive] [bit] NULL,
	[IsRunning] [bit] NULL,
	[MultiExecBehavior] [varchar](150) NULL,
 CONSTRAINT [PK_Workflows] PRIMARY KEY CLUSTERED 
(
	[WorkflowId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[UpdateWorkflows]    Script Date: 2023-10-19 14:32:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateWorkflows]
	@Workflows WorkflowArray readonly
AS
BEGIN
	BEGIN TRANSACTION
	BEGIN TRY
		MERGE [dbo].[Workflows] dbw 
		USING @Workflows w  on [w].[WorkflowId] = [dbw].[WorkflowId]
		WHEN MATCHED
		THEN 
			UPDATE SET [dbw].[WorkflowName] = [w].[WorkflowName], [dbw].[IsActive] = [w].[IsActive], [dbw].[IsRunning] = [w].[IsRunning], [dbw].[MultiExecBehavior] = [w].[MultiExecBehavior]
		WHEN NOT MATCHED THEN 
			INSERT ([WorkflowId],[WorkflowName],[IsActive],[IsRunning], [MultiExecBehavior]) Values([w].[WorkflowId],[w].[WorkflowName],[w].[IsActive],[w].[IsRunning], [w].[MultiExecBehavior])

		WHEN NOT MATCHED BY SOURCE 
		THEN 
			DELETE;

	COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		THROW;
	END CATCH
END
GO
