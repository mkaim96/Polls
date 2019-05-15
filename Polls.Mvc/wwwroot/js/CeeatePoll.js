var app = new Vue({
    el: '#app',
    data: {
        poll: {
            title: 'Formularz bez tytułu',
            description: '',
            questions: [
                { "QuestionText": "Question text", "QuestionType": "SingleChoice", "Choices": ["choice 1", "choice 2", "choice 3"], "Number": 1 },
            ]
        }
    },
    methods: {
        addChoice(question) {
            question.Choices.push("Choice " + (question.Choices.length + 1));
        },

        deleteChoice(question, index) {
            question.Choices.splice(index, 1);
        },

        addSingleChoiceQuestion() {
            this.poll.questions.push({
                QuestionText: 'Question',
                QuestionType: 'SingleChoice',
                Choices: ['Choice 1'],
                Number: 0
            });
        },

        addTextAnswerQuestion() {
            this.poll.questions.push({
                QuestionText: 'Question',
                QuestionType: 'TextAnswer',
                Number: 0
            });
        },

        deleteQuestion(question) {
            var index = this.poll.questions.indexOf(question);
            this.poll.questions.splice(index, 1);
        },

        giveQuestionANumber() {
            var counter = 1;
            this.poll.questions.forEach(x => {
                x.Number = counter;
                counter += 1;
            });
        },

        save() {
            this.giveQuestionANumber();

            // prepare request object
            var request = {
                userId: '',
                title: this.poll.title,
                description: this.poll.description,
                singleChoiceQuestions: this.poll.questions.filter(x => x.QuestionType === 'SingleChoice'),
                textAnswerQuestions: this.poll.questions.filter(x => x.QuestionType === 'TextAnswer')
            };

            axios({
                method: 'post',
                url: 'https://localhost:44392/polls/create',
                data: request
            }).then(res => {
                if (res.status === 200) {
                    window.location.href = "https://localhost:44392";
                }
            }).catch(err => {
                alert("Failed to save poll to database, try again later.");
            });
        }
    }
});