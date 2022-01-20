function initAutocomplete() {

    autocomplete = new google.maps.places.Autocomplete(document.getElementById("venueInput"));
    autocomplete.addListener('place_changed', onPlaceChanged);
}


function onPlaceChanged() {
    var place = autocomplete.getPlace();
    
    if (!place.geometry) {
        document.getElementById("venueInput").placeholder = "Enter a name for a place";
    }
    else {

        document.getElementById("locName").value = place.name;
        document.getElementById("locAddress").value = place.formatted_address;
        document.getElementById("locLat").value = place.geometry.location.lat();
        document.getElementById("locLong").value = place.geometry.location.lng();

        StaticMap(place.geometry.location.lat(), place.geometry.location.lng());

    }
}

$(document).ready(function StaticMap() {
    var center = 37.9755 + "+" + 23.7326814;
    document.getElementById("staticMap").src = "https://maps.googleapis.com/maps/api/staticmap?center=" + center + "&zoom=11&scale=1&size=600x200&maptype=roadmap&" +
        "key=" + key +
        "&format=png&visual_refresh=true&markers=size:mid%7Ccolor:0x077de9%7Clabel:%7C" + center;

})

function StaticMap(lat, lng) {
    var center = lat + "+" + lng;
    document.getElementById("staticMap").src = "https://maps.googleapis.com/maps/api/staticmap?center=" + center + "&zoom=11&scale=1&size=600x200&maptype=roadmap&" +
        "key=" + key +
        "&format=png&visual_refresh=true&markers=size:mid%7Ccolor:0x077de9%7Clabel:%7C" + center;

}


//var componentForm = {
//    street_number: 'short_name',
//    route: 'long_name',
//    locality: 'long_name',
//    administrative_area_level_1: 'short_name',
//    country: 'long_name',
//    postal_code: 'short_name'
//};


//function fillInAddress() {
//    // Get the place details from the autocomplete object.
//    var place = autocomplete.getPlace();

//    for (var component in componentForm) {
//        document.getElementById(component).value = '';
//        document.getElementById(component).disabled = false;
//    }

//    // Get each component of the address from the place details
//    // and fill the corresponding field on the form.
//    for (var i = 0; i < place.address_components.length; i++) {
//        var addressType = place.address_components[i].types[0];
//        if (componentForm[addressType]) {
//            var val = place.address_components[i][componentForm[addressType]];
//            document.getElementById(addressType).value = val;
//        }
//    }


 
