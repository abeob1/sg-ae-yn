/****** Object:  Table [dbo].[CancelOVPM]    Script Date: 11/05/2013 22:20:51 ******/
IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CancelOVPM]') AND type in (N'U'))
Begin
	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON
	
	CREATE TABLE [dbo].[CancelOVPM](
		[DocEntry] [int] NULL,
		[Cancelled] [int] NULL,
		[CardCode] [nvarchar](15),
		[ProjectCode] [nvarchar](20)
	) ON [PRIMARY]
End


/****** Object:  Table [dbo].[InterestOVPM]    Script Date: 11/05/2013 22:22:15 ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InterestOVPM]') AND type in (N'U'))
Begin
	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON
	CREATE TABLE [dbo].[InterestOVPM](
		[DocEntry] [int] NOT NULL,
		[Term] [int] NULL,
		[Day] [int] NULL,
		[Date] [date] NULL,
		[Value] [numeric](19, 6) NULL,
		[Synced] [int] NULL,
		[ErrorMess] [nvarchar](max) NULL,
		[Cancelled] [int] NULL,
		[CardCode] [nvarchar](15),
		[ProjectCode] [nvarchar](20)
	) ON [PRIMARY]
End
