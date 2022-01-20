
var map;
var options;
var athens = { lat: 37.9838, lng: 23.7275 }
var mapMarkers = [];


//SEARCH THIS AREA BUTTON SECTION --------------------------
function searchArea(controlDiv, map) {

    const searchAreaButton = document.createElement("button");

    searchAreaButton.classList.add("custom-map-control-button");
    searchAreaButton.setAttribute("id", "searchButton");
    searchAreaButton.setAttribute("value", "Search this Area");
    searchAreaButton.innerHTML = '<img src="/images/search.svg" height="15px" class="d-inline"/>  Search this Area';
    searchAreaButton.title = "Search this area";
    searchAreaButton.style.position = "relative";
    searchAreaButton.style.width = "120px";
    searchAreaButton.style.height = "40px";
    searchAreaButton.style.backgroundColor = "#fff";
    searchAreaButton.style.border = "6px solid #fff";
    searchAreaButton.style.borderRadius = "15px";
    searchAreaButton.style.boxShadow = "0 2px 6px rgba(0,0,0,.3)";
    searchAreaButton.style.padiding = "10px";
    searchAreaButton.style.marginRight = "10px";
    searchAreaButton.style.marginTop = "10px";
    controlDiv.appendChild(searchAreaButton);

    searchAreaButton.addEventListener("click", GetLocations);

}

//GO-TO MY LOCATION BUTTON SECTION --------------------------
function myLocation(controlDiv, map) {

    const locationButton = document.createElement("button");

    locationButton.classList.add("custom-map-control-button");
    locationButton.title = "My Location";
    locationButton.style.position = "relative";
    locationButton.innerHTML = '<img src="/images/geo.svg" height="25px" style="margin-left:-10px; margin-top:-10px"/>';
    locationButton.style.width = "40px";
    locationButton.style.height = "40px";
    locationButton.style.backgroundColor = "#fff";
    locationButton.style.border = "2px solid #fff";
    locationButton.style.borderRadius = "2px";
    locationButton.style.boxShadow = "0 2px 6px rgba(0,0,0,.3)";
    locationButton.style.padding = "1rem";
    locationButton.style.marginRight = "10px";
    controlDiv.appendChild(locationButton);

    locationButton.addEventListener("click", () => {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                (position) => {
                    const currentPosition = {
                        lat: position.coords.latitude,
                        lng: position.coords.longitude
                    };
                    var infoWindow = new google.maps.InfoWindow();

                    infoWindow.setPosition(currentPosition);
                    infoWindow.setContent("You are here");
                    infoWindow.open(map);
                    map.setCenter(currentPosition);

                    map.setCenter(currentPosition);
                    map.setZoom(15);
                }
            )
        }
    })
}

//THEME BUTTON SECTION --------------------------
function themeButtonDiv(controlDiv, map) {

    const themeButton = document.createElement("button");

    themeButton.classList.add("custom-map-control-button");
    themeButton.setAttribute("id", "searchButton");
    themeButton.setAttribute("class", "btn btn-secondary");
    themeButton.innerHTML = '<img src="/images/brightness-high-fill.svg" height="20px" class="d-inline" style="margin-left:-3px"/>';
    themeButton.title = "Change map theme";
    themeButton.style.position = "relative";
    themeButton.style.width = "40px";
    themeButton.style.height = "40px";
    themeButton.style.boxShadow = "0 2px 6px rgba(0,0,0,.3)";
    themeButton.style.padiding = "10px";
    themeButton.style.marginRight = "10px";
    themeButton.style.marginBottom = "10px";
    controlDiv.appendChild(themeButton);

    themeButton.addEventListener("click", () => {
        if (themeButton.classList.contains("btn-secondary")) {

            themeButton.classList.remove("btn-secondary");
            themeButton.classList.add("btn-light");
            themeButton.innerHTML = '<img src="/images/brightness-high.svg"height="20px"class="d-inline" style="margin-left:-3px"/>';
            map.setOptions({ styles: styles["dark"] });

        }
        else {
            themeButton.classList.remove("btn-light");
            themeButton.classList.add("btn-secondary")
            themeButton.innerHTML = '<img src="/images/brightness-high-fill.svg" height="20px" class="d-inline" style="margin-left:-3px"/>';
            map.setOptions({ styles: styles["light"] });
        }
    })
}

const SearchButton = document.getElementById("go");
const LocationInput = document.getElementById("gotolocation");



