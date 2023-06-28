import {compareObject, getBearerToken} from '../support/basicFunctions';

describe('Create service with minimum data then query to return it', () => {

    var organisation;
    var service;

    it('Create test organisation (POST api/organisations)', () => {

        cy.insertTestOrganisation().then((response)=>{
            organisation = response;
        })
    })

    it('Create test service (POST api/services)', () => {

        cy.createTestServiceJson('Test Get Services', organisation.id , true).then((serviceJson) =>{

            cy.insertTestService(serviceJson).then((response)=>{
                service = response;   
            })
        })
    })

    it('Get service and validate (GET api/services?text=serviceName)', () => {

        cy.request(`api/services?text=${service.name}&pageSize=99999999`)
        .then((response) =>{
        expect(response.status).to.eq(200);
        
        var testObject = response.body.items.find(x=>x.id === service.id);
        expect(compareObject(testObject, service)).to.eq(true);
        })
    })

})

describe('Service endpoints end to end test', () => {

    var organisation;
    var service;

    afterEach(function() {
        if (this.currentTest.state === 'failed') {
          Cypress.runner.stop()
        }
      });

    it('Create test organisation (POST api/organisations)', () => {

        cy.insertTestOrganisation().then((response)=>{
            organisation = response;
        })
    })

    it('Create test service (POST api/services)', () => {

        cy.createTestServiceJson('Test Get Services', organisation.id , true).then((serviceJson) =>{
            
            cy.getTestTaxonomy().then((taxonomy) =>{
                serviceJson.serviceTaxonomies = [
                    {
                        "taxonomy" : taxonomy, 
                }];

                cy.insertTestService(serviceJson).then((response)=>{
                    service = response
                })
            })
        })
    })

    it('Get service and validate (GET api/services/{id})', () => {

        cy.request(`api/services/${service.id}`)
        .then((response) =>{
        expect(response.status).to.eq(200);
        
        var testObject = response.body.id;
        expect(testObject).to.eq(service.id);
        })
    })

    it('Get service and validate (GET api/services?text=serviceName)', () => {

        cy.request(`api/services?text=${service.name}&pageSize=99999999`)
        .then((response) =>{
        expect(response.status).to.eq(200);
        
        var testObject = response.body.items.find(x=>x.id === service.id);
        expect(compareObject(testObject, service)).to.eq(true);
        })
    })

    it('Get service and validate (GET api/organisationservices/{id})', () => {

        cy.request(`api/organisationservices/${organisation.id}`)
        .then((response) =>{
        expect(response.status).to.eq(200);
        
        var testObject = response.body.find(x=>x.id === service.id);
        expect(compareObject(testObject, service)).to.eq(true);
        })
    })

    it('Update Service (PUT api/services)', () => {

        service.name += 'updated';
        service.description += 'updated';

        var token = getBearerToken();

        cy.request({
          method: 'PUT',
          url: `api/services/${service.id}`, 
          body: service,
          auth: {
            'bearer': token
          }
        }).then((response) =>{ 
            expect(response.status).to.eq(200); 
        }) 


    })

    it('Get service and validate after update (GET api/services/{id})', () => {

        cy.request(`api/services/${service.id}`)
        .then((response) =>{
        expect(response.status).to.eq(200);
        
        var testObject = response.body;
        expect(compareObject(testObject, service)).to.eq(true);
        })
    })

    it('Delete Service (GET api/services/{id})', () => {

        var token = getBearerToken();

        cy.request({
          method: 'DELETE',
          url: `api/services/${service.id}`, 
          auth: {
            'bearer': token
          }
        }).then((response) =>{ 
            expect(response.status).to.eq(200); 
        }) 

    })

    it('Get service and validate status changed (GET api/services/{id})', () => {

        cy.request(`api/services/${service.id}`)
        .then((response) =>{
        expect(response.status).to.eq(200);
        
        var testObject = response.body;
        expect(testObject.status).to.eq(5);
        })
    })

})