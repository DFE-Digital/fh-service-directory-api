DECLARE @CurrentDateTime DATETIME2 = SYSDATETIME();

INSERT [dbo].[Organisations] 
(
    [OrganisationType], 
    [Name], 
    [Description], 
    [AdminAreaCode], 
    [AssociatedOrganisationId], 
    [Logo], 
    [Uri], 
    [Url], 
    [Created], 
    [CreatedBy], 
    [LastModified], 
    [LastModifiedBy]
)
VALUES 
(
    N'LA', 
    N'Lewisham Council', 
    N'Lewisham Council', 
    N'E09000023', 
    NULL, 
    NULL, 
    N'https://lewisham.gov.uk/', 
    N'https://lewisham.gov.uk/', 
    @CurrentDateTime, 
    NULL, 
    @CurrentDateTime, 
    NULL
)

INSERT [dbo].[Organisations] 
(
    [OrganisationType], 
    [Name], 
    [Description], 
    [AdminAreaCode], 
    [AssociatedOrganisationId], 
    [Logo], 
    [Uri], 
    [Url], 
    [Created], 
    [CreatedBy], 
    [LastModified], 
    [LastModifiedBy]
)
VALUES 
(
    N'LA', 
    N'North East Lincolnshire Council', 
    N'North East Lincolnshire Council', 
    N'E06000012', 
    NULL, 
    NULL, 
    N'https://www.nelincs.gov.uk/', 
    N'https://www.nelincs.gov.uk/', 
    @CurrentDateTime, 
    NULL, 
    @CurrentDateTime, 
    NULL
)

INSERT [dbo].[Organisations] 
(
    [OrganisationType], 
    [Name], 
    [Description], 
    [AdminAreaCode], 
    [AssociatedOrganisationId], 
    [Logo], 
    [Uri], 
    [Url], 
    [Created], 
    [CreatedBy], 
    [LastModified], 
    [LastModifiedBy]
)
VALUES 
(
    N'LA', 
    N'City of Wolverhampton Council', 
    N'City of Wolverhampton Council', 
    N'E08000031', 
    NULL, 
    NULL, 
    N'https://www.wolverhampton.gov.uk/', 
    N'https://www.wolverhampton.gov.uk/', 
    @CurrentDateTime, 
    NULL, 
    @CurrentDateTime, 
    NULL
)

INSERT [dbo].[Organisations] 
(
    [OrganisationType], 
    [Name], 
    [Description], 
    [AdminAreaCode], 
    [AssociatedOrganisationId], 
    [Logo], 
    [Uri], 
    [Url], 
    [Created], 
    [CreatedBy], 
    [LastModified], 
    [LastModifiedBy]
)
VALUES 
(
    N'LA', 
    N'Sheffield City Council', 
    N'Sheffield City Council', 
    N'E08000019', 
    NULL, 
    NULL, 
    N'https://www.sheffield.gov.uk/', 
    N'https://www.sheffield.gov.uk/', 
    @CurrentDateTime, 
    NULL, 
    @CurrentDateTime, 
    NULL
)
