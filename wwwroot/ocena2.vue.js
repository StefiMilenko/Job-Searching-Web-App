Vue.component('star-rating', VueStarRating.default)
window.ocenaComp = Vue.component('ocena-card', vueCompLink({
    props: ['ocena', 'user'],
    template: `
        <div class="card mb-2">
            <div class="card-body comBdy">
                <a class="card-text">{{ window.vApp.getPoslodavacName(ocena.korisnik) }}</a>
                
                <div class="comment-section mt-4 card">
                    <div class="card-body">

                    <div>
                    <label>Plata&nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;</label>
                    <star-rating :read-only="true||ocena.korisnik.id != window.loggedInUser.id" :inline="true" :star-size="30" v-model="ocena.plata"></star-rating>
                </div>
                <div>
                    <label>Okruzenje&nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp; &nbsp;</label>
                    <star-rating :read-only="true||ocena.korisnik.id != window.loggedInUser.id" :inline="true" :star-size="30" v-model="ocena.okruzenje"></star-rating>
                </div>
                <div>
                    <label>Napredovanje&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</label>
                    <star-rating :read-only="true||ocena.korisnik.id != window.loggedInUser.id" :inline="true" :star-size="30" v-model="ocena.napredovanje"></star-rating>
                </div>
                <div>
                    <label>Balans poso-zivot&nbsp; &nbsp;</label>
                    <star-rating :read-only="true||ocena.korisnik.id != window.loggedInUser.id" :inline="true" :star-size="30" v-model="ocena.posoKuca"></star-rating>
                </div>
                </div>
            </div>

                
            </div>
        </div>`,
    methods: {
        izmeniOcena(ocena) {
            this.$emit('izmeni-ocena', ocena);
        },
        obrisiOcena(ocena) {
            this.$emit('obrisi-ocena', ocena);
        }
    },
    created: function() {
        
    }
}));
