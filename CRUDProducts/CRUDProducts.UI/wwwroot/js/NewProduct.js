const form = document.querySelector("form");
const input_name = document.querySelector("#NAME_PRO");
const input_stock = document.querySelector("#STOCK_PRO");
const input_value = document.querySelector("#numeroConComa");

const checkName = /^[a-zA-Z ]+$/;
const checkStock = /^[1-9]\d*$/;
const checkValue = /^[1-9]\d*([\.,]\d+)?$/;

addEventListener();

function addEventListener() {

    form.addEventListener("submit", NewProducto);
}

function NewProducto(e) {

    e.preventDefault();
 
    if (!checkName.test(input_name.value)) {

        Swal.fire({
            title: "Oop",
            text: "Enter a product valid",
            icon: "error"
        });

        return;
    }

    if (!checkStock.test(input_stock.value)) {

        Swal.fire({
            title: "Oop",
            text: "Enter a stock greater than 0",
            icon: "error"
        });
    }

    if (!checkValue.test(input_value.value)) {

        Swal.fire({
            title: "Oop",
            text: "Enter a value greater than 0",
            icon: "error"
        });
    }

    if (checkName.test(input_name.value) && checkStock.test(input_stock.value) && checkValue.test(input_value.value)) {

        let result = input_value.value;

        result = result.replace(",", ".");

        input_value.value = result;

        fetch('/Products/NewProduct', {
            method: 'POST',
            body: new FormData(form),
        })
        .then(response => {
            if (response.ok) {
                return response.json();
            } else {
                throw new Error('Network response was not ok.');
            }
        })
        .then(data => {

            if (data.success) {
                Swal.fire({
                    title: "Great",
                    text: "New product introduced",
                    icon: "success"
                }).then(() => {
                    window.location.href = '/Products/Index';
                });
            }
            else {
                Swal.fire({
                    title: "Error",
                    text: "The product already exist",
                    icon: "error"
                });
            }
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
            Swal.fire("Ocurrió un error al intentar agregar el producto " + error);
        });
    }

}