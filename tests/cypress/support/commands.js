import {createUUID} from './basicFunctions';

Cypress.Commands.add('insertTestOrganisation', () => {

    cy.fixture('organisation').then(organisation=>{

        organisation.id = createUUID();

        cy.request('POST','api/organizations', organisation)
            .then((response) =>{ 
              expect(response.status).to.eq(200); 
              return organisation;
            }) 
    })
})

Cypress.Commands.add('createTestServiceJson', (name, organisationId, removeOptionalFields) => {

  cy.fixture('service').then(service=>{

      service.id = createUUID();
      service.openReferralOrganisationId = organisationId;
      service.name = name + service.id;
      service.serviceDelivery[0].id = createUUID();
      service.eligibilities[0].id = createUUID();
      service.contacts[0].id = createUUID();
      service.contacts[0].phones[0].id = createUUID();
      service.cost_options[0].id = createUUID();
      service.languages[0].id = createUUID();
      service.service_areas[0].id = createUUID();
      service.service_at_locations[0].id = createUUID();
      service.service_at_locations[0].location.id = createUUID();
      service.service_at_locations[0].location.physical_addresses[0].id = createUUID();
      service.service_at_locations[0].regular_schedule[0].id = createUUID();
      service.service_taxonomys = null; // Needs to be set based on value in db
      return service;

  })
})

Cypress.Commands.add('insertTestService', (service) => {
  cy.request('POST','api/services', service)
      .then((response) =>{ 
        expect(response.status).to.eq(200); 
        return service;
      }) 
})

Cypress.Commands.add('getTestTaxonomy', (searchFor) => {
  // This will retrieve a test taxonomy if it exists or create and return it if it doesn't
  cy.request('GET','api/taxonomies?text=childTaxonomyForServicesTest')
    .then((response) =>{ 
      expect(response.status).to.eq(200); 

      if(response.body.items.length == 0){

        var parentTaxonomy = {
          "id" : createUUID(),
          "name" : "parenttaxonomyForServicesTest",
          "vocabulary":"parenttaxonomyForServicesTest"
        };
        var childTaxonomy = {
          "id" : createUUID(),
          "name" : "childTaxonomyForServicesTest",
          "vocabulary":"childTaxonomyForServicesTest",
          "parent":parentTaxonomy.id
        };

        //  Create Parent
        cy.request('POST','api/taxonomies', parentTaxonomy)
        .then((responseParentTaxonomy) =>{ 
          expect(responseParentTaxonomy.status).to.eq(200); 
    
          //  Create Child
          cy.request('POST','api/taxonomies', childTaxonomy)
          .then((responseChildTaxonomy) =>{ 
            expect(responseChildTaxonomy.status).to.eq(200); 
            return childTaxonomy;
          }) 
        }) 

      }
      return response.body.items[0];
    }) 
})

Cypress.Commands.add('compareServiceObject', (expected, actual) => {
  expect(actual.id).to.eq(expected.id); 
  expect(actual.serviceType).to.deep.equal(expected.serviceType);
  expect(actual.name).to.eq(expected.name);
  expect(actual.openReferralOrganisationId).to.eq(expected.openReferralOrganisationId);
  expect(actual.description).to.eq(expected.description);
  expect(actual.accreditations).to.eq(expected.accreditations);
  expect(actual.assured_date).to.eq(expected.assured_date);
  expect(actual.attending_access).to.eq(expected.attending_access);
  expect(actual.attending_type).to.eq(expected.attending_type);
  expect(actual.deliverable_type).to.eq(expected.deliverable_type);
  expect(actual.status).to.eq(expected.status);
  
  expect(actual.url).to.eq(expected.url);
  expect(actual.email).to.eq(expected.email);
  expect(actual.fees).to.eq(expected.fees);
  expect(actual.distance).to.eq(expected.distance);
  expect(actual.canFamilyChooseDeliveryLocation).to.eq(expected.canFamilyChooseDeliveryLocation);
  if(expected.serviceDelivery!=null || expected.serviceDelivery!=undefined){
    expect(actual.serviceDelivery).to.deep.equal(expected.serviceDelivery);
  }
  if(expected.eligibilities!=null || expected.eligibilities!=undefined){
    expect(actual.eligibilities).to.deep.equal(expected.eligibilities);
  }
  if(expected.contacts!=null || expected.contacts!=undefined){
    expect(actual.contacts).to.deep.equal(expected.contacts);
  }
  if(expected.cost_options!=null || expected.cost_options!=undefined){
    expect(actual.cost_options).to.deep.equal(expected.cost_options);
  }
  if(expected.languages!=null || expected.languages!=undefined){
    expect(actual.languages).to.deep.equal(expected.languages);
  }
  cy.compareServiceAtLocationArray(expected.service_at_locations, actual.service_at_locations);

  if(expected.service_taxonomys!=null || expected.service_taxonomys!=undefined){
    expect(actual.service_taxonomys[0].id).to.deep.equal(expected.service_taxonomys[0].id);
  }

  expect(actual.status).to.eq(expected.status);
})

Cypress.Commands.add('compareServiceAtLocationArray', (expected, actual) => {
  if(expected ==null || expected ==undefined){
    return;
  }

  for(var i=0; i < expected; i++){

    expect(actual[i].location.name).to.eq(expected[i].location.name);
    expect(actual[i].location.description).to.eq(expected[i].location.description);
    expect(actual[i].location.latitude).to.eq(expected[i].location.latitude);
    expect(actual[i].location.longitude).to.eq(expected[i].location.longitude);
    expect(actual[i].location.distance).to.eq(expected[i].location.distance);

    for(var j=0; j < expected; j++){
      var actualAddress = actual[i].location.physical_addresses[j];
      var expectedAddress = expected[i].location.physical_addresses[j];
      expect(actualAddress.address_1).to.eq(expectedAddress.address_1);
      expect(actualAddress.city).to.eq(expectedAddress.city);
      expect(actualAddress.postal_code).to.eq(expectedAddress.postal_code);
      expect(actualAddress.country).to.eq(expectedAddress.country);
      expect(actualAddress.state_province).to.eq(expectedAddress.state_province);
    }

    expect(actual[i].regular_schedule).to.deep.equal(expected[i].regular_schedule);
    expect(actual[i].holidayScheduleCollection).to.deep.equal(expected[i].holidayScheduleCollection);
  }
})

Cypress.Commands.add('createTaxonomyJson', (parent) => {

  cy.fixture('taxonomy').then(taxonomy=>{

    taxonomy.id = createUUID();
    taxonomy.name += taxonomy.id;
    taxonomy.vocabulary += taxonomy.id;
    if(parent != null || parent != undefined){
      taxonomy.parent = parent;
    }
    return taxonomy;

  })
})