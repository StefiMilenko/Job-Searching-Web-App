
//Vue.use(Vuetify);


console.log("NEW VUE");
window.vApp = new Vue(Object.assign(window.vueStart,
    {
        el: '#app',

        vuetify: new Vuetify({icons: {
                    iconfont: 'mdi', // Use the MDI fonts as the iconfont
                },
            }),
        components: {
            VueSlider: window['vue-slider-component'],
        //    VCombobox: window['v-combobox'],
            'comment-card': window.commentComp,
            'ocena-card': window.ocenaComp,
            'app-header': window.headerComp,
        },
        mounted: function() {
            
            LOADMAP();
            setTimeout(()=>{
                //this.showMap = false;
            },100);

            
            getUserIpGeo((resp)=>{
                console.log("USER IP GEO",resp);
                window.mapUser([resp.latitude,resp.longitude]);
                //resp.latitude
                //resp.longitude
            });
        }
    }
));



let jobId = urlQuery().job;
if(jobId) {
    Req("GET").path("Posao/VratiPosao").query({PosaoId:jobId}).onDone(function(r){
        window.vApp.selectedJob = first(JSON.parse(r));
        reqComms();
    }).send();
}


function reqJobs(filters=undefined){
    //window.vApp.jobs = [];
    if(!filters){
        filters = {};
        renames = {
            "enteredDistance":"udaljenost",
            "nazivPoslodavca":"firma",
            "search":"query"
        };
        [
            "search",
            "lokacija",
            "nazivPoslodavca",
            "nazivPosla", //experience level
            "plataOd",
            "plataDo",
            "plataValuta",
            //"plataInfo",
            "sektorRada",
            "industrija",
            "vrstaPosla",
            "tipPosla",
            "enteredDistance",
        ].forEach(f=>{
            let v = (window.vApp[f]!=="")?window.vApp[f] : null;
            if(v!==null) filters[(f in renames)?renames[f]:f]= v;
        });
         //    
    }
    //filters.count = 10;
    //xfilters.page = 0;

    console.log("REQ JOBS ",filters);

    Req("GET","/Posao/PreuzmiPoslove/1").query(filters).onDone((r)=>{
        console.log("REQ JOBS DONE ",r);
        //alert(r.rt);
        //console.log(r.rt);
        //alert('vidi konzolu i reci mi output.');
        console.log(r);
        let jobs = JSON.parse(r.rt);
        console.log(jobs);
        window.vApp.clearJobs();
        jobs.forEach(j=>window.vApp.jobs.push(j));

        jobsGotten();
    }).send();
}
function parseCom(modelKomentar){
    return {
        id:modelKomentar.id,
        sadrzaj:modelKomentar.sadrzaj,
        autor:modelKomentar.autor,
        odgovori: modelKomentar.odgovori ?? [],
        
        isExpanded: false,
        showReply:false,
        replyText:'',
    };
}
function reqComms(){

    //window.vApp.comments = [];
    let job = window.vApp.selectedJob;
    if(!job) throw new Error("No selected job.");

    Req("GET","/Komentar/VratiKomentarePosla").query({PosaoId:Number(job.id)}).onDone((r)=>{
        console.log(JSON.parse(r.rt));
        window.vApp.comments = JSON.parse(r.rt).map(parseCom);
        collapseComments(true,window.vApp.comments,1);
    }).send();

    reqRate();
}
function reqRate(){

    //window.vApp.comments = [];
    let job = window.vApp.selectedJob;
    if(!job) throw new Error("No selected job.");

    Req("GET","/Posao/VratiOcene").query({PosaoId:Number(job.id)}).onDone((r)=>{
        console.log(JSON.parse(r.rt));
        let odg = JSON.parse(r.rt);
        odg.sve ??= [];
        if(odg.moj){
            odg.sve = odg.sve.filter(o=>o.id!=odg.moj.id);

            window.vApp.newOcena_1 = odg.moj.plata;
            window.vApp.newOcena_2 = odg.moj.okruzenje;
            window.vApp.newOcena_3 = odg.moj.napredovanje;
            window.vApp.newOcena_4 = odg.moj.posoKuca;
        }
        window.vApp.mojaOcena = odg.moj;
        window.vApp.sveOcene = odg.sve;//.map(parseCom);
        
        window.vApp.avgOcena = odg.prosecna;
        //collapseComments(true,window.vApp.comments,1);
    }).send();
}


function collapseComments(state,comments, depth, currentDepth = 0) {
    if(comments.forEach === undefined) comments = [comments];
    for (let comment of comments) {
        console.log("COLLAPSE ",comment.sadrzaj, currentDepth, depth);
        if(comment.isExpanded==false && state==false) continue;
        if(state==true && currentDepth==depth){ window.vApp.expandCom(comment,true,null); comment.isExpanded = false; }
        else window.vApp.expandCom(comment,(currentDepth < depth)?state:!state,(currentDepth < depth)?
        function(com){
            collapseComments(state, com.odgovori, depth, currentDepth + 1);
        }:null);
        
    }
}


function jobsGotten(jobs=[]){
   window.clearLocations();
   let lokacije = [];
   window.vApp.jobs.forEach(j=>lokacije.push(j.lokacija));
    window.markLocations(lokacije);
/*
    if(jobs.filter(j=>j.id==window.vApp.selectedJob.id).length==0 ){

    }
    */
}

document.getElementById("moreFiltersBtn").addEventListener("click", function(e) {
    e.stopPropagation();
    var filterContainer = document.getElementById("filterContainer");
    if (filterContainer.style.display === "none") {
      filterContainer.style.display = "block";
    } else {
      filterContainer.style.display = "none";
    }
  });

jobsGotten();
reqJobs(); //get all jobs