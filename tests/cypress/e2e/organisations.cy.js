import {compareObject} from '../support/basicFunctions';

describe('organisations endpoints e2e', () => {
  var organisation;

  afterEach(function() {
    if (this.currentTest.state === 'failed') {
      Cypress.runner.stop()
    }
  });

  it('Create Organisation (POST api/organisations)', () => {
    cy.insertTestOrganisation().then(response=>{
      organisation = response;
      console.log(organisation);
    })
  })

  it('Get Organisation by ID (GET api/organisations/{id})', () => {
    cy.request(`api/organisations/${organisation.id}`)
    .then((response) =>{
      expect(response.status).to.eq(200);
      var testObject = response.body;
      expect(compareObject(testObject, organisation)).to.eq(true);
      organisation = testObject;
    })
  })

  it('Update Organisation (PUT api/organisations/{id})', () => {

    organisation.name += 'Updated';

    cy.request('PUT', `api/organisations/${organisation.id}`, organisation)
    .then((response) =>{
      expect(response.status).to.eq(200);
    })
  })

  it('Validate Organization Updated (GET api/organisations/{id})', () => {
    cy.request(`api/organisations/${organisation.id}`)
    .then((response) =>{
      expect(response.status).to.eq(200);
      var testObject = response.body;
      expect(compareObject(testObject, organisation)).to.eq(true);
    })
  })

  it('Get Organisation (GET api/organisationAdminCode/{id})', () => {
    cy.request(`api/organisationAdminCode/${organisation.id}`)
    .then((response) =>{
      expect(response.status).to.eq(200);
      var testObject = response.body;
      expect(testObject).to.deep.equal(organisation.adminAreaCode);
    })
  })

  it('Get Organisation (GET api/organisations)', () => {
    cy.request('api/organisations')
    .then((response) =>{
      expect(response.status).to.eq(200);
      var testObject = response.body.find(x => x.id == organisation.id);
      expect(compareObject(organisation, testObject)).to.eq(true);
    })
  })
})