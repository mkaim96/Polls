var app = new Vue({
    el: '#app',
    data: {
        poll: {
            pollId: 0,
            title: 'Formularz bez tytułu',
            description: '',
            questions: []
        },
        questionsToDelete: [],
        newQuestions: []
    },
    methods: {
        addChoice(question) {
            question.choices.push("Choice " + (question.choices.length + 1));
        },

        deleteChoice(question, index) {
            question.choices.splice(index, 1);
        },

        addSingleChoiceQuestion() {
            let q = { questionText: 'Question', questionType: 0, choices: ['Choice 1'], Number: 0}
            this.newQuestions.push(q);
        },

        addTextAnswerQuestion() {
            let q = {questionText: 'Question', questionType: 1, Number: 0};
            this.newQuestions.push(q);
        },

        deleteQuestion(question) {
            // check if it is new question
            let i = this.newQuestions.findIndex(x => x === question);

            // if it is just delete it from newQuestions array
            if(i != -1)
            {
                this.newQuestions.splice(i, 1);
                return;      
            }

            var index = this.poll.questions.indexOf(question);

            this.questionsToDelete.push(question);
            this.poll.questions.splice(index, 1);
        },

        giveQuestionANumber() {
            var counter = 1;
            this.allQuestions.forEach(x => {
                x.Number = counter;
                counter += 1;
            });
        },

        save() {
            this.giveQuestionANumber();
            // construct request object
            let request = {
                PollId: this.poll.pollId,
                NewTitle: this.poll.title,
                NewDescription: this.poll.description,

                ScQuestionsToUpdate: this.poll.questions.filter(x => x.questionType === 0),
                TaQuestionsToUpdate: this.poll.questions.filter(x => x.questionType === 1),

                NewScQuestions: this.newQuestions.filter(x => x.questionType === 0),
                NewTaQuestions: this.newQuestions.filter(x => x.questionType === 1),

                ScQuestionsToDelete: this.questionsToDelete.filter(x => x.questionType === 0),
                TaQuestionsToDelete: this.questionsToDelete.filter(x => x.questionType === 1)
            }

            console.log(request);

            axios({
                method: 'post',
                url: 'https://localhost:44392/polls/edit',
                data: request
            }).then(res => {
                console.log(res);
            })
        }
    },

    computed: {
        allQuestions() {
            return this.poll.questions.concat(this.newQuestions);
        }
    },

    beforeCreate() {
        var url = new URL(window.location.href);

        // get id from query string
        var params = url.searchParams;
        var id = parseInt(params.get('id'));

        //get poll to edit and populate poll in data object
        axios({
            method: 'get',
            url: `https://localhost:44392/polls/edit/poll/${id}`
        }).then(res => {
            this.poll.pollId = res.data.pollId;
            this.poll.title = res.data.title;
            this.poll.description = res.data.description;
            this.poll.questions = res.data.questions;
        });
    
    }
});