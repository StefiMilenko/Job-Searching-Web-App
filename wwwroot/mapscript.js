

window.geoWaiters = [];
navigator.geolocation.watchPosition(
    // Success callback function
    function(position) {
        // Get the user's latitude and longitude coordinates
        const lat = position.coords.latitude;
        const lng = position.coords.longitude;
        window.lastUserLoc = [lat,lng];
        let cb;
        while(cb = window.geoWaiters.pop())
            cb({latitude:lat,longitude:lng});
    },
    function(error) {
        console.error("Error getting user location:", error);
    }
);
window.getUserIpGeo = function getUserIpGeo(callback){
    window.geoWaiters.push(callback);
    return;
    Req('GET','check?access_key=51b6ddb546de6b142bd56da040005be1','http://api.ipstack.com').onDone((res)=>{
        callback(JSON.parse(res.rt));
    }).cred(false).send();
    // #region Response https://ipstack.com/documentation#jsonp
    /*
    {
"ip": "155.52.187.7",
"type": "ipv4",
"continent_code": "NA",
"continent_name": "North America",
"country_code": "US",
"country_name": "United States",
"region_code": "MA",
"region_name": "Massachusetts",
"city": "Boston",
"zip": "02115",
"latitude": 42.3424,
"longitude": -71.0878,
"location": {
"geoname_id": 4930956,
"capital": "Washington D.C.",
"languages": [
    {
      "code": "en",
      "name": "English",
      "native": "English"
    }
],
"country_flag": "https://assets.ipstack.com/images/assets/flags_svg/us.svg",
"country_flag_emoji": "🇺🇸",
"country_flag_emoji_unicode": "U+1F1FA U+1F1F8",
"calling_code": "1",
"is_eu": false
},
"time_zone": {
"id": "America/New_York",
"current_time": "2018-03-30T07:54:25-04:00",
"gmt_offset": -14400,
"code": "EDT",
"is_daylight_saving": true
},
"currency": {
"code": "USD",
"name": "US Dollar",
"plural": "US dollars",
"symbol": "$",
"symbol_native": "$"
},
"connection": {
"asn": 40127,
"isp": "Longwood Medical and Academic Area (LMA)"
}
}
*/
//#endregion
}
window.geocode = function(locationStr,callback=null){
    if(callback){
        geocoder.geocode(locationStr,callback);
    }else{
        return new Promise((resolve, reject) => {
            geocoder.geocode(locationStr, function(results) {
                if (results && results.length > 0) {
                    resolve(results);
                } else {
                    reject(null);
                }
            });
        });
    }
}

