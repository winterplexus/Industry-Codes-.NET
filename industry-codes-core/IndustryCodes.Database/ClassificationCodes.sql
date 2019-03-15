CREATE TABLE [dbo].[ClassificationCodes](
  [ID]                              [int] IDENTITY(1,1) NOT NULL,
  [INDUSTRY_SECTOR]                 [nvarchar](255)         NULL,
  [INDUSTRY_SUBSECTOR]              [nvarchar](255)         NULL,
  [NORTH_AMERICAN_CODE]             [int]                   NULL,
  [NORTH_AMERICAN_DESCRIPTION]      [nvarchar](255)         NULL,
  [STANDARD_CODE]                   [int]                   NULL,
  [STANDARD_DESCRIPTION]            [nvarchar](255)         NULL,
  [KIND_CODE]                       [int]                   NULL,
  [KIND_CODE_DESCRIPTION]           [nvarchar](255)         NULL,
  [NORTH_AMERICAN_CODE_2002]        [int]                   NULL,
  [NORTH_AMERICAN_DESCRIPTION_2002] [nvarchar](255)         NULL,

  CONSTRAINT [PK_PID] PRIMARY KEY CLUSTERED 
  (
   [ID] ASC
  ) 
  WITH ( PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON )
)