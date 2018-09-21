CREATE TABLE [dbo].[ClassificationCodes] (
  [ID]                              [INT] IDENTITY(1,1) NOT NULL,
  [INDUSTRY_SECTOR]                 [NVARCHAR](255)     NULL,
  [INDUSTRY_SUBSECTOR]              [NVARCHAR](255)     NULL,
  [NORTH_AMERICAN_CODE_2007]        [INT]               NULL,
  [NORTH_AMERICAN_DESCRIPTION_2007] [NVARCHAR](255)     NULL,
  [STANDARD_CODE]                   [INT]               NULL,
  [STANDARD_DESCRIPTION]            [NVARCHAR](255)     NULL,
  [KIND_CODE]                       [INT]               NULL,
  [KIND_CODE_DESCRIPTION]           [NVARCHAR](255)     NULL,
  [NORTH_AMERICAN_CODE_2002]        [INT]               NULL,
  [NORTH_AMERICAN_DESCRIPTION_2002] [NVARCHAR](255)     NULL,

  CONSTRAINT [PK_PID] PRIMARY KEY CLUSTERED
  (
    [ID] ASC
  )
  WITH
  (
    PAD_INDEX = OFF,
    STATISTICS_NORECOMPUTE = OFF,
    IGNORE_DUP_KEY = OFF,
    ALLOW_ROW_LOCKS = ON,
    ALLOW_PAGE_LOCKS = ON
  ) ON [PRIMARY]

) ON [PRIMARY]