window.locationUpdWaiting = false;
window.locationUpdChecks = 0;
window.locationOptions = {};
setInterval(function(){
    if(window.vApp.lokacija in window.locationOptions){
        window.mapUser(window.locationOptions[window.vApp.lokacija]);
    }
},500);
window.locationInputChanged = async function(){
    console.log(window.vApp.lokacija);
    
    window.locationUpdChecks++;
    //if(window.locationUpdChecks > 1) return;
    geocoder.geocode(window.vApp.lokacija, function(geocodeResults) {
        
        //window.locationOptions = {};
        //console.log("GEOOCDRESULT",geocodeResults);
        window.vApp.locationSuggestions.length = 0;
        for (var i = 0; i < geocodeResults.length; i++) {
            console.log(geocodeResults[i]);
            window.locationOptions[geocodeResults[i].name] = [geocodeResults[i].center.lat, geocodeResults[i].center.lng];//geocodeResults[i].center;//L.latLng(geocodeResults[i].center.lat, geocodeResults[i].center.lng);  
            
            window.vApp.locationSuggestions.push(geocodeResults[i].name);
        }
/*
        if(window.locationUpdChecks>1){
            window.locationUpdChecks=0;
            window.locationInputChanged();
        }
        */
    });

}
window.LOADMAP = function LOADMAP(){
    /*
    window.mapLoads ??= 0; window.mapLoads++;
    console.log("LOADED");
    if(window.mapLoads<2) return;
    if(window.mapLoads==2) 
    console.log("CONFIRMED");
    */
    let mapOptions = {
center:[43.3073219, 21.9076474],
zoom:10,
maxZoom: 15,
minZoom: 2
}

let map = new L.map('map' , mapOptions);

let layer = new L.TileLayer('http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png');
map.addLayer(layer);
var LocationLayer = L.layerGroup().addTo(map);
geocoder = new L.Control.Geocoder.Nominatim();


    let apiKey = "9098219d18994560be55415be86df062";

    let myMarker = null; //LatLng
    //let myMarker = [43.3073219, 21.5076474]; //koordiante markera lokacije, ovaj marker treba korisnik da stavi ili da ukuca svoju lokaciju pa to da se prevede//
    //let markerPosition = new L.LatLng(myMarker[0], myMarker[1]); //postavljanje markera
    //let enteredDistance = null; //KM, ovu vrednost treba korisnik da unese.
    let myRangeCircle = null;

    let jobIcon = L.divIcon({
        className: 'custom-div-icon',
        html: "<div style='background-color:#c30b82;color:#fff'>Job</div>",
        iconSize: [30, 42],
        iconAnchor: [15, 42]
    });

    window.allMarkers = [];
    window.clearLocations = function(){
        window.allMarkers.forEach(m=>m.remove());
        window.allMarkers = [];
    }
    window.markLocations = function(locations){
        function roundToFourDecimals(num) {
            return Math.round(num * 10000) / 10000;
        }
        let clumpedMarkers = [];
        [...locations].forEach((location,idx)=>{
            geocoder.geocode(location, function(results) { 
                if(results.length==0){ console.log("unable to resolve location ",location); return; }
                let latLng= L.latLng(results[0].center.lat, results[0].center.lng);
                console.log(latLng);
                let draw = true;
                if (myMarker && window.vApp.enteredDistance != null && window.vApp.enteredDistance != 0 && window.vApp.enteredDistance != 2000) //ovde proverava da li je uneta udaljenost 
                {
                    let distBetween = myMarker._latlng.distanceTo(latLng); 
                    console.log(distBetween);
                    draw = (distBetween <= window.vApp.enteredDistance*1000); //ako jeste onda uporeduje sa unetu vrednost
                }
                
                if(draw) {
                    let m = L.marker(latLng);//, {icon: jobIcon});
                    m.jobIdx = idx;
                    m.on('click contextmenu', function(e) {
                        console.log('Click type: ', e.type);
                        console.log('Marker location: ', e.latlng);
                        this.closePopup();
                    });
                    
                    m.on('mouseover', function(e) {
                        this.openPopup();
                    });
                    m.on('mouseout', function(e) {
                        this.closePopup();
                    });

                    window.allMarkers.push( 
                        m
                        .bindPopup(location, { offset: L.point(0, -10) })
                        .addTo(map)
                    );

                    /*
                    window.allMarkers.push(
                        // Add text above marker
                        L.marker(latLng, {
                            icon: L.divIcon({
                                className: 'map-label',   // Set class for CSS styling
                                html: 'Job'
                            }),
                            opacity: 1.0, // Adjust this if needed (0 = fully transparent)
                            interactive: false // Ignore mouse events for the text marker
                        }).addTo(map)
                    );
                    */
                }
            });
        });
    }
    window.mapCircle = function(){
        
        if (myRangeCircle) {
            map.removeLayer(myRangeCircle);
            myRangeCircle = null;
        }
        
        if (myMarker && (window.vApp.enteredDistance!=0 && window.vApp.enteredDistance!=2000)) {
            //console.log(myMarker);
            //console.log(myMarker._latlng);
            myRangeCircle = L.circle(myMarker._latlng
                , {
                color: 'rgba(0,0,255,0.5)',
                fillColor: '#30f',
                fillOpacity: 0.2,
                radius: window.vApp.enteredDistance * 1000
            }).addTo(map);
            
        }
    }

    window.setLocation = function(latLng = null){
        if (myMarker){
            map.removeLayer(myMarker);
            myMarker = null;
        }
        if(latLng){

        }
        var lokacija = window.vApp.lokacija;
        if (lokacija){
            geocoder.geocode(lokacija, function(results) { 
                if(results.length==0){ console.log("unable to resolve location ",lokacija); return; }
                let latLng= L.latLng(results[0].center.lat, results[0].center.lng);
                myMarker = L.marker(latLng), {
                    icon: L.icon({
                        iconUrl: 'assets/marker-64.png',
                        iconSize: [48, 48],
                        iconAnchor: [24, 48]
                    }).addTo(map)
                }
                map.setView([latlong[0], latlong[1]], 7);
                window.mapCircle();
            })  
        }
    }
    window.mapUser = function mapUser(latlong){
        if (myMarker) {
            myMarker.remove();
          myMarker = null;
        }

		//add marker 
		myMarker = L.marker([latlong[0], latlong[1]], {
            icon: L.icon({
                iconUrl: 'assets/marker-64.png',
                iconSize: [48, 48],
                iconAnchor: [24, 48]
            })}).addTo(map);
            //Sets the view of the map (geographical center and zoom) with the given animation options.
            map.setView([latlong[0], latlong[1]], 7);
        
        window.mapCircle();
    }

const addressSearchControl = L.control.addressSearch(apiKey, {
    position: 'topleft',

	//set it true to search addresses nearby first
    mapViewBias:true,

    //Text shown in the Address Search field when it's empty
    placeholder:"Enter your address",

    // /Callback to notify when a user has selected an address
    resultCallback: (address) => {
        if (address) window.mapUser([address.lat,address.lon]);
      },


      //Callback to notify when new suggestions have been obtained for the entered text
      suggestionsCallback: (suggestions) => {
        console.log(suggestions);
      }
});


map.addControl(addressSearchControl);
/*
map.on('click', function(e) {
    myMarker.setLatLng(e.latlng);
    window.mapCircle();
});
*/
window.removeLocation = function(emptyLoc = false){
    if (myMarker) {
        myMarker.remove();
      myMarker = null;
    }
    window.mapCircle();
    if(emptyLoc) window.vApp.lokacija = '';
}
map.on('contextmenu', function(e) {
    e.originalEvent.preventDefault();
    window.mapUser(e.latlng);

    geocodeService.reverse().latlng(e.latlng).run(function (error, result) {
        if (error) {  removeLocation(); return; }
        let elems = result.address.LongLabel.split(',')
        address = result.address.LongLabel;
        window.vApp.lokacija = address;
    });  
});

map.on("load",function() { setTimeout(() => {
    map.invalidateSize();
 }, 1); });
 

 window.refreshMap = function(){
    setTimeout(()=>{
        map.invalidateSize();
    },100);
 }

    }
//setLocation();
