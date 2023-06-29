import {compareObject, getBearerToken} from '../support/basicFunctions';

describe('taxonomies endpoints e2e', () => { 

    var parentTaxonomy;
    var childTaxonomy;

    afterEach(function() {
      if (this.currentTest.state === 'failed') {
        Cypress.runner.stop()
      }
    });
  
    it('Create ParentTaxonomy (POST api/taxonomies)', () => {
      cy.createTaxonomyJson(null).then(taxonomy=>{

        parentTaxonomy = taxonomy;

        var token = getBearerToken();

        cy.request({
          method: 'POST',
          url: `api/taxonomies`, 
          body: parentTaxonomy,
          auth: {
            'bearer': token
          }
        }).then((response) =>{ 
          parentTaxonomy.id = response.body;
          expect(response.status).to.eq(200);
        }) 

      })
    })

  
    it('Get new ParentTaxonomy (GET api/taxonomies?text=name)', () => {
      cy.request(`api/taxonomies?text=${parentTaxonomy.name}&taxonomyType=NotSet`)
      .then((response) =>{
        expect(response.status).to.eq(200);
        var testObject = response.body.items.find(x=>x.id === parentTaxonomy.id);
        expect(compareObject(parentTaxonomy, testObject)).to.eq(true);
      })
    })
  
    it('Create ChildTaxonomy (POST api/taxonomies)', () => {
        cy.createTaxonomyJson(parentTaxonomy.id).then(taxonomy=>{
            childTaxonomy = taxonomy;

            var token = getBearerToken();

            cy.request({
              method: 'POST',
              url: `api/taxonomies`, 
              body: childTaxonomy,
              auth: {
                'bearer': token
              }
            }).then((response) =>{ 
              childTaxonomy.id = response.body;
              expect(response.status).to.eq(200);
            }) 

        })
    })

    it('Get new ChildTaxonomy (GET api/taxonomies?text=name&taxonomyType=NotSet)', () => {
        cy.request(`api/taxonomies?text=${childTaxonomy.name}&taxonomyType=NotSet`)
        .then((response) =>{
            expect(response.status).to.eq(200);
            var testObject = response.body.items.find(x=>x.id === childTaxonomy.id);
            expect(compareObject(testObject, childTaxonomy)).to.eq(true);
        })
    })

    it('Update ChildTaxonomy (PUT api/taxonomies/{id})', () => {
        childTaxonomy.name += 'updated';

        var token = getBearerToken();
        
        cy.request({
          method: 'PUT',
          url: `api/taxonomies/${childTaxonomy.id}`, 
          body: childTaxonomy,
          auth: {
            'bearer': token
          }
        }).then((response) =>{ 
          expect(response.status).to.eq(200);
        }) 

    })

    it('Get updated ChildTaxonomy (GET api/taxonomies?text=name&taxonomyType=NotSet)', () => {
        cy.request(`api/taxonomies?text=${childTaxonomy.name}&taxonomyType=NotSet`)
        .then((response) =>{
            expect(response.status).to.eq(200);
            var testObject = response.body.items.find(x=>x.id === childTaxonomy.id);
            expect(compareObject(testObject, childTaxonomy)).to.eq(true);
        })
    })
})