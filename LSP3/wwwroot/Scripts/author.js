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
function UpdateAuthor(data) {

    const author = JSON.parse(data);

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

function deleteFromObject(keyPart, obj) {
    for (var k in obj) {          // Loop through the object
        if (~k.indexOf(keyPart)) { // If the current key contains the string we're looking for
            delete obj[k];       // Delete obj[key];
        }
    }
}