//MAP INITIALIZATION SECTION----------------------
function initMap() {

    options = {
        zoom: 14,
        center: athens,
        styles: styles["light"]
    }
    map = new google.maps.Map(document.getElementById('map'), options);
    //MY LOCATION FUNCTION CALL ------------------------------
    const myLocationDiv = document.createElement("div");
    myLocation(myLocationDiv, map);
    map.controls[google.maps.ControlPosition.RIGHT_BOTTOM].push(myLocationDiv);
    const searchAreaDiv = document.createElement("div");
    searchArea(searchAreaDiv, map);
    map.controls[google.maps.ControlPosition.TOP_CENTER].push(searchAreaDiv);
    const themeAreaDiv = document.createElement("div");
    themeButtonDiv(themeAreaDiv, map);
    map.controls[google.maps.ControlPosition.RIGHT_BOTTOM].push(themeAreaDiv);

    autocomplete = new google.maps.places.Autocomplete(document.getElementById("gotolocation"));
    autocomplete.addListener('place_changed', onPlaceChanged);

    SearchButton.addEventListener("click", () => new google.maps.Geocoder().geocode({ 'address': document.getElementById("gotolocation").value },
        function (results, status) {

            if (status == "OK") {
                map.setZoom(14);
                var infoWindow = new google.maps.InfoWindow();
                infoWindow.setPosition(results[0].geometry.location);
                infoWindow.setContent(results[0].formatted_address);
                infoWindow.open(map);
                map.setCenter(results[0].geometry.location);

            }
            else {
                document.getElementById('noresultButton').click();
            }
        }
    ));
   
    google.maps.event.addListener(map, 'idle', getBounds)
}

function onPlaceChanged() {
    var place = autocomplete.getPlace();
    if (!place.geometry) {
        document.getElementById("gotolocation").placeholder = "Enter a name for a place";
    }
}
LocationInput.addEventListener("keyup", function (event) {
    if (event.keyCode === 13) {
        event.preventDefault();
        SearchButton.click();
    }
});

var swLat;
var swLng;
var neLat;
var neLng;

function getBounds() {

    bounds = map.getBounds();
    swLat = bounds.getSouthWest().lat();
    swLng = bounds.getSouthWest().lng();
    neLat = bounds.getNorthEast().lat();
    neLng = bounds.getNorthEast().lng();

}

$("#filters").click(GetLocations);


function GetLocations() {
    $.ajax({
        url: '/Event/GetLocations',
        type: "GET",
        dataType: 'JSON',
        data: {
            swLat: swLat, swLng: swLng, neLat: neLat, neLng: neLng,
            Price: $('#ticket').val(), Day: $('#day').val(), CategoryId: $('#CategoryId').val()
        },
        success: function (returnedData) {

            if (returnedData.length==0) {
                document.getElementById('noresultButton').click();
            }

            mapMarkers.forEach(function (marker) {
                marker.setMap(null);
                marker = null;
            });

            mapMarkers = [];
            var markers = [];
            $("#partial").hide();
            $.each(returnedData, function (index, item) {
                var marker = {};

                marker["lat"] = item.Latitude;
                marker["lng"] = item.Longitude;
                marker["id"] = item.LocationId;
                marker["num"] = item.EventNumber;
                marker["day"] = item.Day;
                marker["price"] = item.Price;
                marker["categoryid"] = item.CategoryId;
                marker["events"] = item.AssociatedEvents;
                //push the current marker details in markers array
                markers.push(marker);
            })

            initializeMap(markers);

        },
        error: function () {
            alert("failed");
        }
    })
}


