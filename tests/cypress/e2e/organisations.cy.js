import {compareObject} from '../support/basicFunctions';

describe('organisations endpoints e2e', () => {
  var organisation;

  afterEach(function() {
    if (this.currentTest.state === 'failed') {
      Cypress.runner.stop()
    }
  });

  it('Create Organisation (POST api/organizations)', () => {
    cy.insertTestOrganisation().then(response=>{
      organisation = response;
      console.log(organisation);
    })
  })

  it('Get Organisation by ID (GET api/organizations/{id})', () => {
    cy.request(`api/organizations/${organisation.id}`)
    .then((response) =>{
      expect(response.status).to.eq(200);
      var testObject = response.body;
      expect(testObject.name).to.eq(organisation.name);
      organisation = testObject;
    })
  })

  it('Update Organisation (PUT api/organizations/{id})', () => {

    organisation.name += 'Updated';

    cy.request('PUT', `api/organizations/${organisation.id}`, organisation)
    .then((response) =>{
      expect(response.status).to.eq(200);
    })
  })

  it('Validate Organization Updated (GET api/organizations/{id})', () => {
    cy.request(`api/organizations/${organisation.id}`)
    .then((response) =>{
      expect(response.status).to.eq(200);
      var testObject = response.body;
      expect(testObject.name).to.eq(organisation.name);
    })
  })

  it('Get Organisation (GET api/organizationAdminCode/{id})', () => {
    cy.request(`api/organizationAdminCode/${organisation.id}`)
    .then((response) =>{
      expect(response.status).to.eq(200);
      var testObject = response.body;
      expect(testObject).to.deep.equal(organisation.adminAreaCode);
    })
  })

  it('Get Organisation (GET api/organizations)', () => {
    cy.request('api/organizations')
    .then((response) =>{
      expect(response.status).to.eq(200);
      var testObject = response.body.find(x => x.id == organisation.id);
      expect(compareObject(organisation, testObject)).to.eq(true);
    })
  })
})