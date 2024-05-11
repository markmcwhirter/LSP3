function AddSale(data) {

    const sale = JSON.parse(data);

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

    fetch(API_URL + "/api/author", options)
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

async function isUrlReachable(url) {
    try {
        const response = await fetch(url);
        return response.ok; // Check if the HTTP status code is in the 2xx range
    } catch (error) {
        return false; // Any error during fetch indicates unreachable
    }
}
