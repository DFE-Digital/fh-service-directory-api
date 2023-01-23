describe('info endpoint', () => {
  it('returns api info', () => {
    cy.request('api/info')
    .then((response) => {

      expect(response.status).to.eq(200)
      expect(response.body).to.match(/^Version/)
      expect(response.body).to.match(/Last Updated/)
      expect(response.body).to.match(/Db Type/)
    });
  })
})