import {createUUID} from '../support/basicFunctions'
describe('Create service with minimum data then query to return it', () => {

    var organisation;
    var service;

    it('Create test organisation (POST api/organizations)', () => {

        cy.insertTestOrganisation().then((response)=>{
            organisation = response;
        })
    })

    it('Create test service (POST api/services)', () => {

        cy.createTestServiceJson('Test Get Services', organisation.id , true).then((serviceJson) =>{

            serviceJson.description = null;
            serviceJson.url = null;
            serviceJson.email = null;
            serviceJson.fees = null;
            serviceJson.distance = null;
            serviceJson.serviceDelivery = null;
            serviceJson.eligibilities = null;
            serviceJson.contacts = null;
            serviceJson.service_taxonomys = null;
            serviceJson.service_at_locations = null;
            serviceJson.cost_options = null;
            serviceJson.languages = null;
            serviceJson.service_areas = null;
            serviceJson.service_at_locations = null;
            serviceJson.service_taxonomys = null;

            cy.insertTestService(serviceJson).then((response)=>{
                service = response;   
            })
        })
    })

    it('Get service and validate (GET api/services?text=serviceName)', () => {

        cy.request(`api/services?text=${service.name}`)
        .then((response) =>{
        expect(response.status).to.eq(200);
        
        var testObject = response.body.items.find(x=>x.id === service.id);
        cy.compareServiceObject(service, testObject);
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

    it('Create test organisation (POST api/organizations)', () => {

        cy.insertTestOrganisation().then((response)=>{
            organisation = response;
        })
    })

    it('Create test service (POST api/services)', () => {

        cy.createTestServiceJson('Test Get Services', organisation.id , true).then((serviceJson) =>{
            
            cy.getTaxonomy('Before and after school clubs').then((taxonomy) =>{
                serviceJson.service_taxonomys = [
                    {
                        "id":createUUID(),
                        "taxonomy" :{"id":taxonomy.id}, 
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
        
        var testObject = response.body;
        cy.compareServiceObject(service, testObject);
        })
    })

    it('Get service and validate (GET api/services?text=serviceName)', () => {

        cy.request(`api/services?text=${service.name}`)
        .then((response) =>{
        expect(response.status).to.eq(200);
        
        var testObject = response.body.items.find(x=>x.id === service.id);
        cy.compareServiceObject(service, testObject);
        })
    })

    it('Get service and validate (GET api/organisationservices/{id})', () => {

        cy.request(`api/organisationservices/${organisation.id}`)
        .then((response) =>{
        expect(response.status).to.eq(200);
        
        var testObject = response.body.find(x=>x.id === service.id);
        cy.compareServiceObject(service, testObject);
        })
    })

    it('Update Service (PUT api/services)', () => {

        service.name += 'updated';
        service.description += 'updated';
        service.contacts[0].name += 'updated';
        service.contacts[0].phones[0].number= '01234 56789';
        service.service_at_locations[0].name += 'updated';

        cy.request('PUT',`api/services/${service.id}`, service)
            .then((response) =>{ 
            expect(response.status).to.eq(200); 
        }) 

    })

    it('Get service and validate after update (GET api/services/{id})', () => {

        cy.request(`api/services/${service.id}`)
        .then((response) =>{
        expect(response.status).to.eq(200);
        
        var testObject = response.body;
        cy.compareServiceObject(service, testObject);
        })
    })

    it('Delete Service (GET api/services/{id})', () => {

        cy.request('DELETE', `api/services/${service.id}`)
        .then((response) =>{
        expect(response.status).to.eq(200);

        })
    })

    it('Get service and validate status changed (GET api/services/{id})', () => {

        cy.request(`api/services/${service.id}`)
        .then((response) =>{
        expect(response.status).to.eq(200);
        
        var testObject = response.body;
        expect(testObject.status).to.eq('Deleted');
        })
    })

})