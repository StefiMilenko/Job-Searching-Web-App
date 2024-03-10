
window.commentComp = Vue.component('comment-card', vueCompLink({
    props: ['comment', 'user'],
    template: `
        <div class="card mb-2">
            <div class="card-body comBdy">
                <button v-show="comment.odgovori.length>0" class="btn clpsBtn" :class="{'btn-outline-secondary': comment.isExpanded}" @click="expandCom(comment)">
                    <span v-if="comment.isExpanded" style="font-size:19px;top:-7px;">&#9650;</span>
                    <span v-else>&#9654;</span>
                </button>
                <a class="card-text" :href="'/profile.html?id='+comment.autor.id">{{ window.vApp.getPoslodavacName(comment.autor) }}</a>
                
                <p class="card-text">{{ comment.sadrzaj }}</p>
                <button class="btn btn-secondary mt-2" @click="toggleReplyBox" v-if="canComment">Reply</button>

                
                <div v-if="comment.showReply">
                    <textarea class="form-control mt-2" v-model="comment.replyText" maxlength="256" v-if="canComment"></textarea>
                    <button class="btn btn-primary mt-2" @click="addReply(comment)" v-if="canComment">Postavi Odgovor</button>
                    <button class="btn btn-secondary mt-2" @click="cancelReply(comment)" v-if="canComment">Otkazi</button>
                    <button class="btn btn-info mt-2" @click="izmeniCom(comment)" v-if="canComment && window.loggedInUser.id == comment.autor.id">Izmeni</button>
                    <button class="btn btn-info mt-2" @click="obrisiCom(comment)" v-if="canComment && window.loggedInUser.id == comment.autor.id">Obrisi</button>
                </div>
                
                <comment-card 
                    class="card mt-2" 
                    v-for="(reply, index) in comment.odgovori" 
                    :key="index" 
                    :user="user" 
                    :comment="reply" 
                    @expand-com="$emit('expand-com', $event)"
                    @izmeni-com="$emit('izmeni-com', $event)"
                    @obrisi-com="$emit('obrisi-com', $event)"  
                    @toggle-reply="$emit('toggle-reply', $event)" 
                    @add-reply="$emit('add-reply', $event)" 
                    @cancel-reply="$emit('cancel-reply', $event)" 
                    v-if="comment.isExpanded">
                </comment-card>
            </div>
        </div>`,
    methods: {
        toggleReplyBox() {
            this.$emit('toggle-reply', this.comment);
        },
        addReply(comment) {
            this.$emit('add-reply', comment);
        },
        izmeniCom(comment) {
            this.$emit('izmeni-com', comment);
        },
        expandCom(comment) {
            this.$emit('expand-com', comment);
        },
        obrisiCom(comment) {
            this.$emit('obrisi-com', comment);
        },
        cancelReply(comment) {
            this.$emit('cancel-reply', comment);
        }
    },
    created: function() {
        
    }
}));


/*

window.commentComp = Vue.component('comment-card', {
    props: ['comment', 'canComment','user'],
    template: `
        <div class="card mb-2">
            <div class="card-body comBdy">
                <button class="btn clpsBtn" :class="{'btn-outline-secondary': comment.isExpanded}" @click="expandCom(comment)">
                    <span v-if="comment.isExpanded" style="font-size:19px;top:-7px;">&#9650;</span>
                    <span v-else>&#9654;</span>
                </button>
                <p class="card-text">{{ comment.sadrzaj }}</p>
                <button class="btn btn-secondary mt-2" @click="toggleReplyBox" v-if="canComment">Reply</button>

                
                <div v-if="comment.showReply">
                    <textarea class="form-control mt-2" v-model="comment.replyText" maxlength="256" v-if="canComment"></textarea>
                    <button class="btn btn-primary mt-2" @click="addReply(comment)" v-if="canComment">Post Reply</button>
                    <button class="btn btn-secondary mt-2" @click="cancelReply(comment)" v-if="canComment">Cancel</button>
                    <button class="btn btn-info mt-2" @click="izmeniCom(comment)" v-if="canComment && user.id == comment.autor.id">Izmeni</button>
                    <button class="btn btn-info mt-2" @click="obrisiCom(comment)" v-if="canComment && user.id == comment.autor.id">Obrisi</button>
                </div>
                
                <comment-card 
                    class="card mt-2" 
                    v-for="(reply, index) in comment.odgovori" 
                    :key="index" 
                    :user="user" 
                    :comment="reply" 
                    :can-comment="canComment" 
                    @expand-com="$emit('expand-com', $event)"
                    @izmeni-com="$emit('izmeni-com', $event)"
                    @obrisi-com="$emit('obrisi-com', $event)"  
                    @toggle-reply="$emit('toggle-reply', $event)" 
                    @add-reply="$emit('add-reply', $event)" 
                    @cancel-reply="$emit('cancel-reply', $event)" 
                    v-if="comment.isExpanded">
                </comment-card>
            </div>
        </div>`,
    methods: {
        toggleReplyBox() {
            this.$emit('toggle-reply', this.comment);
        },
        addReply(comment) {
            this.$emit('add-reply', comment);
        },
        izmeniCom(comment) {
            this.$emit('izmeni-com', comment);
        },
        expandCom(comment) {
            this.$emit('expand-com', comment);
        },
        obrisiCom(comment) {
            this.$emit('obrisi-com', comment);
        },
        cancelReply(comment) {
            this.$emit('cancel-reply', comment);
        }
    },
    created: function() {
        vueCompLink(this.$options.computed);
    }
});
*/