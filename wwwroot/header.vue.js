window.headerComp = Vue.component('app-header', vueCompLink(
    {
    template: `


    <nav class="navbar navbar-expand-lg navbar-dark bg-dark" id="navbar-main" style="padding: 0; height: fit-content;">
        <div class="container-fluid">
          <div class="collapse navbar-collapse" id="navbarSupportedContent">
            <ul class="navbar-nav me-auto mb-2 mb-lg-0">
              <li class="nav-item">
                <a class="nav-link active" aria-current="page" href="./homepage.html">
                    <img src="assets/home.png" width="18px" height="18px">
                    &nbsp;Pocetna</a>
              </li>
              <li class="nav-item">
                <a class="nav-link" href="./">Search</a>
              </li>
              <li class="nav-item">
                <a class="nav-link" href="#scrollspyAboutUs">O nama</a>
              </li>
              <li class="nav-item">
                <a class="nav-link" href="#scrollspyContactUs">Kontaktirajte nas</a>
              </li>
            </ul>
            <slot></slot>
            <button :key="refresh" v-if="loggedInUser && loggedInUser.uloga == 1" onclick="window.location.href=makeJobUrl()" 
             type="button" class="btn btn-outline-info" style="padding-left: 25px; padding-right: 25px; white-space: nowrap;color:white;">
                <img src="assets/plus.png" width="18px" height="18px">
                &nbsp;Dodaj posao</button>&nbsp;&nbsp;&nbsp;
            
    <button :key="refresh" v-if="!loggedInUser" id="profBadge" class="btn btn-success"  type="button"  style="color:white;padding-left: 25px; padding-right: 25px; white-space: nowrap;" onclick="window.location.href=loginUrl();"><img src="assets/login.png" width="18px" height="18px">&nbsp;Log in</button>  
    <button :key="refresh" v-if="loggedInUser" id="profBadge" class="btn btn-success"  type="button"  style="color:white;padding-left: 25px; padding-right: 25px; white-space: nowrap;" onclick="window.location.href=profileUrl()+'?id='+window.loggedInUser.id;"><img src="assets/login.png" width="18px" height="18px">&nbsp; {{loggedInUser.ime}} {{loggedInUser.prezime}}</button>    

          </div>
        </div>

      </nav>
      
    `,
    data: function() {
        return {
            refresh: 0
        };
    },
    methods: {
        upd:function(){
            console.log("Header update ",this.loggedInUser);
            this.refresh++;
        }
    },
    mounted: function(){
        window.vueHeader = this;
        console.log("header: ",this.loggedInUser);
    },  
    computed :{
        
    },
    created: function() {
   //     vueCompLink(this.$options.computed);
    }
}));