import {createUUID} from './basicFunctions';

Cypress.Commands.add('insertTestOrganisation', () => {

    cy.fixture('organisation').then(organisation=>{

        cy.request('POST','api/organisations', organisation)
            .then((response) =>{ 
              expect(response.status).to.eq(200);
              organisation.id = response.body;
              return organisation;
            }) 
    })
})

Cypress.Commands.add('createTestServiceJson', (name, organisationId, removeOptionalFields) => {

  cy.fixture('service').then(service=>{

      service.organisationId = organisationId;
      service.name = name + service.id;
      return service;

  })
})

Cypress.Commands.add('insertTestService', (service) => {
  cy.request('POST','api/services', service)
      .then((response) =>{ 
        expect(response.status).to.eq(200); 
        service.id = response.body;
        return service;
      }) 
})

Cypress.Commands.add('createTaxonomyJson', (parent) => {

  cy.fixture('taxonomy').then(taxonomy=>{

    taxonomy.name += taxonomy.id;
    taxonomy.taxonomyType = 1;
    if(parent != null || parent != undefined){
      taxonomy.parent = parent;
    }
    
    return taxonomy;
  })
})

Cypress.Commands.add('getTestTaxonomy', (searchFor) => {
  // This will retrieve a test taxonomy if it exists or create and return it if it doesn't
  cy.request('GET','api/taxonomies?text=childTaxonomyForServicesTest&taxonomyType=NotSet')
    .then((response) =>{ 
      expect(response.status).to.eq(200); 

      if(response.body.items.length == 0){

        var parentTaxonomy = {
          "name" : "parenttaxonomyForServicesTest",
          "taxonomyType":1
        };
        var childTaxonomy = {
          "name" : "childTaxonomyForServicesTest",
          "taxonomyType":1,
          "parent":parentTaxonomy.id
        };

        //  Create Parent
        cy.request('POST','api/taxonomies', parentTaxonomy)
        .then((responseParentTaxonomy) =>{ 
          expect(responseParentTaxonomy.status).to.eq(200); 
          parentTaxonomy.id = responseParentTaxonomy.body;
          //  Create Child
          cy.request('POST','api/taxonomies', childTaxonomy)
          .then((responseChildTaxonomy) =>{ 
            expect(responseChildTaxonomy.status).to.eq(200); 
            childTaxonomy.id = responseChildTaxonomy.body;
            return childTaxonomy;
          }) 
        }) 

      }
      return response.body.items[0];
    }) 
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
