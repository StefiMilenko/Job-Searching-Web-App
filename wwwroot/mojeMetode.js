function baseUrl(){
    return location.protocol + '//' + location.host + '/';
}
function first(obj){
    if(obj && obj.forEach)
        return obj[0];
    return obj;
}
function vueCompLink(component){
/*
    this.$options.computed = new Proxy(component.computed || {}, {
        get: function(target, property) {
            if (property in target) {
                return target[property];
            } else {
                return window.vApp[property];
            }
        }
    });
*/
    component.computed ??= {};
    let computed = component.computed;
    //if(window.vApp == null) return;
    let data = window.vueStart.data; //window.vApp.$options.data();
    let comp = window.vueStart.computed;//window.vApp.$options.computed;
    for(let k in data)
        if(k!="refresh")
        computed[k] = {
            cache: false,
            get: function(){
                    let r = null;
                    if(window.vApp) r = window.vApp[k];
                    else r = data[k];
                    if(typeof r == 'function') r=r();
                    return r;
                }
            }
    for(let k in comp)
        if(k!="refresh")
        computed[k] = {
            cache: false,
            get: function(){
                let r = null;
                if(window.vApp) r = window.vApp[k];
                else r = comp[k];
                if(typeof r == 'function') r=r();
                return r;
            }
        }

    return component;
    
}
function loginUrl(){ return baseUrl()+"login.html";}
function profileUrl(){ return baseUrl()+"profile.html";}
function indexUrl(){ return baseUrl()}//+"index.html";}
function makeJobUrl(){return baseUrl()+"makeJob.html";}
function reqGetUsrInfo(id,_fn){
   Req("GET","/Korisnik/VratiKorisnikaId/").query({KorisnikId:Number(id)}).onDone(function(r){ //.query({KorisnikId:Number(id)})
        console.log(r.rt);
        if(r.status==200){ if(_fn)_fn(fixUser(first(JSON.parse(r.rt)))); }
        else{ console.error(r); if(_fn)_fn(null); }
    }).send();
}

window.loggedInUser ??= null;
function fixUser(user){
    if(user.cv !== undefined) user.uloga = 0;
    else user.uloga = 1;
    return user;
}
function reqAuthToAcc(_fn,force=false){
    if(force==false && window.loggedInUser && _fn) _fn(window.loggedInUser);
    //let auth = parseCookies()["authentication-token"];
    Req("GET","/Korisnik/VratiKorisnika").onDone(function(r){
        if(r.status==200){ 
            window.loggedInUser = fixUser(first(JSON.parse(r.rt)));
            console.log("LOGGED IN ",window.loggedInUser);
            if(window.vueHeader) window.vueHeader.upd();
            //if(window.vueHeader){ console.log("UPDATING HEADER"); window.vueHeader.refresh++;}
            if(_fn)_fn(window.loggedInUser);
        }else{ console.error(r); if(_fn)_fn(null); }
    }).send();
}
function logOut(after=''){
    Req("POST","Account/Logout").onDone(function(r){
        console.log("LOGOUT ",r);
        window.loggedUser = null;
        if(after=='search') window.location.href = indexUrl();
        else if(after=='login') window.location.href = loginUrl();
    }).send();
    //document.cookie = "";
    //if(window.vApp) window.vApp.$options.data().loggedInUser=null;   
}
function parseCookies(){
    return document.cookie
  .split(';')
  .map(v => v.split('='))
  .reduce((acc, v) => {
    acc[decodeURIComponent(v[0].trim())] = decodeURIComponent(v[1].trim());
    return acc;
  }, {});
}

function urlQuery(){
    return new Proxy(new URLSearchParams(window.location.search), {
    get: (searchParams, prop) => searchParams.get(prop),
  });
}
  // Get the value of "some_key" in eg "https://example.com/?some_key=some_value"
//let value = params.some_key; // "some_value"
  

function renameProperties(object, renameMap) {
    for (let oldPropertyName in renameMap) {
        if (object.hasOwnProperty(oldPropertyName)) {
        const newPropertyName = renameMap[oldPropertyName];
        object[newPropertyName] = object[oldPropertyName];
        delete object[oldPropertyName];
        }
    }
}

function remoteLog(v){
  const xhr = new XMLHttpRequest();
  xhr.open("POST", "https://8235.lukak.link/", true);
  xhr.setRequestHeader("Content-Type", "application/json");
  xhr.send(JSON.stringify({resp: JSON.stringify(v)}));
}
function Req(__method="POST",__path=null,__url=null){
    return {
      _method:__method,
      _json: null,
      _query: null,
      _onDone:null,
      _url:__url,
      _path:__path,
      _cred:true,
      _async:true,
      async:function(a){this._async=a;return this;},
      json:function(o){this._json = o;return this;},
      query:function(o){
          this._query = "?" + Object.keys(o).
          map((key)=>(key+"="+encodeURIComponent(o[key]))).
          join("&");
          return this;
      },
      cred:function(b){this._cred=b;return this;},
      path:function(p){this._path=p;return this;},
      url:function(p){this._url=p;return this;},
      onDone:function(f){this._onDone=f;return this;},
      method:function(m){this._method=m;return this;},
      send:function(){
          const xhr = new XMLHttpRequest();
          let ths = this;
          xhr.onreadystatechange = function(){
              if (xhr.readyState === XMLHttpRequest.DONE) {
                  if(ths._onDone){
                      ths._onDone(
                          {rt:xhr.responseText,r:xhr.response,status:xhr.status,full:xhr}
                      );
                  }
                  if (xhr.status === 200){  }
                  else console.error("ajax request failed with status " + xhr.status);
              }
          };
          
          let body = undefined;
          let url = (this._url===null)?baseUrl():this._url;

          if(url.endsWith('/')==false) url+='/';
          if(this._path!==null){
               url += (this._path.startsWith('/'))?this._path.substring(1):this._path;
          }
  
          if(this._query!==null){
              if(url.endsWith('/')) url=url.slice(0, -1);
              url += this._query;
          }
  
          xhr.finalUrl = url;
          xhr.open(this._method, url, this._async);
  
          if(this._json!==null){
              xhr.setRequestHeader("Content-Type", "application/json");
              body = JSON.stringify(this._json);
          }
          
          xhr.withCredentials = this._cred?true:false;
          console.log("Sending request to ",url," with body ",body);
          return xhr.send(body);
      },
    };
}

reqAuthToAcc();