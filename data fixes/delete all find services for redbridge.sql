DECLARE @ServiceIDs TABLE (ServiceID BIGINT);

---- to delete a set of provided service ids
--INSERT INTO @ServiceIDs (ServiceID)
---- list service ids to delete here
--VALUES (150876), (140876), (130876);

-- to delete all of a organisation's services (optionally filtered by ServiceType etc.)
INSERT INTO @ServiceIDs (ServiceID)
SELECT s.Id
FROM Services s
WHERE 
s.ServiceType = 'FamilyExperience'
--  ServiceType = 'InformationSharing'
AND s.OrganisationId IN (
    SELECT o.Id
    FROM Organisations o
    WHERE o.AdminAreaCode = 'E09000026'
);

-- Declare a cursor to loop through the service IDs
DECLARE @CurrentServiceID BIGINT;
DECLARE ServiceCursor CURSOR FOR
SELECT ServiceID FROM @ServiceIDs;

-- Open the cursor
OPEN ServiceCursor;

-- Fetch the first service ID
FETCH NEXT FROM ServiceCursor INTO @CurrentServiceID;

-- Loop through the service IDs and perform deletions
WHILE @@FETCH_STATUS = 0
BEGIN
    -- Delete records related to the current service ID
	delete Contacts where ServiceId = @CurrentServiceID;
	delete CostOptions where ServiceId = @CurrentServiceID;
	delete Eligibilities where ServiceId = @CurrentServiceID;
	delete Languages where ServiceId = @CurrentServiceID;
	delete Schedules where ServiceId = @CurrentServiceID;
	delete Schedules where ServiceAtLocationId in (
	  select Id from ServiceAtLocations where ServiceId = @CurrentServiceID
	);
	delete ServiceAtLocations where ServiceId = @CurrentServiceID;
	delete ServiceDeliveries where ServiceId = @CurrentServiceID;
	delete ServiceTaxonomies where ServiceId = @CurrentServiceID;
	delete Services where Id = @CurrentServiceID;

    -- Get the next service ID
    FETCH NEXT FROM ServiceCursor INTO @CurrentServiceID;
END;

-- Close and deallocate the cursor
CLOSE ServiceCursor;
DEALLOCATE ServiceCursor;