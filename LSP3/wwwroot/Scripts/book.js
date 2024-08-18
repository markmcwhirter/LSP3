
function evaluateInputAndUpdateButton() {
    const inputElement = document.getElementById("bookid");
    const buttonElement = document.getElementById("UpdateButton");

    const inputValue = inputElement.value;

    if (inputValue === null || inputValue.trim() === "" || inputValue.trim() === "0") {
        buttonElement.innerHTML = "Add";
    } else {
        buttonElement.innerHTML = "Update";
    }
}


function AddBook(formdata) {

    var data = JSON.stringify(Object.fromEntries(formdata));

    const book = JSON.parse(data);
    deleteFromObject('__RequestVerificationToken', book);

    let options = {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(book)
    };

    fetch(API_URL + "book/update", options)
        .then(
            response => { return 'Success'; }
        ).then(
            html => console.log(html)
        );
}

function validateForm() {

    var checkbox = document.getElementById("tandc");
    if (checkbox == null || !checkbox.checked) {
        alert("Please check the checkbox before submitting.");
        return false; // Prevent form submission
    }
    return true; // Allow form submission
}

function CancelBookEventHandler() {

    event.preventDefault();

    return false;

}
