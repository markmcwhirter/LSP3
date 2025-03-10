async function CheckAuthor(username) {


    try {
        const response = await fetch('http://64.23.161.76:2173/api/' + username);
        var data = await response.json(); 
        console.log(data);
    } catch (error) {
        console.error('Error fetching data:', error);
    }

    return data;
}

function UpdateAuthor(data) {

    const author = JSON.parse(data);
    deleteFromObject('__RequestVerificationToken', author);

    let options = {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(author)
    };

    fetch(API_URL + "author/update", options)
        .then(
            response => { html => console.log(html) }
        ).then(
            html => console.log(html)
        );
}
function DeleteAuthor(id) {

    fetch(API_URL + "author/delete/" + id)
        .then(
            response => { return response.statusText() }
        ).then(
            html => console.log(html)
        );
}


function UpdateAuthorEventHandler(event) {

    event.preventDefault();

    if (document.getElementById("fieldset").disabled == true) {
        document.getElementById("fieldset").disabled = false;
        var elem = document.getElementById('UpdateButton');
        var txt = elem.textContent || elem.innerText;
        elem.innerText = 'Save';
        return;
    }

    if (!Validate('username', 'Please supply your username')) return false;
    if (!Validate('email', 'Please supply your email')) return false;

    if (!Validate('firstname', 'Please supply your first name')) return false;
    if (!Validate('lastname', 'Please supply your last name')) return false;
    var formData = new FormData(document.getElementById("authorform"));


    var data = JSON.stringify(Object.fromEntries(formData));

    var result = UpdateAuthor(data);
    alert('Author has been updated');

    return false;
}
