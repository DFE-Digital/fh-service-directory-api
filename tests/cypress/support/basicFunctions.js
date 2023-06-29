export function compareObject(expected, actual){

  if(isNull(expected) != isNull(actual)){
      console.log('is null', expected, actual);
      return false;
  }

  for (var key in expected) {
      
      if(key.toUpperCase().includes('ID') || 
         key.toUpperCase().includes('DATE') || 
         key.toUpperCase().includes('DISTANCE') || 
         key.toUpperCase().includes('accessibilityForDisabilities'.toUpperCase()) ||
         key.toUpperCase().includes('At'.toUpperCase()) ||
         key == 'openReferralOrganisationId'){
          continue;
      }

      if(typeof expected[key] == 'object'){
          var subObjectValid = compareObject(expected[key], actual[key]); // recursion
          if(!subObjectValid){
              return false;
          }
      }
      else{
          if(expected[key] != actual[key]){
              console.log(`Expected property ${key} does not match in actual object`,expected[key], actual[key], expected);
              return false;
          }
      }
  }

  return true;
}


function isNull(obj){
  if(obj == null || obj == undefined){
      return true;
  }

  if(Array.isArray(obj) && obj.length === 0){
    return true;
  }
  
  return false;
}

export function getBearerToken(){
    var jwt = require('jsonwebtoken');
    var timeStamp = (Math.floor((new Date()).getTime() / 1000) + 90000);

    var token = jwt.sign({
        "role": "DfeAdmin",
        "OrganisationId": "-1",
        "AccountStatus": "Active",
        "FullName": "dfeAdmin",
        "PhoneNumber": "0121 121 1234",
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": "DfeAdmin",
        "LoginTime": "638235387725514695",
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress": "cypress.user@test.com",
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier": "dfeAdmin.user@stub.com",
        "exp": timeStamp
      }, 'StubPrivateKey123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ');

      return token;
}

export function createUUID() {
    return ([1e7]+-1e3+-4e3+-8e3+-1e11).replace(/[018]/g, c =>
      (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    )
  }