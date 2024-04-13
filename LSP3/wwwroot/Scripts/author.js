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

    fetch("http://localhost:5253/api/author/update", options)
        .then(
            response => { return response.statusText() }
        ).then(
            html => console.log(html)
        );
}

function AddAuthor(data) {

    const author = JSON.parse(data);
    deleteFromObject('__RequestVerificationToken', author);

    author.AuthorID = 0;
    author.DateCreated = '';
    author.DateUpdated = '';
    author.Admin = '';
    author.Bio = '';

    let options = {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(author)
    };

    fetch("http://localhost:5253/api/author", options)
        .then(
            response => { return response.statusText() }
        ).then(
            html => console.log(html)
        );
}

function UpdateAuthorEventHandler(event) {

    event.preventDefault();

    debugger
    if (document.getElementById("fieldset").disabled == true) {
        document.getElementById("fieldset").disabled = false;
        var elem = document.getElementById('UpdateButton');
        var txt = elem.textContent || elem.innerText;
        elem.innerText = 'Save';
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

function Validate(field, message) {
    var testvalue = document.getElementById(field).value;

    if (testvalue == null || testvalue == '') {
        document.getElementById('lblError').innerHTML = message;
        return false;
    }
    return true;
}

function UpdateAuthor(data) {

    const author = JSON.parse(data);

    let options = {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(author)
    };

    fetch("http://localhost:5253/api/author/update", options)
        .then(
            response => { return response.statusText() }
        ).then(
            html => console.log(html)
        );
}

/*
function UpdateAuthor(data) {

    const author = JSON.parse(data);
    deleteFromObject('__RequestVerificationToken', author);

    author.AuthorID = 0;
    author.DateCreated = '';
    author.DateUpdated = '';
    author.Admin = '';
    author.Bio = '';

    let options = {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(author)
    };

    fetch("http://localhost:5253/api/author/update", options)
        .then(
            response => { return response.statusText() }
        ).then(
            html => console.log(html)
        );
}
*/

function deleteFromObject(keyPart, obj) {
    for (var k in obj) {          // Loop through the object
        if (~k.indexOf(keyPart)) { // If the current key contains the string we're looking for
            delete obj[k];       // Delete obj[key];
        }
    }
}
