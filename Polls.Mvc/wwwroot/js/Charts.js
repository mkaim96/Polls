
let app = document.getElementById('app');

function createCanvas(id) {
    var canvWrapper = document.createElement('div');
    canvWrapper.setAttribute('class', 'canvWrapper');

    var canvas = document.createElement('canvas');
    canvas.setAttribute('id', id);

    canvWrapper.appendChild(canvas);
    return canvWrapper;
}

async function getStats() {

    let url = window.location.pathname;
    let urlArr = url.split('/');
    let id = urlArr[urlArr.length - 1];

    let res = await axios.get(`https://localhost:44392/stats/poll/${id}`);
    return res;
}

function transformChoicesCountToMap(choicesCount) {
    let props = Object.getOwnPropertyNames(choicesCount);
    let map = new Map();

    for (let prop of props) {
        map.set(prop, choicesCount[prop]);
    }

    return map;
}

function createDataObjectForChart(stat) {
    let data = {
        labels: [],
        datasets: [{
            data: [],
            backgroundColor: []
        }]

    };

    let choicesCount = transformChoicesCountToMap(stat.choicesCount);

    choicesCount.forEach((v, k) => {
        data.labels.push(k);
        data.datasets[0].data.push(v);
        data.datasets[0].backgroundColor.push(getRandomColor());
    });

    return data;
}

function createElementForQuestionWithoutAnswers(questionText) {
    let questionWrapper = document.createElement('div');
    questionWrapper.setAttribute('class', 'questionWrapper');

    let qText = document.createElement('h2');
    qText.innerText = questionText;

    let votesCountSmall = document.createElement("small");
    votesCountSmall.innerText = '(0 odpowiedzi)';

    questionWrapper.appendChild(qText);
    questionWrapper.appendChild(votesCountSmall);

    var h5 = document.createElement('p');
    h5.innerText = 'Na razie nie ma odpowiedzi na to pytanie.';
    questionWrapper.appendChild(h5);
    return questionWrapper;
}

function createElementForSingleChoiceQuestion(stat) {
    let questionWrapper = document.createElement('div');
    questionWrapper.setAttribute('class', 'questionWrapper');

    let qText = document.createElement('h2');
    qText.innerText = stat.question.questionText;

    let votesCountSmall = document.createElement("small");
    votesCountSmall.innerText = `(${stat.votesCount} odpowiedzi)`;

    let canvas = createCanvas(stat.question.id);

    let ctx = canvas.children[0].getContext('2d');

    let votesCount = stat.votesCount;

    // Construct chart oject for question
    let chart = new Chart(ctx, {
        type: 'pie',
        data: createDataObjectForChart(stat),
        options: {
            legend: {
                position: 'right'
            },
            tooltips: {
                callbacks: {
                    label: function (tooltipItem, object) {

                        let i = tooltipItem.index;
                        let dataset = object.datasets[0];

                        let percent = parseFloat((dataset.data[i] / votesCount) * 100).toFixed(1);

                        let labelText = `${object.labels[i]} - głosy: ${dataset.data[i]} (${percent}%)`;
                        return labelText;
                    }
                }
            }
        }
    });

    questionWrapper.appendChild(qText);
    questionWrapper.appendChild(votesCountSmall);
    questionWrapper.appendChild(canvas);

    return questionWrapper;
}

function createElementForMultipleChoiceQuestion(stat) {
    let questionWrapper = document.createElement('div');
    questionWrapper.setAttribute('class', 'questionWrapper');

    let qText = document.createElement('h2');
    qText.innerText = stat.question.questionText;

    let votesCountSmall = document.createElement("small");
    votesCountSmall.innerText = `(${stat.votesCount} odpowiedzi)`;

    let canvas = createCanvas(stat.question.id);

    let ctx = canvas.children[0].getContext('2d');

    let votesCount = stat.votesCount;

    // Construct chart oject for question
    let chart = new Chart(ctx, {
        type: 'horizontalBar',
        data: createDataObjectForChart(stat),
        options: {
            legend: {
                display: false
            },
            scales: {
                xAxes: [{
                    ticks: {
                        max: stat.votesCount,
                        min: 0,
                        stepSize: 1
                    }
                }]
            },

            tooltips: {
                callbacks: {
                    label: function (tooltipItem, object) {

                        let i = tooltipItem.index;
                        let dataset = object.datasets[0];

                        let percent = parseFloat((dataset.data[i] / votesCount) * 100).toFixed(1);

                        let labelText = `${object.labels[i]} - głosy: ${dataset.data[i]} (${percent}%)`;
                        return labelText;
                    },

                    title: function () { }
                }
            }
        }
    });

    questionWrapper.appendChild(qText);
    questionWrapper.appendChild(votesCountSmall);
    questionWrapper.appendChild(canvas);

    return questionWrapper;
}

function createElementForTextAnswerQuestion(stat) {
    let questionWrapper = document.createElement('div');
    questionWrapper.setAttribute('class', 'questionWrapper');

    let qText = document.createElement('h2');
    qText.innerText = stat.question.questionText;

    let answersCountSmall = document.createElement("small");
    answersCountSmall.innerText = `(${stat.answersCount} odpowiedzi)`;

    let answersList = document.createElement('ul');
    answersList.setAttribute('class', 'answers');

    let answers = [];

    for (let answer of stat.answers) {
        let liElement = document.createElement('li');
        liElement.setAttribute('class', 'answer');
        liElement.innerHTML = answer;
        answers.push(liElement);
    }

    // Add li's to list
    for (let answer of answers) {
        answersList.appendChild(answer);
    }

    questionWrapper.appendChild(qText);
    questionWrapper.appendChild(answersCountSmall);
    questionWrapper.appendChild(answersList);

    return questionWrapper;
}

function getRandomColor() {
    var letters = '0123456789ABCDEF';
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}

async function main() {
    let stats = await getStats();

    let elements = [];

    // Create html element for each question
    for (var stat of stats.data) {
        if (stat.votesCount === 0) {
            elements.push(createElementForQuestionWithoutAnswers(stat.question.questionText));
            continue;
        }
        switch (stat.question.questionType) {
            case 'SingleChoice':
                elements.push(createElementForSingleChoiceQuestion(stat));
                break;
            case 'MultipleChoice':
                elements.push(createElementForMultipleChoiceQuestion(stat));
                break;
            case 'TextAnswer':
                elements.push(createElementForTextAnswerQuestion(stat));
                break;
            default:
                console.log("Nieznany typ pytania");
        }
    }

    // Add each element to DOM
    for (let el of elements) {
        app.appendChild(el);
    }
}

main();  
