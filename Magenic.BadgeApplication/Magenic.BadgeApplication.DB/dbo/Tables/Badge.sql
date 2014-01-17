﻿CREATE TABLE [dbo].[Badge] (
    [BadgeId]                    INT             IDENTITY (1, 1) NOT NULL CONSTRAINT [pk_Badge] PRIMARY KEY CLUSTERED ([BadgeId] ASC),
    [BadgeName]                  VARCHAR (200)   NOT NULL,
    [BadgeTagLine]               VARCHAR (200)   NULL,
    [BadgeDescription]           VARCHAR (MAX)   NULL,
    [BadgeTypeId]                INT             NOT NULL CONSTRAINT [fk_Badge_BadgeType] FOREIGN KEY ([BadgeTypeId]) REFERENCES [dbo].[BadgeType] ([BadgeTypeId]),
    [BadgePath]                  VARCHAR(512)    NULL,
    [BadgeCreated]               DATETIME2 (7)   CONSTRAINT [df_Badge_BadgeCreated] DEFAULT getdate() NOT NULL,
    [BadgeEffectiveStart]        DATETIME2 (7)   NULL,
    [BadgeEffectiveEnd]          DATETIME2 (7)   NULL,
    [BadgePriority]              INT             NOT NULL,
    [MultipleAwardPossible]      BIT             NOT NULL,
    [DisplayOnce]                BIT             NOT NULL,
    [ManagementApprovalRequired] BIT             NOT NULL,
    [ActivityPointsAmount]       INT             NOT NULL,
    [BadgeAwardValueAmount]      INT             NOT NULL,
    [BadgeApprovedByADName]            VARCHAR(100)             NULL CONSTRAINT [fk_Badge_Employee] FOREIGN KEY ([BadgeApprovedByADName]) REFERENCES [dbo].[Employee] ([ADName]),
    [BadgeApprovedDate]          DATETIME2 (7)   CONSTRAINT [df_Badge_BadgeApprovedDate] DEFAULT getdate() NULL
);