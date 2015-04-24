BEGIN TRANSACTION
GO
Alter table dbo.Customer Add 
[ContactEmail] [varchar](50) COLLATE Chinese_PRC_CI_AS NULL,
[SubIndustryID] [int] NULL,
[PageDesignContact] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NULL,
[PageDesignPhone] [varchar](50) COLLATE Chinese_PRC_CI_AS NULL,
[PageDesignEmail] [varchar](50) COLLATE Chinese_PRC_CI_AS NULL,
[PageDesignQQ] [varchar](50) COLLATE Chinese_PRC_CI_AS NULL,
[PageDesignMobile] [varchar](50) COLLATE Chinese_PRC_CI_AS NULL,
[PageExecuteContact] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NULL,
[PageExecutePhone] [varchar](50) COLLATE Chinese_PRC_CI_AS NULL,
[PageExecuteEmail] [varchar](50) COLLATE Chinese_PRC_CI_AS NULL,
[PageExecuteQQ] [varchar](50) COLLATE Chinese_PRC_CI_AS NULL,
[PageExecuteMobile] [varchar](50) COLLATE Chinese_PRC_CI_AS NULL,
[ComplaintHandlingContact] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NULL,
[ComplaintHandlingPhone] [varchar](50) COLLATE Chinese_PRC_CI_AS NULL,
[ComplaintHandlingEmail] [varchar](50) COLLATE Chinese_PRC_CI_AS NULL,
[ComplaintHandlingQQ] [varchar](50) COLLATE Chinese_PRC_CI_AS NULL,
[ComplaintHandlingMobile] [varchar](50) COLLATE Chinese_PRC_CI_AS NULL,
[FinancialContact] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NULL,
[FinancialPhone] [varchar](50) COLLATE Chinese_PRC_CI_AS NULL,
[FinancialEmail] [varchar](50) COLLATE Chinese_PRC_CI_AS NULL,
[FinancialQQ] [varchar](50) COLLATE Chinese_PRC_CI_AS NULL,
[FinancialMobile] [varchar](50) COLLATE Chinese_PRC_CI_AS NULL,
[BusinessLisenceImg] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL,
[TaxRegisterCertificateImg] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL,
[HealthPermitImg] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL,
[LisenceImg] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL,
[OtherProfQualificationImg] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL,
[PowerofAttorneyImg] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL,
[ExternalEnvImg] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL,
[InternalEnvImg] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL;

ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_SubIndustry] FOREIGN KEY([SubIndustryID])
REFERENCES [dbo].[SubIndustry] ([SubIndustryID]);

Alter TABLE [dbo].[Payment] ADD
   [ApprovalStatus] [int] NULL,
	[PaymentStatus] [int] NULL,
	[CertificateImg] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL,
	[ApplyDate] [datetime] NULL,
	[Applicant] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Payer] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL;

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FinApprovalRecord](
	[RecordID] [int] IDENTITY(1,1) NOT NULL,
	[PaymentID] [int] NOT NULL,
	[Approver] [varchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[ApprovalResult] [int] NULL,
	[ApprovalDate] [datetime] NULL,
	[ApprovalComment] [nvarchar](300) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_FinApprovalRecord] PRIMARY KEY CLUSTERED 
(
	[RecordID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
USE [DSHOrderManagement]
GO
ALTER TABLE [dbo].[FinApprovalRecord]  WITH CHECK ADD  CONSTRAINT [FK_FinApprovalRecord_Payment] FOREIGN KEY([PaymentID])
REFERENCES [dbo].[Payment] ([PaymentID]);
COMMIT

