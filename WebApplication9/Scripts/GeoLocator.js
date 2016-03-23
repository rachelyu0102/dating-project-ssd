
window.onload = function find() {
    var startPos;
    var latitude;
    var longitude
    var geoOptions = {
        enableHighAccuracy: true,
    }
    var geoError = function (position) {
        console.log('Error occurred. Error code: ' + error.code);
        // error.code can be:
        //   0: unknown error
        //   1: permission denied
        //   2: position unavailable (error response from location provider)
        //   3: timed out
    };
    var geoSuccess = function (position) {
        startPos = position;
        latitude = startPos.coords.latitude;
        longitude = startPos.coords.longitude;
        reverseGeoLocate(latitude, longitude)
    }
    navigator.geolocation.getCurrentPosition(geoSuccess, geoError, geoOptions);
}
function reverseGeoLocate(latitude, longitude) {
    var reverGeo = 'http://maps.googleapis.com/maps/api/geocode/json?latlng=' + latitude + ',' + longitude + '&sensor=false'
    var province;
    var country;
    var selectorCountry=1;
    var selectorProvince=0;
    $.getJSON(reverGeo,
        function (data) {
            if (data == null) {
                $('#clientsResult').text('clients not found.');
                return;
            }
            console.log(data);
            if (data.results[8].address_components.length > 2)
            {
            province = data.results[8].address_components[selectorProvince+1].long_name;
            country = data.results[8].address_components[selectorCountry+1].long_name;

            }
            else {
                province = data.results[8].address_components[selectorProvince].long_name;
                country = data.results[8].address_components[selectorCountry].long_name;
            }
            getClients(province, country);
        })
    .fail(
        function (jqueryHeaderRequest, textStatus, err) {
            $('#clientsResult').text('Find error: ' + err);
        });
}

function getClients(province, country) {
    var wait = $('#countryId option:selected').text();
    var countrySelect = $('#countryId option:selected');
    var provinceSelect = $('#provinceId option:selected');
   // alert(countrySelect.text());
    //For FindADate Autofill{
    if (provinceSelect.text() === wait) {//we want it to match
      //  alert(countrySelect.text());
      //  setTimeout(alert(countrySelect.text()), 50);//wait 50 millisecnds then recheck
        return;
    }
     countrySelect.text(country);
    //real action
    if (provinceSelect.text() === wait) {//we want it to match
        setTimeout(alert(provinceSelect.text()), 50);//wait 50 millisecnds then recheck
        return;
    }
    provinceSelect.text(province);
    //}
}