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

    fetch(API_URL + "author", options)
        .then(
            response => { return response.statusText() }
        ).then(
            html => console.log(html)
        );
}