function initializeMap(markers) {

    var infoWindow = new google.maps.InfoWindow();
    var content;
    //loop through each marker data
    for (i = 0; i < markers.length; i++) {

        var locationData = markers[i];

        var myLatlng = new google.maps.LatLng(locationData.lat, locationData.lng);
        var labelText = String(locationData.num);
        var marker = new google.maps.Marker({
            position: myLatlng,
            title: locationData.title,
            label: { text: labelText },
            map: map
        });

        mapMarkers[i] = marker;

        (function (marker, locationData) {
            google.maps.event.addListener(marker, "click", () => {
                $.ajax({
                    url: '/Event/GetAssociatedEvents',
                    type: "POST",
                    cache: false,
                    dataType: 'html',
                    data: {
                        AssociatedEvents: locationData.events
                    },
                    success: function (returnedEvents) {
                        console.log(returnedEvents);
                        $("#partial").show();
                        $("#partial").html(returnedEvents);
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        alert("failed");
                        console.log('STATUS: ' + textStatus + '\nERROR THROWN: ' + errorThrown);
                    }
                })
            });
        })(marker, locationData);

    }
}
const styles = {
    light: [
        {
            "featureType": "all",
            "elementType": "labels.text",
            "stylers": [
                {
                    "color": "#878787"
                }
            ]
        },
        {
            "featureType": "all",
            "elementType": "labels.text.stroke",
            "stylers": [
                {
                    "visibility": "off"
                }
            ]
        },
        {
            "featureType": "landscape",
            "elementType": "all",
            "stylers": [
                {
                    "color": "#f9f5ed"
                }
            ]
        },
        {
            "featureType": "road.highway",
            "elementType": "all",
            "stylers": [
                {
                    "color": "#f5f5f5"
                }
            ]
        },
        {
            "featureType": "road.highway",
            "elementType": "geometry.stroke",
            "stylers": [
                {
                    "color": "#c9c9c9"
                }
            ]
        },
        {
            "featureType": "water",
            "elementType": "all",
            "stylers": [
                {
                    "color": "#aee0f4"
                }
            ]
        }
    ],
    dark: [
        {
            "elementType": "geometry",
            "stylers": [
                {
                    "color": "#212121"
                }
            ]
        },
        {
            "elementType": "labels.icon",
            "stylers": [
                {
                    "visibility": "off"
                }
            ]
        },
        {
            "elementType": "labels.text.fill",
            "stylers": [
                {
                    "color": "#757575"
                }
            ]
        },
        {
            "elementType": "labels.text.stroke",
            "stylers": [
                {
                    "color": "#212121"
                }
            ]
        },
        {
            "featureType": "administrative",
            "elementType": "geometry",
            "stylers": [
                {
                    "color": "#757575"
                }
            ]
        },
        {
            "featureType": "administrative.country",
            "elementType": "labels.text.fill",
            "stylers": [
                {
                    "color": "#9e9e9e"
                }
            ]
        },
        {
            "featureType": "administrative.land_parcel",
            "stylers": [
                {
                    "visibility": "off"
                }
            ]
        },
        {
            "featureType": "administrative.locality",
            "elementType": "labels.text.fill",
            "stylers": [
                {
                    "color": "#bdbdbd"
                }
            ]
        },
        {
            "featureType": "poi",
            "elementType": "labels.text.fill",
            "stylers": [
                {
                    "color": "#757575"
                }
            ]
        },
        {
            "featureType": "poi.park",
            "elementType": "geometry",
            "stylers": [
                {
                    "color": "#181818"
                }
            ]
        },
        {
            "featureType": "poi.park",
            "elementType": "labels.text.fill",
            "stylers": [
                {
                    "color": "#616161"
                }
            ]
        },
        {
            "featureType": "poi.park",
            "elementType": "labels.text.stroke",
            "stylers": [
                {
                    "color": "#1b1b1b"
                }
            ]
        },
        {
            "featureType": "road",
            "elementType": "geometry.fill",
            "stylers": [
                {
                    "color": "#2c2c2c"
                }
            ]
        },
        {
            "featureType": "road",
            "elementType": "labels.text.fill",
            "stylers": [
                {
                    "color": "#8a8a8a"
                }
            ]
        },
        {
            "featureType": "road.arterial",
            "elementType": "geometry",
            "stylers": [
                {
                    "color": "#373737"
                }
            ]
        },
        {
            "featureType": "road.highway",
            "elementType": "geometry",
            "stylers": [
                {
                    "color": "#3c3c3c"
                }
            ]
        },
        {
            "featureType": "road.highway.controlled_access",
            "elementType": "geometry",
            "stylers": [
                {
                    "color": "#4e4e4e"
                }
            ]
        },
        {
            "featureType": "road.local",
            "elementType": "labels.text.fill",
            "stylers": [
                {
                    "color": "#616161"
                }
            ]
        },
        {
            "featureType": "transit",
            "elementType": "labels.text.fill",
            "stylers": [
                {
                    "color": "#757575"
                }
            ]
        },
        {
            "featureType": "water",
            "elementType": "geometry",
            "stylers": [
                {
                    "color": "#000000"
                }
            ]
        },
        {
            "featureType": "water",
            "elementType": "labels.text.fill",
            "stylers": [
                {
                    "color": "#3d3d3d"
                }
            ]
        }
    ]
}
  //function createInfoWindow(data) {

            //    var content = "<div class='border-1 bg-warning rounded-2 flex' >" +
            //        labelText + "Event(s) in this location."
            //    '<p><button class="btn btn-secondary border-dark" type="button" data-bs-toggle="collapse" ' +
            //        'data-bs-target="#collapseEvent" aria-expanded="false" aria-controls="collapseEvent">View details</button></p>';

            //    infoWindow.setContent(content);
            //    infoWindow.open(map, marker);
            //}

//SEARCH BUTTON, ACTION AT ENTER SECTION ----------------------------------------



    //const sampleMarker = new google.maps.Marker({
    //    position: markerPosition,
    //    map: map,
    //    icon: icon
    //});

    //const infoWindow = new google.maps.InfoWindow({
    //    content:


    //});

    //sampleMarker.addListener("click", () => {
    //    infoWindow.open({
    //        anchor: sampleMarker,
    //        map,
    //        shouldFocus: false,
    //    });
    //});

