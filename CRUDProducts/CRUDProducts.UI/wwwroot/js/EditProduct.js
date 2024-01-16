const form = document.querySelector("form");
const input = document.querySelector("#numeroConComa");

LoadEventListeners();

function LoadEventListeners() {

    form.addEventListener("submit", Convert);
}

function Convert() {

    let result = input.value;

    result = result.replace(",", ".");

    input.value = result;

    console.log(input.value);
}
