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