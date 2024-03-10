
window.vueStart = {
    data: {

        sveIndustrije: {'Computer Science':["Web Dev","Game Dev"],'Manual Labor':[]},  //TODO make longer
        
        enteredDistance: 2000,
        locationSuggestions: [],

        // commenting {
        comments: [],
        sveOcene: [],
        avgOcena: 0,
        ocene: [],
        newComment: '',
        newOcena: '',
        newOcena_1: 5,
        newOcena_2: 5,
        newOcena_3: 5,
        newOcena_4: 5,
        currentComment: null,
        currentOcena: null,
        mojaOcena: null,

        // commenting }
        // job search {
        search: '',
        nazivPosla: '', //Job title
        nazivPoslodavca: '', //Company
        vrstaPosla: 'Any', //Experience level
        tipPosla: 'Any', //Part or Full time
        plataOd: 0, //Salary range min
        plataDo: 500000, //Salary range max
        plataValuta: 'Any', //Currency type
        sektorRada: '', //Sector
        lokacija: '',
        industrija: '', //Industry
        
        showMap: true,

        jobListRef: 0,
        selectedJob: 0,
        locations: [],
        jobTitles: ['Human Resources', 'Software Developer', 'Customer Support',
            "Software Developer",
            "Game Developer",
            "Web Developer",
            "Frontend Developer",
            "Backend Developer",
            "Full Stack Developer",
            "Python Developer",
            "Java Developer",
            "C++ Developer",
            "JavaScript Developer",
            "Ruby Developer",
            "DevOps Engineer",
            "Database Administrator",
            "System Administrator",
            "Network Administrator",
            "Data Analyst",
            "Data Scientist",
            "Machine Learning Engineer",
            "Artificial Intelligence Engineer",
            "Cloud Architect",
            "Cyber Security Analyst",
            "Information Security Analyst",
            "QA Engineer",
            "Test Engineer",
            "Software Tester",
            "UI/UX Designer",
            "Graphic Designer",
            "Digital Designer",
            "Product Manager",
            "Project Manager",
            "Scrum Master",
            "Business Analyst",
            "SEO Specialist",
            "IT Support Specialist",
            "Technical Support Engineer",
            "IT Consultant",
            "Technical Writer",
            "Blockchain Developer",
            "iOS Developer",
            "Android Developer",
            "Mobile App Developer",
            "Augmented Reality Developer",
            "Virtual Reality Developer",
            "Ethereum Smart Contract Developer",
            "Cryptocurrency Analyst",
            "Data Engineer",
            "Computer Vision Engineer",
            "NLP Engineer",
            "Quantum Computing Scientist",
            "Firmware Engineer",
            "Embedded Systems Engineer",
            "Information Systems Manager",
            "Data Center Engineer",
            "IT Auditor",
            "Cybersecurity Consultant",
            "Penetration Tester",
            "Game Designer",
            "Multimedia Artist",
            "3D Animator",
            "Hardware Engineer",
            "Systems Analyst",
            "Network Engineer",
            "Cloud Solutions Engineer",
            "E-commerce Specialist",
            "CRM Developer",
            "Salesforce Developer",
            "Dynamics 365 Developer",
            "Cloud Security Engineer",
            "IT Project Coordinator",
            "Application Support Analyst",
            "Infrastructure Engineer",
            "Release Manager",
            "SAP Consultant",
            "Big Data Engineer",
            "Robotics Engineer",
            "IoT Engineer",
            "Software Architect",
            "Cloud Consultant",
            "IT Director",
            "IT Manager",
            "RPA Developer",
            "Storage Engineer",
            "Video Game Tester",
            "Bioinformatics Specialist",
            "Ethical Hacker",
            "Digital Forensics Expert",
            "Incident Response Analyst",
            "SEO Analyst",
            "Content Strategist",
            "Data Visualization Specialist",
            "Instructional Designer",
            "IT Coordinator",
            "IT Business Analyst",
            "Service Delivery Manager",
            "API Developer",
            "Cloud Product Manager",
            "Azure Cloud Architect",
            "AWS Solutions Architect",
            "Google Cloud Engineer",
            "Hadoop Developer",
            "Drupal Developer",
            "Magento Developer"
        
        ],  //TODO make longer
        industrije: ['Computer Science','Manual Labor',    "Computer Science",
        "Information Technology",
        "Software Development",
        "Web Development",
        "Mobile App Development",
        "Database Management",
        "Data Science",
        "Machine Learning",
        "Artificial Intelligence",
        "Cloud Computing",
        "Cybersecurity",
        "Networking",
        "Hardware Engineering",
        "Game Development",
        "Virtual Reality",
        "Augmented Reality",
        "Internet of Things (IoT)",
        "Robotics",
        "E-commerce",
        "SEO",
        "Digital Marketing",
        "IT Consulting",
        "IT Services",
        "Health Informatics",
        "Financial Technology (Fintech)",
        "Business Intelligence",
        "Enterprise Software",
        "Customer Relationship Management (CRM)",
        "Social Media",
        "Search Engine",
        "Content Management System (CMS)",
        "Data Center",
        "Telecommunications",
        "Video Conferencing",
        "Messaging Software",
        "Computer Graphics",
        "Animation",
        "User Interface Design",
        "User Experience Design",
        "Instructional Technology",
        "Blockchain",
        "Cryptocurrency",
        "Quantum Computing",
        "Education Technology (EdTech)",
        "Bioinformatics",
        "3D Printing",
        "Digital Forensics",
        "Data Recovery",
        "Wearable Technology",
        "IT Support Services",
        "Software Testing",
        "Digital Publishing",
        "Video Editing",
        "Music Technology",
        "Cloud Storage",
        "Software as a Service (SaaS)",
        "Platform as a Service (PaaS)",
        "Infrastructure as a Service (IaaS)",
        "Network Security",
        "Web Hosting",
        "Data Analysis",
        "Big Data",
        "API Management",
        "IT Training",
        "Freelance IT Services",
        "Embedded Systems",
        "Software Architecture",
        "Technical Writing",
        "Project Management Software",
        "Digital Signage",
        "Wireless Networking",
        "IT Compliance",
        "IT Auditing",
        "IT Asset Management",
        "IT Disaster Recovery",
        "ERP Systems",
        "Internet Security",
        "IT Outsourcing",
        "ICT in Agriculture",
        "ICT in Manufacturing",
        "ICT in Retail",
        "ICT in Healthcare",
        "ICT in Transportation",
        "ICT in Finance",
        "ICT in Education",
        "ICT in Construction",
        "ICT in Energy",
        "ICT in Real Estate",
        "ICT in Entertainment",
        "ICT in Tourism",
        "ICT in Food Service",
        "ICT in Non-profit",
        "ICT in Government",
        "ICT in Public Safety",
        "ICT in Science and Research",
        "ICT in Environmental Services",
        "ICT in Human Resources"],  //TODO make longer
        sektori: ["Web Dev","Game Dev",'Human Resources', 'Software Developer', 'Customer Support',
        "Software Developer",
        "Game Developer",
        "Web Developer",
        "Frontend Developer",
        "Backend Developer",
        "Full Stack Developer",
        "Python Developer",
        "Java Developer",
        "C++ Developer",
        "JavaScript Developer",
        "Ruby Developer",
        "DevOps Engineer",
        "Database Administrator",
        "System Administrator",
        "Network Administrator",
        "Data Analyst",
        "Data Scientist",
        "Machine Learning Engineer",
        "Artificial Intelligence Engineer",
        "Cloud Architect",
        "Cyber Security Analyst",
        "Information Security Analyst",
        "QA Engineer",
        "Test Engineer",
        "Software Tester",
        "UI/UX Designer",
        "Graphic Designer",
        "Digital Designer",
        "Product Manager",
        "Project Manager",
        "Scrum Master",
        "Business Analyst",
        "SEO Specialist",
        "IT Support Specialist",
        "Technical Support Engineer",
        "IT Consultant",
        "Technical Writer",
        "Blockchain Developer",
        "iOS Developer",
        "Android Developer",
        "Mobile App Developer",
        "Augmented Reality Developer",
        "Virtual Reality Developer",
        "Ethereum Smart Contract Developer",
        "Cryptocurrency Analyst",
        "Data Engineer",
        "Computer Vision Engineer",
        "NLP Engineer",
        "Quantum Computing Scientist",
        "Firmware Engineer",
        "Embedded Systems Engineer",
        "Information Systems Manager",
        "Data Center Engineer",
        "IT Auditor",
        "Cybersecurity Consultant",
        "Penetration Tester",
        "Game Designer",
        "Multimedia Artist",
        "3D Animator",
        "Hardware Engineer",
        "Systems Analyst",
        "Network Engineer",
        "Cloud Solutions Engineer",
        "E-commerce Specialist",
        "CRM Developer",
        "Salesforce Developer",
        "Dynamics 365 Developer",
        "Cloud Security Engineer",
        "IT Project Coordinator",
        "Application Support Analyst",
        "Infrastructure Engineer",
        "Release Manager",
        "SAP Consultant",
        "Big Data Engineer",
        "Robotics Engineer",
        "IoT Engineer",
        "Software Architect",
        "Cloud Consultant",
        "IT Director",
        "IT Manager",
        "RPA Developer",
        "Storage Engineer",
        "Video Game Tester",
        "Bioinformatics Specialist",
        "Ethical Hacker",
        "Digital Forensics Expert",
        "Incident Response Analyst",
        "SEO Analyst",
        "Content Strategist",
        "Data Visualization Specialist",
        "Instructional Designer",
        "IT Coordinator",
        "IT Business Analyst",
        "Service Delivery Manager",
        "API Developer",
        "Cloud Product Manager",
        "Azure Cloud Architect",
        "AWS Solutions Architect",
        "Google Cloud Engineer",
        "Hadoop Developer",
        "Drupal Developer",
        "Magento Developer"
    ,'Computer Science','Manual Labor',    "Computer Science",
    "Information Technology",
    "Software Development",
    "Web Development",
    "Mobile App Development",
    "Database Management",
    "Data Science",
    "Machine Learning",
    "Artificial Intelligence",
    "Cloud Computing",
    "Cybersecurity",
    "Networking",
    "Hardware Engineering",
    "Game Development",
    "Virtual Reality",
    "Augmented Reality",
    "Internet of Things (IoT)",
    "Robotics",
    "E-commerce",
    "SEO",
    "Digital Marketing",
    "IT Consulting",
    "IT Services",
    "Health Informatics",
    "Financial Technology (Fintech)",
    "Business Intelligence",
    "Enterprise Software",
    "Customer Relationship Management (CRM)",
    "Social Media",
    "Search Engine",
    "Content Management System (CMS)",
    "Data Center",
    "Telecommunications",
    "Video Conferencing",
    "Messaging Software",
    "Computer Graphics",
    "Animation",
    "User Interface Design",
    "User Experience Design",
    "Instructional Technology",
    "Blockchain",
    "Cryptocurrency",
    "Quantum Computing",
    "Education Technology (EdTech)",
    "Bioinformatics",
    "3D Printing",
    "Digital Forensics",
    "Data Recovery",
    "Wearable Technology",
    "IT Support Services",
    "Software Testing",
    "Digital Publishing",
    "Video Editing",
    "Music Technology",
    "Cloud Storage",
    "Software as a Service (SaaS)",
    "Platform as a Service (PaaS)",
    "Infrastructure as a Service (IaaS)",
    "Network Security",
    "Web Hosting",
    "Data Analysis",
    "Big Data",
    "API Management",
    "IT Training",
    "Freelance IT Services",
    "Embedded Systems",
    "Software Architecture",
    "Technical Writing",
    "Project Management Software",
    "Digital Signage",
    "Wireless Networking",
    "IT Compliance",
    "IT Auditing",
    "IT Asset Management",
    "IT Disaster Recovery",
    "ERP Systems",
    "Internet Security",
    "IT Outsourcing",
    "ICT in Agriculture",
    "ICT in Manufacturing",
    "ICT in Retail",
    "ICT in Healthcare",
    "ICT in Transportation",
    "ICT in Finance",
    "ICT in Education",
    "ICT in Construction",
    "ICT in Energy",
    "ICT in Real Estate",
    "ICT in Entertainment",
    "ICT in Tourism",
    "ICT in Food Service",
    "ICT in Non-profit",
    "ICT in Government",
    "ICT in Public Safety",
    "ICT in Science and Research",
    "ICT in Environmental Services",
    "ICT in Human Resources"],
        companies: ['OpenAI', 'Google', 'Uber'],  //TODO make dynamic
        jobs: [
            /*
            {
                poslodavac: {ime:"Boban",prezime:"Bobic",id:3},
                naziv: 'Software Developer',
                vrstaPosla: 'Junior',
                lokacija: 'Serbia, Belgrade',
                plataOd: 3000,
                plataDo: 4500,
                plataValuta: "EUR",
                plataInfo: "mesecno",
                opisPosla: 'This is a description of the job. It contains information about the job requirements, responsibilities and more.'
            },
            {
                poslodavac: {ime:"Boban",prezime:"Bobic",nazivFirme:"Microsoft"},
                naziv: 'Software Developer',
                vrstaPosla: 'Senior',
                lokacija: 'Bulgaria, Sofia',
                plataOd: 4500,
                plataDo: 4500,
                plataValuta: "RSD",
                plataInfo: "dnevno",
                opisPosla: 'This is another description of the job. It contains information about the job requirements, responsibilities and more.'
            }
            */
        ]
        
        // job search }
    },
    computed: {

        window: () => window,
/*
        loggedInUser: {
            cache:false,
            get:function(){
            
                //return {ime:"Luka",prezime:"Bobanovic"};
                return window.loggedInUser;
            
            },
        },
*/
        loggedInUser: function(){
            //cache:false,
            //get:function(){
              console.log("GET USER ",window.loggedInUser);
                //return {ime:"Luka",prezime:"Bobanovic"};
                return window.loggedInUser;
            
            //},
        },
        canComment : function(){
            if(this.loggedInUser){
                if(this.loggedInUser.uloga == 0){
                    return true;
                }
            }else return false;
        },
        salaryRange: {
            get() {
                return [this.plataOd, this.plataDo];
            },
            set(value) {
                this.plataOd = value[0];
                this.plataDo = value[1];
            }
        }
    },
    methods: {
        clearJobs(){
            this.jobs = this.jobs.filter(j=>j.id==-99);
        },

        toggleReplyBox(comment) {
            if(this.currentComment && this.currentComment !== comment) {
                this.currentComment.showReply = false;
            }

            comment.showReply = true;
            this.currentComment = comment;
        },
        expandCom(comment,newState=undefined,onDoneCB=undefined){
            console.log("EXPAND",comment);
            
            if(newState===undefined)
                comment.isExpanded = !comment.isExpanded;
            else comment.isExpanded=newState;
            if(onDoneCB===undefined){
                collapseComments(comment.isExpanded, comment, 1);
                return;
            }
            if(comment.isExpanded){
                console.log("Loading replies of ",comment.sadrzaj);
                Req("GET","/Komentar/VratiKomentareKomentara").query({KomentarId:comment.id}).onDone(function(r){
                    //alert(r.rt);
                    //console.log(r.rt);
                    //alert('vidi konzolu i reci mi output.');

                    console.log(JSON.parse(r.rt));
                    comment.odgovori = first(JSON.parse(r.rt)).map(parseCom);
                    if(onDoneCB) onDoneCB(comment);
                }).send();
            }
        },
        addReply(comment) {
            console.log("REPLYING TO COM ",comment);
            let body = {
                sadrzaj: comment.replyText,
                komentarId: comment.id
            };
            Req("POST","/Komentar/DodajKomentar").json(body).onDone((r)=>{
                //alert(r.rt);
                console.log(r);
                //alert('vidi konzolu i reci mi output.');
                let kom = parseCom(JSON.parse(r.rt));
                //window.vApp.comments = JSON.parse(r.rt).map(parseCom);


                comment.odgovori.push(kom);

                comment.isExpanded = true;
                comment.replyText = '';
                comment.showReply = false;
                this.currentComment = null;
            }).send();
            
        },
        cancelReply(comment) {
            comment.showReply = false;
            this.currentComment = null;
        },
        addComment() {
            if(!this.selectedJob) throw new Error("No selected job.");
            let body = {
                sadrzaj: this.newComment,
                posaoId: this.selectedJob.id
            };
            Req("POST","/Komentar/DodajKomentar").json(body).onDone((r)=>{
                //alert(r.rt);
                console.log(r);
                //alert('vidi konzolu i reci mi output.');
                let kom = parseCom(JSON.parse(r.rt));

                this.comments.push(kom);

                this.newComment = '';
            }).send();
            
        },
        addOcena() {
            if(!this.selectedJob) throw new Error("No selected job.");
            let body = {
                posaoId: this.selectedJob.id,
                ocenaPlata: this.newOcena_1,
                ocenaOkruzenje: this.newOcena_2,
                ocenaNapredovanje: this.newOcena_3,
                ocenaPosoKuca: this.newOcena_4,
            };
            console.log(body);
            Req("PUT","/Posao/DodajOcenu").query(body).onDone((r)=>{
                //alert(r.rt);
                console.log(r);
                //alert('vidi konzolu i reci mi output.');
                let kom = (JSON.parse(r.rt));
                console.log(kom);
                this.mojaOcena = kom;
                this.newOcena_1 = kom.plata;
                this.newOcena_2 = kom.okruzenje;
                this.newOcena_3 = kom.napredovanje;
                this.newOcena_4 = kom.posoKuca;
                //this.sveOcene.unshift(kom);

                //this.newComment = '';
            }).send();
        },

        getPoslodavacName(poslodavac){
            if(!poslodavac) return "";
            if(poslodavac.nazivFirme) return poslodavac.nazivFirme;
            return `${poslodavac.ime+" "+poslodavac.prezime}`;
        },

        getLocation: function() {
            // this.locations = fetchLocationsFromAPI(this.location);
        },

        selectNewJob: function(job){
            console.log("SELECTING JOB ",job);
            this.avgOcena = 0;
            this.sveOcene = [];
            this.ocene = [];
            this.comments = [];
            this.selectedJob = job;
            this.mojaOcena = null;
            this.newOcena_1 = 5;
            this.newOcena_2 = 5;
            this.newOcena_3 = 5;
            this.newOcena_4 = 5;
            reqComms();
        },
    }
}