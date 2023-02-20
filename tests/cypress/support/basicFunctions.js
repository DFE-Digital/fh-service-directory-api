export function createUUID() {
    return ([1e7]+-1e3+-4e3+-8e3+-1e11).replace(/[018]/g, c =>
      (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    )
  }

export function compareObject(expected, actual){

  if(isNull(expected) != isNull(actual)){
      console.log('is null', expected, actual);
      return false;
  }

  for (var key in expected) {
      
      if(key == 'id' || key == 'openReferralOrganisationId'){
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