async function CheckAuthor(username) {

    var data;
 
    try {
        const response = await fetch('http://localhost:5253/api/user/' + username);
        data = await response.json(); // Assuming the response is JSON
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
            response => { return response.statusText() }
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

//async function AddAuthor(adddata) {
//    debugger;

//    adddata.AuthorID = 0;
//    adddata.DateCreated = '';
//    adddata.DateUpdated = '';
//    adddata.Admin = '';
//    adddata.Bio = '';

//    const response = await fetch("http://localhost:5253/api/author/add", {
//        method: 'POST',
//        body: adddata
//    });

//    data = await response.json(); // Assuming the response is JSON
//    console.log(data);

//}

function UpdateAuthorEventHandler(event) {

    event.preventDefault();


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
