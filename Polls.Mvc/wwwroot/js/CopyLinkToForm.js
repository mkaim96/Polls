let copyBtn = document.getElementById('copy');

let url = window.location.href;

let urlArr = url.split("/");
let id = urlArr[urlArr.length - 1];


let urlToCopy = `https://localhost:44392/polls/form/${id}`;

let input = document.getElementById('link');

input.value = urlToCopy;

copyBtn.addEventListener('click', function() {
    input.select();
    document.execCommand('copy');   
})